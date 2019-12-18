using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using System.Xml;

namespace NextBusDesktop.DataProvider
{
    public static class TripPlannerProviderContainer
    {
        private static ITripPlannerProviderAsync _tripPlannerProvider;
        public static ITripPlannerProviderAsync TripPlannerProvider
        {
            get => _tripPlannerProvider;
            private set => _tripPlannerProvider = value;
        }

        public static async Task Initialize()
        {
            // TODO: Make less ugly
            IAccessTokenProviderAsync accessTokenProvider = new AccessTokenProvider();
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (!File.Exists(storageFolder.Path + "\\CurrentAccessToken.txt"))
            {
                Windows.Storage.StorageFile newFile = await storageFolder.CreateFileAsync("CurrentAccessToken.txt", Windows.Storage.CreationCollisionOption.FailIfExists);
                AccessTokenResponse newAccessToken = await accessTokenProvider.GetAccessTokenAsync();
                newAccessToken.CreatedDateTime = DateTime.Now;
                string serialized = Serialize(newAccessToken);
                await Windows.Storage.FileIO.WriteTextAsync(newFile, serialized);
                _tripPlannerProvider = new TripPlannerProvider(newAccessToken);
            }
            else
            {
                Windows.Storage.StorageFile retrievedFile = await storageFolder.GetFileAsync("CurrentAccessToken.txt");
                var data = await Windows.Storage.FileIO.ReadLinesAsync(retrievedFile);
                string[] lines = data.ToArray();

                AccessTokenResponse lastSessionAccessToken = Deserialize(lines);
                if (lastSessionAccessToken.ExpiresDateTime < DateTime.Now)
                {
                    AccessTokenResponse newAccessToken = await accessTokenProvider.GetAccessTokenAsync();
                    newAccessToken.CreatedDateTime = DateTime.Now;
                    string serialized = Serialize(newAccessToken);
                    await Windows.Storage.FileIO.WriteTextAsync(retrievedFile, serialized);
                    _tripPlannerProvider = new TripPlannerProvider(newAccessToken);
                }
                else
                {
                    _tripPlannerProvider = new TripPlannerProvider(lastSessionAccessToken);
                }
            }
        }

        private static string Serialize(AccessTokenResponse accessToken)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(accessToken.Scope)}: {accessToken.Scope}");
            stringBuilder.AppendLine($"{nameof(accessToken.TokenType)}: {accessToken.TokenType}");
            stringBuilder.AppendLine($"{nameof(accessToken.Expires)}: {accessToken.Expires}");
            stringBuilder.AppendLine($"{nameof(accessToken.Token)}: {accessToken.Token}");
            stringBuilder.AppendLine($"{nameof(accessToken.CreatedDateTime)}: {accessToken.CreatedDateTime.ToString("yyyyMMddHHmmss")}");
            return stringBuilder.ToString();
        }

        private static AccessTokenResponse Deserialize(string[] values)
        {
            if (values is null || values.Count() is 0)
                return null;

            AccessTokenResponse accessToken = new AccessTokenResponse();
            foreach (string kvp in values)
            {
                string[] keyValuePair = kvp.Split(':').Select(v => v.Trim()).ToArray();
                string key = keyValuePair[0];
                string value = keyValuePair[1];
                switch (key)
                {
                    case "Scope":
                        accessToken.Scope = value;
                        break;
                    case "TokenType":
                        accessToken.TokenType = value;
                        break;
                    case "Expires":
                        accessToken.Expires = int.Parse(value);
                        break;
                    case "Token":
                        accessToken.Token = Guid.Parse(value);
                        break;
                    case "CreatedDateTime":
                        accessToken.CreatedDateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return accessToken;
        }
    }
}
