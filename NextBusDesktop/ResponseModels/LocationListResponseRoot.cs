using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class LocationListResponseRoot
    {
        [DeserializeAs(Name = "LocationList")]
        public LocationListResponse LocationList { get; set; }
    }
}
