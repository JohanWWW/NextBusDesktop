using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.DepartureBoard
{
    public class DepartureBoardResponseContainer
    {
        [DeserializeAs(Name = "DepartureBoard")]
        public DepartureBoardResponse DepartureBoard { get; set; }
    }
}
