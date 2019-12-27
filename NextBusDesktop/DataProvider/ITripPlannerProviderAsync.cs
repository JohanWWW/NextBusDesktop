using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.DataProvider
{
    public interface ITripPlannerProviderAsync
    {
        Task<LocationList> GetLocationListAsync(string query);
        Task<Models.DepartureBoard.DepartureBoard> GetDepartureBoardAsync(string stopId);
        Task<Models.DepartureBoard.DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime);
        Task<Models.TripPlanner.TripList> GetTripListAsync(string originStopId, string destinationStopId);
        Task<Models.TripPlanner.TripList> GetTripListAsync(string originStopId, string destinationStopId, DateTime dateTime);
    }
}
