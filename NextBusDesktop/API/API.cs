using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using NextBusDesktop.API.ResponseModels;
using Windows.Storage;

namespace NextBusDesktop.API
{
    /// <summary>
    /// Västtrafik - Reseplaneraren
    /// </summary>
    public class API
    {
        private const string _dateFormat = "yyyy-MM-dd";
        private const string _timeFormat = "HH:mm";

        private IRestClient _client;
        private IRestClient _tokenGenerationClient;
        private AccessToken _accessToken;
        
        public API()
        {
            var baseUrl = new Uri("https://api.vasttrafik.se/bin/rest.exe/v2/");
            var tokenGenerationBaseUrl = new Uri("https://api.vasttrafik.se/token");

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            _client = new RestClient(baseUrl);
            _tokenGenerationClient = new RestClient(tokenGenerationBaseUrl);
            _tokenGenerationClient.Authenticator = new HttpBasicAuthenticator(
                localSettings.Values["Api_ClientId"].ToString(), 
                localSettings.Values["Api_Secret"].ToString());

        }

        public APIResult<LocationList> GetLocationList(string searchString)
        {
            IRestRequest request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.TokenType} {_accessToken.Token}");
            request.AddQueryParameter("input", searchString);
            request.AddQueryParameter("format", "json");

            IRestResponse<LocationListContainer> response = _client.Execute<LocationListContainer>(request, Method.GET);

            return new APIResult<LocationList>(response.Data.LocationList, response.StatusCode);
        }

        public APIResult<DepartureBoard> GetDepartureBoard(string stopId) => GetDepartureBoard(stopId, DateTime.Now);

        public APIResult<DepartureBoard> GetDepartureBoard(string stopId, DateTime dateTime)
        {
            IRestRequest request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.TokenType} {_accessToken.Token}");
            request.AddQueryParameter("id", stopId);
            request.AddQueryParameter("date", dateTime.ToString(_dateFormat));
            request.AddQueryParameter("time", dateTime.ToString(_timeFormat));
            request.AddQueryParameter("format", "json");

            IRestResponse<DepartureBoardContainer> response = _client.Execute<DepartureBoardContainer>(request, Method.GET);

            return new APIResult<DepartureBoard>(response.Data.DepartureBoard, response.StatusCode);
        }

        public bool GenerateAccessToken()
        {
            IRestRequest tokenRequest = new RestRequest();
            tokenRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            tokenRequest.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            IRestResponse<AccessToken> response = _tokenGenerationClient.Execute<AccessToken>(tokenRequest, Method.POST);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;

            _accessToken = response.Data;
            return true;
        }

    }
}
