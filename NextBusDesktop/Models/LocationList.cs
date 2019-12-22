using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.Models
{
    public class LocationList
    {
        public DateTime ServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageDetailed { get; set; }
        public IEnumerable<StopLocation> StopLocations { get; set; }

        public LocationList(LocationListResponse locationListResponseModel)
        {
            DateTime.TryParseExact(string.Format("{0} {1}", locationListResponseModel.ServerDate, locationListResponseModel.ServerTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDt);
            ServerDateTime = parsedDt;
            ErrorMessage = locationListResponseModel.Error;
            ErrorMessageDetailed = locationListResponseModel.ErrorText;
            StopLocations = locationListResponseModel.StopLocations?.Select(stopLocResponseModel => new StopLocation(stopLocResponseModel));
        }

        public LocationList()
        {
        }
    }
}
