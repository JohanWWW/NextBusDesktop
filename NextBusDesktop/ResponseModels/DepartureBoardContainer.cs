using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class DepartureBoardContainer
    {
        [DeserializeAs(Name = "DepartureBoard")]
        public DepartureBoard DepartureBoard { get; set; }
    }
}
