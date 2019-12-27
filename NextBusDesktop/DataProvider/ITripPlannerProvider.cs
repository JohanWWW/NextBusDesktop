using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.DataProvider
{
    public interface ITripPlannerProvider
    {
        LocationList GetLocationList(string query);
        Models.DepartureBoard.DepartureBoard GetDepartureBoard(string stopId);
        Models.DepartureBoard.DepartureBoard GetDepartureBoard(string stopId, DateTime dateTime);
        Models.TripPlanner.TripList GetTripList(string originStopId, string destinationStopId);
        Models.TripPlanner.TripList GetTripList(string originStopId, string destinationStopId, DateTime dateTime);
    }
}
