using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.TripPlanner
{
    public class TripResponse
    {
        [DeserializeAs(Name = "Leg")]
        public IEnumerable<LegResponse> Legs { get; set; }
    }
}
