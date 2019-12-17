using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Models
{
    public class StopLocation
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int Index { get; set; }

        public StopLocation(ResponseModels.StopLocation stopLocationResponseModel)
        {
            Name = stopLocationResponseModel.Name;
            Id = stopLocationResponseModel.Id;
            Index = stopLocationResponseModel.Index;
        }

        public StopLocation()
        {
        }
    }
}
