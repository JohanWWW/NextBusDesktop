﻿using System;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.ResponseModels.TripPlanner;
using NextBusDesktop.ResponseModels.DepartureBoard;
using NextBusDesktop.Models;
using RestSharp;
using NextBusDesktop.Models.TripPlanner;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.Utilities;

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
        private ILog _logger;
        private AccessToken _accessToken;

        public ILog Logger
        {
            set => _logger = value;
        }

        public bool IsAccessTokenExpired => _accessToken.ExpiresDateTime < DateTime.Now ? true : false;

        public TripPlannerProvider(AccessToken accessToken) : this() => _accessToken = accessToken;

        public TripPlannerProvider() => _client = new RestClient("https://api.vasttrafik.se/bin/rest.exe/v2/");

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId) => await GetDepartureBoardAsync(stopId, DateTime.Now);

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime)
        {
            Log($"{nameof(GetDepartureBoardAsync)}: Requesting departure board for stop {stopId}", "Request");
            var request = new RestRequest("/departureBoard");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddParameter("id", stopId);
            request.AddParameter("date", dateTime.ToString(_dateFormat));
            request.AddParameter("time", dateTime.ToString(_timeFormat));
            request.AddParameter("format", "json");

            var response = await _client.ExecuteTaskAsync<DepartureBoardResponseRoot>(request, Method.GET);
            Log($"{nameof(GetDepartureBoardAsync)}: {response.StatusCode}", "Response");

            return new DepartureBoard(response.Data.DepartureBoard);
        }

        public async Task<LocationList> GetLocationListAsync(string query)
        {
            Log($"{nameof(GetLocationListAsync)}: Requesting location list for query '{query}'", "Request");

            var request = new RestRequest("/location.name");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddQueryParameter("input", query);
            request.AddQueryParameter("format", "json");

            var response = await _client.ExecuteTaskAsync<LocationListResponseRoot>(request, Method.GET);
            Log($"{nameof(GetLocationListAsync)}: {response.StatusCode}", "Response");

            return new LocationList(response.Data.LocationList);
        }

        public async Task<TripList> GetTripListAsync(string originStopId, string destinationStopId) => await GetTripListAsync(originStopId, destinationStopId, DateTime.Now);

        public async Task<TripList> GetTripListAsync(string originStopId, string destinationStopId, DateTime dateTime, bool isSearchForArrival = false)
        {
            Log($"{nameof(GetTripListAsync)}: Requesting trips for {nameof(originStopId)} {originStopId} and {nameof(destinationStopId)} {destinationStopId}", "Request");

            var request = new RestRequest("/trip");
            request.AddHeader("Authorization", $"{_accessToken.Type} {_accessToken.Token}");
            request.AddQueryParameter("originId", originStopId);
            request.AddQueryParameter("destId", destinationStopId);
            request.AddQueryParameter("date", dateTime.ToString("yyyy-MM-dd"));
            request.AddQueryParameter("time", dateTime.ToString("HH:mm"));
            request.AddQueryParameter("searchForArrival", (isSearchForArrival ? 1 : 0).ToString()); // Convert bool to bit
            request.AddQueryParameter("format", "json");

            var response = await _client.ExecuteTaskAsync<TripListRoot>(request, Method.GET);
            Log($"{nameof(GetTripListAsync)}: {response.StatusCode}", "Response");

            return new TripList(response.Data.TripList);
        }

        public void SetToken(AccessToken token) => _accessToken = token;

        private void Log(string message) => _logger?.Log(message);

        private void Log(string message, string category) => _logger?.Log(message, category);
    }
}
