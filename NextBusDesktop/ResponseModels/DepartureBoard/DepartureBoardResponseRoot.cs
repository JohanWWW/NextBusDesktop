using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.DepartureBoard
{
    public class DepartureBoardResponseRoot
    {
        [DeserializeAs(Name = "DepartureBoard")]
        public DepartureBoardResponse DepartureBoard { get; set; }
    }
}
