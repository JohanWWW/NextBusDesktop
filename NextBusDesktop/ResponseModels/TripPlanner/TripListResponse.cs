using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.TripPlanner
{
    public class TripListResponse
    {
        [DeserializeAs(Name = "errorText")] public string ErrorText { get; set; }
        [DeserializeAs(Name = "error")] public string Error { get; set; }
        [DeserializeAs(Name = "serverdate")] public string ServerDate { get; set; }
        [DeserializeAs(Name = "servertime")] public string ServerTime { get; set; }
        [DeserializeAs(Name = "Trip")] public IEnumerable<TripResponse> Trips { get; set; }
    }
}
