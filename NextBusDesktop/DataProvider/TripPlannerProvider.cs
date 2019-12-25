using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.Models;
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

        public DepartureBoard GetDepartureBoard(string stopId) => GetDepartureBoard(stopId, DateTime.Now);

        public DepartureBoard GetDepartureBoard(string stopId, DateTime dateTime)
        {
            IRestRequest request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddParameter("id", stopId);
            request.AddParameter("date", dateTime.ToString(_dateFormat));
            request.AddParameter("time", dateTime.ToString(_timeFormat));
            request.AddParameter("format", "json");

            IRestResponse<DepartureBoardResponseContainer> response = _client.Execute<DepartureBoardResponseContainer>(request, Method.GET);

            return new DepartureBoard(response.Data.DepartureBoard);
        }

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId) => await GetDepartureBoardAsync(stopId, DateTime.Now);

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime)
        {
            Log($"Request -> {nameof(GetDepartureBoardAsync)}: Requesting departure board for stop {stopId}.");
            IRestRequest request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddParameter("id", stopId);
            request.AddParameter("date", dateTime.ToString(_dateFormat));
            request.AddParameter("time", dateTime.ToString(_timeFormat));
            request.AddParameter("format", "json");

            IRestResponse<DepartureBoardResponseContainer> response = await _client.ExecuteTaskAsync<DepartureBoardResponseContainer>(request, Method.GET);
            Log($"Response -> {nameof(GetDepartureBoardAsync)} {response.StatusCode}");

            return new DepartureBoard(response.Data.DepartureBoard);
        }

        public LocationList GetLocationList(string query)
        {
            IRestRequest request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddQueryParameter("input", query);
            request.AddQueryParameter("format", "json");

            IRestResponse<LocationListResponseContainer> response = _client.Execute<LocationListResponseContainer>(request, Method.GET);

            return new LocationList(response.Data.LocationList);
        }

        public async Task<LocationList> GetLocationListAsync(string query)
        {
            Log($"Request -> {nameof(GetLocationListAsync)}: Requesting location list for query '{query}'.");

            IRestRequest request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddQueryParameter("input", query);
            request.AddQueryParameter("format", "json");

            IRestResponse<LocationListResponseContainer> response = await _client.ExecuteTaskAsync<LocationListResponseContainer>(request, Method.GET);
            Log($"Response -> {nameof(GetLocationListAsync)} {response.StatusCode}: location list count {response.Data?.LocationList?.StopLocations?.Count()}");

            return new LocationList(response.Data.LocationList);
        }

        private void Log(string message) => System.Diagnostics.Debug.WriteLine($"{nameof(TripPlannerProvider)}: {message}");
    }
}
