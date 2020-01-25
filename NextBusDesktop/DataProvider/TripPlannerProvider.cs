using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.ResponseModels.TripPlanner;
using NextBusDesktop.ResponseModels.DepartureBoard;
using NextBusDesktop.Models;
using RestSharp;
using NextBusDesktop.Models.TripPlanner;
using NextBusDesktop.Models.DepartureBoard;
using System.ComponentModel;

namespace NextBusDesktop.DataProvider
{
    /// <summary>
    /// Provides Västtrafik's Trip planner api.
    /// </summary>
    public class TripPlannerProvider : ITripPlannerProvider
    {
        private const string _dateFormat = "yyyy-MM-dd";
        private const string _timeFormat = "HH:mm";

        private readonly IRestClient _client;
        private AccessToken _accessToken;

        public bool IsAccessTokenExpired => _accessToken.ExpiresDateTime < DateTime.Now ? true : false;

        public TripPlannerProvider(AccessToken accessToken) : this() => _accessToken = accessToken;

        public TripPlannerProvider() => _client = new RestClient("https://api.vasttrafik.se/bin/rest.exe/v2/");

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId) => await GetDepartureBoardAsync(stopId, DateTime.Now);

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime)
        {
            Log($"Request -> {nameof(GetDepartureBoardAsync)}: Requesting departure board for stop {stopId}.");
            var request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddParameter("id", stopId);
            request.AddParameter("date", dateTime.ToString(_dateFormat));
            request.AddParameter("time", dateTime.ToString(_timeFormat));
            request.AddParameter("format", "json");

            var response = await _client.ExecuteTaskAsync<DepartureBoardResponseRoot>(request, Method.GET);
            Log($"Response -> {nameof(GetDepartureBoardAsync)} {response.StatusCode}");

            return new DepartureBoard(response.Data.DepartureBoard);
        }

        public async Task<LocationList> GetLocationListAsync(string query)
        {
            Log($"Request -> {nameof(GetLocationListAsync)}: Requesting location list for query '{query}'.");

            var request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddQueryParameter("input", query);
            request.AddQueryParameter("format", "json");

            var response = await _client.ExecuteTaskAsync<LocationListResponseRoot>(request, Method.GET);
            Log($"Response -> {nameof(GetLocationListAsync)} {response.StatusCode}: location list count {response.Data?.LocationList?.StopLocations?.Count()}");

            return new LocationList(response.Data.LocationList);
        }

        public async Task<TripList> GetTripListAsync(string originStopId, string destinationStopId) => await GetTripListAsync(originStopId, destinationStopId, DateTime.Now);

        public async Task<TripList> GetTripListAsync(string originStopId, string destinationStopId, DateTime dateTime, bool isSearchForArrival = false)
        {
            Log($"Request -> {nameof(GetTripListAsync)}: Requesting trips for {nameof(originStopId)} {originStopId} and {nameof(destinationStopId)} {destinationStopId}.");

            var request = new RestRequest("/trip");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddQueryParameter("originId", originStopId);
            request.AddQueryParameter("destId", destinationStopId);
            request.AddQueryParameter("date", dateTime.ToString("yyyy-MM-dd"));
            request.AddQueryParameter("time", dateTime.ToString("HH:mm"));
            request.AddQueryParameter("searchForArrival", (isSearchForArrival ? 1 : 0).ToString()); // Convert bool to bit
            request.AddQueryParameter("format", "json");

            var response = await _client.ExecuteTaskAsync<TripListRoot>(request, Method.GET);
            Log($"Response -> {nameof(GetTripListAsync)} {response.StatusCode}");

            return new TripList(response.Data.TripList);
        }

        public void SetToken(AccessToken token) => _accessToken = token;

        private void Log(string message) => System.Diagnostics.Debug.WriteLine($"{nameof(TripPlannerProvider)}: {message}");
    }
}
