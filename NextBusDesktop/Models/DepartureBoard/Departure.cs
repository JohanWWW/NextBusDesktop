using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels.DepartureBoard;

namespace NextBusDesktop.Models.DepartureBoard
{
    public class Departure
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Track { get; set; }
        public VehicleType VehicleType { get; set; }
        public string JourneyNumber { get; set; }
        public string JourneyId { get; set; }
        public string StopName { get; set; }
        public string StopId { get; set; }
        public DateTime ScheduledDeparture { get; set; }
        public DateTime? RealisticDeparture { get; set; }
        public string Direction { get; set; }
        public string Accessibility { get; set; }

        public string LineLogoTextColor { get; set; }
        public string LineLogoBackgroundColor { get; set; }
        public string LineLogoBorderStyle { get; set; }

        public Departure(DepartureResponse departureResponseModel)
        {
            FullName = departureResponseModel.Name;
            ShortName = departureResponseModel.SName;
            Track = departureResponseModel.Track;
            switch (departureResponseModel.Type)
            {
                case "BUS":
                    VehicleType = VehicleType.Bus;
                    break;
                case "VAS": // Västtåg
                case "LDT": // Long Distance Train
                case "REG": // Regional Train
                    VehicleType = VehicleType.Train;
                    break;
                case "BOAT":
                    VehicleType = VehicleType.Boat;
                    break;
                case "TRAM":
                    VehicleType = VehicleType.Tram;
                    break;
                case "TAXI":
                    VehicleType = VehicleType.Taxi;
                    break;
                default:
                    VehicleType = VehicleType.Unknown;
                    break;
            }
            JourneyNumber = departureResponseModel.JourneyNumber;
            JourneyId = departureResponseModel.JourneyId;
            StopName = departureResponseModel.Stop;
            StopId = departureResponseModel.StopId;
            ScheduledDeparture = DateTime.ParseExact(string.Format("{0} {1}", departureResponseModel.ScheduledDate, departureResponseModel.ScheduledTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            string realDate = departureResponseModel.RealisticDate;
            string realTime = departureResponseModel.RealisticTime;
            if (!string.IsNullOrEmpty(realDate) || !string.IsNullOrEmpty(realTime))
                RealisticDeparture = DateTime.ParseExact(string.Format("{0} {1}", realDate, realTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            else RealisticDeparture = null;
            Direction = departureResponseModel.Direction;
            Accessibility = departureResponseModel.Accessibility;
            LineLogoTextColor = departureResponseModel.ForegroundColor;
            LineLogoBackgroundColor = departureResponseModel.BackgroundColor;
            LineLogoBorderStyle = departureResponseModel.Stroke;
        }

        public Departure()
        {
        }

        public override string ToString() => FullName;
    }
}
