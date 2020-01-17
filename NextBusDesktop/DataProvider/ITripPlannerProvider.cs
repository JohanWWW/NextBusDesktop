using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.Models.TripPlanner;

namespace NextBusDesktop.DataProvider
{
    public interface ITripPlannerProvider
    {
        Task<LocationList> GetLocationListAsync(string query);
        Task<DepartureBoard> GetDepartureBoardAsync(string stopId);
        Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime);
        Task<TripList> GetTripListAsync(string originStopId, string destinationStopId);
        Task<TripList> GetTripListAsync(string originStopId, string destinationStopId, DateTime dateTime, bool isSearchForArrival = false);
    }
}
