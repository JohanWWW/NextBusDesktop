using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.API.ResponseModels
{
    public class LocationListContainer
    {
        [DeserializeAs(Name = "LocationList")]
        public LocationList LocationList { get; set; }
    }
}
