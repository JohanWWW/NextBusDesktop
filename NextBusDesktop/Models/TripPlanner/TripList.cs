using System;
using System.Collections.Generic;
using System.Linq;
using NextBusDesktop.ResponseModels.TripPlanner;

namespace NextBusDesktop.Models.TripPlanner
{
    public class TripList
    {
        public DateTime ServerDateTime { get; set; }
        public IEnumerable<Trip> Trips { get; set; }

        public TripList(TripListResponse tripListResponseModel)
        {
            ServerDateTime = DateTime.ParseExact(string.Format("{0} {1}", tripListResponseModel.ServerDate, tripListResponseModel.ServerTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            Trips = tripListResponseModel.Trips.Select(trip => new Trip(trip));
        }
    }
}
