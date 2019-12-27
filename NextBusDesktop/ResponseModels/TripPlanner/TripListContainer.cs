using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.TripPlanner
{
    public class TripListContainer
    {
        [DeserializeAs(Name = "TripList")]
        public TripListResponse TripList { get; set; }
    }
}
