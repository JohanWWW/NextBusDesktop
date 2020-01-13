using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.Models;
using System.Xml;
using Windows.Storage;

namespace NextBusDesktop.DataProvider
{
    public static class TripPlannerProviderContainer
    {
        private static ITripPlannerProvider _tripPlannerProvider;
        public static ITripPlannerProvider TripPlannerProvider
        {
            get => _tripPlannerProvider;
            private set => _tripPlannerProvider = value;
        }

        public static async Task Initialize()
        {
            Log("Initializing TripPlannerProvider");
            IAccessTokenProvider accessTokenProvider = new AccessTokenProvider();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile accessTokenFile;
            bool fileIsEmpty;

            if (!File.Exists(storageFolder.Path + "\\CurrentAccessToken.txt"))
            {
                fileIsEmpty = true;
                accessTokenFile = await storageFolder.CreateFileAsync("CurrentAccessToken.txt", CreationCollisionOption.FailIfExists);
                Log("Write -> Created new access token file.");
            }
            else
            {
                fileIsEmpty = false;
                accessTokenFile = await storageFolder.GetFileAsync("CurrentAccessToken.txt");
                Log("Read -> Load access token from file.");
            }

            AccessToken token;

            Func<Task<AccessToken>> createNewToken = async () =>
            {
                AccessToken newAccessToken = await accessTokenProvider.GetAccessTokenAsync();
                string serialized = Serialize(newAccessToken);
                await FileIO.WriteTextAsync(accessTokenFile, serialized);
                Log("Write -> Save access token to file.");
                return newAccessToken;
            };

            if (fileIsEmpty) // File is empty because the file was just created.
            {
                token = await createNewToken();
                Log("Could not find access token on file. Requesting new access token.");
            }
            else
            {
                var data = await FileIO.ReadLinesAsync(accessTokenFile);
                string[] serializedRetrieved = data.ToArray();
                AccessToken retrievedAccessToken = Deserialize(serializedRetrieved);
                if (retrievedAccessToken.ExpiresDateTime < DateTime.Now)
                    token = await createNewToken();
                else
                    token = retrievedAccessToken;
            }

            _tripPlannerProvider = new TripPlannerProvider(token);

            Log("Initializing complete.");
        }

        private static string Serialize(AccessToken accessToken)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(accessToken.Scope)}: {accessToken.Scope}");
            stringBuilder.AppendLine($"{nameof(accessToken.Type)}: {accessToken.Type}");
            stringBuilder.AppendLine($"{nameof(accessToken.Token)}: {accessToken.Token}");
            stringBuilder.AppendLine($"{nameof(accessToken.CreatedDateTime)}: {accessToken.CreatedDateTime.ToString("yyyyMMddHHmmss")}");
            stringBuilder.AppendLine($"{nameof(accessToken.ExpiresDateTime)}: {accessToken.ExpiresDateTime.ToString("yyyyMMddHHmmss")}");
            return stringBuilder.ToString();
        }

        private static AccessToken Deserialize(string[] values)
        {
            if (values is null || values.Count() is 0)
                return null;

            AccessToken accessToken = new AccessToken();
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
                    case "Type":
                        accessToken.Type = value;
                        break;
                    case "Token":
                        accessToken.Token = Guid.Parse(value);
                        break;
                    case "CreatedDateTime":
                        accessToken.CreatedDateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "ExpiresDateTime":
                        accessToken.ExpiresDateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return accessToken;
        }

        private static void Log(string message) => System.Diagnostics.Debug.WriteLine($"{nameof(TripPlannerProviderContainer)}: {message}");
    }
}
