﻿using System;
using System.Threading.Tasks;
using NextBusDesktop.Models;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.Models.TripPlanner;

namespace NextBusDesktop.DataProvider
{
    public interface ITripPlannerProvider
    {
        bool IsAccessTokenExpired { get; }

        Task<LocationList> GetLocationListAsync(string query);
        Task<DepartureBoard> GetDepartureBoardAsync(string stopId);
        Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime);
        Task<TripList> GetTripListAsync(string originStopId, string destinationStopId);
        Task<TripList> GetTripListAsync(string originStopId, string destinationStopId, DateTime dateTime, bool isSearchForArrival = false);

        void SetToken(AccessToken newToken);
    }
}
