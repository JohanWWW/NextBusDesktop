using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Models
{
    public class DepartureBoard
    {
        public DateTime BoardDateTime { get; set; }
        public IEnumerable<Departure> Departures { get; set; }

        public DepartureBoard(ResponseModels.DepartureBoard departureBoardResponseModel)
        {
            BoardDateTime = DateTime.ParseExact(string.Format("{0} {1}", departureBoardResponseModel.ServerDate, departureBoardResponseModel.ServerTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            Departures = departureBoardResponseModel.Departures?.Select(departureRm => new Departure(departureRm));
        }

        public DepartureBoard()
        {
        }
    }
}
