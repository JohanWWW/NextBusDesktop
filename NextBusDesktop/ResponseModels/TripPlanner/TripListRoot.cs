using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.TripPlanner
{
    public class TripListRoot
    {
        [DeserializeAs(Name = "TripList")]
        public TripListResponse TripList { get; set; }
    }
}
