using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class LocationListResponseRoot
    {
        [DeserializeAs(Name = "LocationList")]
        public LocationListResponse LocationList { get; set; }
    }
}
