using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.Models;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.Models.TripPlanner;
using System.Xml;
using Windows.Storage;
using NextBusDesktop.Utilities;

namespace NextBusDesktop.DataProvider
{
    /// <summary>
    /// Passthrough and maintains the <see cref="ITripPlannerProvider"/> instance
    /// </summary>
    public static class TripPlannerProviderProxy
    {
        private static ITripPlannerProvider _tripPlannerProvider;
        private static IAccessTokenProvider _accessTokenProvider;
        private static IStorageFolder _accessTokenFolder;
        private static IStorageFile _accessTokenFile;

        private static ILog _logger;

        private static bool IsAccessTokenExpired => _tripPlannerProvider.IsAccessTokenExpired;

        public static async Task Initialize()
        {
            _logger = new OutputLogger(typeof(TripPlannerProviderProxy));

            Log("Initializing TripPlannerProvider.");
            _accessTokenProvider = new AccessTokenProvider() { Logger = new OutputLogger(nameof(AccessTokenProvider)) };
            _accessTokenFolder = ApplicationData.Current.LocalFolder;
            
            bool fileIsEmpty;

            if (!File.Exists(_accessTokenFolder.Path + "\\CurrentAccessToken.txt"))
            {
                fileIsEmpty = true;
                _accessTokenFile = await _accessTokenFolder.CreateFileAsync("CurrentAccessToken.txt", CreationCollisionOption.FailIfExists);
                Log("Created new access token file.", "Out");
            }
            else
            {
                fileIsEmpty = false;
                _accessTokenFile = await _accessTokenFolder.GetFileAsync("CurrentAccessToken.txt");
                Log("Load access token from file.", "In");
            }

            AccessToken token;

            Func<Task<AccessToken>> createNewToken = async () =>
            {
                AccessToken newAccessToken = await _accessTokenProvider.GetAccessTokenAsync();
                string serialized = Serialize(newAccessToken);
                await FileIO.WriteTextAsync(_accessTokenFile, serialized);
                return newAccessToken;
            };

            if (fileIsEmpty) // File is empty because the file was just created.
            {
                Log("Could not find access token on file.");
                token = await createNewToken();
            }
            else
            {
                var data = await FileIO.ReadLinesAsync(_accessTokenFile);
                string[] serializedRetrieved = data.ToArray();
                AccessToken retrievedAccessToken = Deserialize(serializedRetrieved);
                if (retrievedAccessToken.ExpiresDateTime < DateTime.Now)
                {
                    Log("Access token has expired. Requesting new access token.");
                    token = await createNewToken();
                }
                else
                    token = retrievedAccessToken;

            }

            _tripPlannerProvider = new TripPlannerProvider(token) { Logger = new OutputLogger(nameof(TripPlannerProvider)) };
            //_tripPlannerProvider = new TripPlannerProviderMock();

            Log($"Access token is valid to {token.ExpiresDateTime}.", "Info");
            Log("Initializing complete.");
        }

        public static async Task<LocationList> GetLocationList(string query)
        {
            if (IsAccessTokenExpired)
                await RenewToken();

            return await _tripPlannerProvider.GetLocationListAsync(query);
        }

        public static async Task<DepartureBoard> GetDepartureBoard(string stopId) => await GetDepartureBoard(stopId, DateTime.Now);

        public static async Task<DepartureBoard> GetDepartureBoard(string stopId, DateTime dateTime)
        {
            if (IsAccessTokenExpired)
                await RenewToken();

            return await _tripPlannerProvider.GetDepartureBoardAsync(stopId, dateTime);
        }

        public static async Task<TripList> GetTripList(string originStopId, string destinationStopId) => await GetTripList(originStopId, destinationStopId, DateTime.Now);

        public static async Task<TripList> GetTripList(string originStopId, string destinationStopId, DateTime dateTime, bool isSearchForArrival = false)
        {
            if (IsAccessTokenExpired)
                await RenewToken();

            return await _tripPlannerProvider.GetTripListAsync(originStopId, destinationStopId, dateTime, isSearchForArrival);
        }

        /// <summary>
        /// Requests a new access token from server and saves it to file.
        /// </summary>
        private static async Task RenewToken()
        {
            Log("Access token has expired. Requesting new access token.");
            AccessToken token = await _accessTokenProvider.GetAccessTokenAsync();
            _tripPlannerProvider.SetToken(token);

            Log("Save access token.", "Out");
            string serialized = Serialize(token);
            await FileIO.WriteTextAsync(_accessTokenFile, serialized);
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

        private static void Log(string message) => _logger?.Log(message);

        private static void Log(string message, string category) => _logger?.Log(message, category);
    }
}
