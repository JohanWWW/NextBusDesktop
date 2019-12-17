using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using RestSharp;

namespace NextBusDesktop.DataProvider
{
    /// <summary>
    /// Provides Västtrafik's Trip planner api.
    /// </summary>
    public class TripPlannerProvider : ITripPlannerProvider, ITripPlannerProviderAsync
    {
        private const string _dateFormat = "yyyy-MM-dd";
        private const string _timeFormat = "HH:mm";

        private readonly IRestClient _client;
        private AccessToken _accessToken;

        /// <param name="accessToken">Access token that is provided by <see cref="AccessTokenProvider"/></param>
        public TripPlannerProvider(AccessToken accessToken)
        {
            _accessToken = accessToken;
            _client = new RestClient("https://api.vasttrafik.se/bin/rest.exe/v2/");
        }

        public Models.DepartureBoard GetDepartureBoard(string stopId) => GetDepartureBoard(stopId, DateTime.Now);

        public Models.DepartureBoard GetDepartureBoard(string stopId, DateTime dateTime)
        {
            IRestRequest request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.TokenType} {_accessToken.Token}");
            request.AddParameter("id", stopId);
            request.AddParameter("date", dateTime.ToString(_dateFormat));
            request.AddParameter("time", dateTime.ToString(_timeFormat));
            request.AddParameter("format", "json");

            IRestResponse<DepartureBoardContainer> response = _client.Execute<DepartureBoardContainer>(request, Method.GET);

            return new Models.DepartureBoard(response.Data.DepartureBoard);
        }

        public async Task<Models.DepartureBoard> GetDepartureBoardAsync(string stopId) => await GetDepartureBoardAsync(stopId, DateTime.Now);

        public async Task<Models.DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime)
        {
            IRestRequest request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.TokenType} {_accessToken.Token}");
            request.AddParameter("id", stopId);
            request.AddParameter("date", dateTime.ToString(_dateFormat));
            request.AddParameter("time", dateTime.ToString(_timeFormat));
            request.AddParameter("format", "json");

            //await Task.Delay(5000);
            IRestResponse<DepartureBoardContainer> response = await _client.ExecuteTaskAsync<DepartureBoardContainer>(request, Method.GET);

            return new Models.DepartureBoard(response.Data.DepartureBoard);
        }

        public Models.LocationList GetLocationList(string query)
        {
            IRestRequest request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.TokenType} {_accessToken.Token}");
            request.AddQueryParameter("input", query);
            request.AddQueryParameter("format", "json");

            IRestResponse<LocationListContainer> response = _client.Execute<LocationListContainer>(request, Method.GET);

            return new Models.LocationList(response.Data.LocationList);

        }

        public async Task<Models.LocationList> GetLocationListAsync(string query)
        {
            IRestRequest request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.TokenType} {_accessToken.Token}");
            request.AddQueryParameter("input", query);
            request.AddQueryParameter("format", "json");

            //await Task.Delay(5000);
            IRestResponse<LocationListContainer> response = await _client.ExecuteTaskAsync<LocationListContainer>(request, Method.GET);

            return new Models.LocationList(response.Data.LocationList);
        }
    }
}
