using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.DepartureBoard
{
    public class DepartureBoardResponse
    {
        [DeserializeAs(Name = "serverTime")] public string ServerTime { get; set; }
        [DeserializeAs(Name = "serverDate")] public string ServerDate { get; set; }
        [DeserializeAs(Name = "error")] public string Error { get; set; }
        [DeserializeAs(Name = "errorText")] public string ErrorText { get; set; }
        [DeserializeAs(Name = "Departure")] public IEnumerable<DepartureResponse> Departures { get; set; }
    }
}
