using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Models
{
    public class LocationList
    {
        public DateTime ServerDateTime { get; set; }
        public IEnumerable<StopLocation> StopLocations { get; set; }

        public LocationList(ResponseModels.LocationList locationListResponseModel)
        {
            ServerDateTime = DateTime.ParseExact(string.Format("{0} {1}", locationListResponseModel.ServerDate, locationListResponseModel.ServerTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            StopLocations = locationListResponseModel.StopLocations?.Select(stopLocResponseModel => new StopLocation(stopLocResponseModel));
        }
    }
}
