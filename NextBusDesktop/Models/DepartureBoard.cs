using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.Models
{
    public class DepartureBoard
    {
        public DateTime BoardDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageDetailed { get; set; }
        public IEnumerable<Departure> Departures { get; set; }

        public DepartureBoard(DepartureBoardResponse departureBoardResponseModel)
        {
            DateTime.TryParseExact(string.Format("{0} {1}", departureBoardResponseModel.ServerDate, departureBoardResponseModel.ServerTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDt);
            BoardDateTime = parsedDt;
            ErrorMessage = departureBoardResponseModel.Error;
            ErrorMessageDetailed = departureBoardResponseModel.ErrorText;
            Departures = departureBoardResponseModel.Departures?.Select(departureRm => new Departure(departureRm));
        }

        public DepartureBoard()
        {
        }
    }
}
