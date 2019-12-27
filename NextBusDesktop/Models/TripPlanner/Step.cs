using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels.TripPlanner;

namespace NextBusDesktop.Models.TripPlanner
{
    public class Step
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string JourneyNumber { get; set; }
        public string JourneyId { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Direction { get; set; }
        public string LineLogoForeground { get; set; }
        public string LineLogoBackground { get; set; }
        public string LineLogoBorderStyle { get; set; }
        public string Accessibility { get; set; }
        public Origin Origin { get; set; }
        public Destination Destination { get; set; }

        public Step(LegResponse legResponseModel)
        {
            FullName = legResponseModel.Name;
            ShortName = legResponseModel.SName;
            JourneyNumber = legResponseModel.JourneyNumber;
            JourneyId = legResponseModel.Id;
            switch (legResponseModel.Type)
            {
                case "VAS":
                case "LDT":
                case "REG":
                    VehicleType = VehicleType.Train;
                    break;
                case "BUS":
                    VehicleType = VehicleType.Bus;
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
                case "WALK":
                    VehicleType = VehicleType.Walk;
                    break;
                case "BIKE":
                    VehicleType = VehicleType.Bicycle;
                    break;
                case "CAR":
                    VehicleType = VehicleType.Car;
                    break;
                default:
                    VehicleType = VehicleType.Unknown;
                    break;
            }
            Direction = legResponseModel.Direction;
            LineLogoForeground = legResponseModel.ForegroundColor;
            LineLogoBackground = legResponseModel.BackgroundColor;
            LineLogoBorderStyle = legResponseModel.Stroke;
            Accessibility = legResponseModel.Accessibility;
            Origin = new Origin(legResponseModel.Origin);
            Destination = new Destination(legResponseModel.Destination);
        }

        public Step()
        {
        }
    }
}
