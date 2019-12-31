using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;
using NextBusDesktop.Models.TripPlanner;

namespace NextBusDesktop.ViewModels
{
    public class StepViewModel : ViewModelBase<Step>
    {
        private Translator _translator;

        private bool DepartureIsRescheduled => Origin.RealisticDepartureDateTime != null && Origin.RealisticDepartureDateTime != Origin.DepartureDateTime;
        private bool ArrivalIsRescheduled => Destination.RealisticArrivalDateTime != null && Destination.RealisticArrivalDateTime != Destination.ArrivalDateTime;

        public bool IsLastStep { get; set; }
        public string FullName
        {
            get => Model.FullName;
            set => SetProperty(Model.FullName, value, () => Model.FullName = value);
        }

        public string ShortName
        {
            get => Model.ShortName;
            set => SetProperty(Model.ShortName, value, () => Model.ShortName = value);
        }

        public string JourneyNumber
        {
            get => Model.JourneyNumber;
            set => SetProperty(Model.JourneyNumber, value, () => Model.JourneyNumber = value);
        }

        public string JourneyId
        {
            get => Model.JourneyId;
            set => SetProperty(Model.JourneyId, value, () => Model.JourneyId = value);
        }

        public VehicleType VehicleType
        {
            get => Model.VehicleType;
            set => SetProperty(Model.VehicleType, value, () => Model.VehicleType = value);
        }

        public string Direction
        {
            get => Model.Direction;
            set => SetProperty(Model.Direction, value, () => Model.Direction = value);
        }

        public string LineLogoForeground
        {
            get => Model.LineLogoForeground;
            set => SetProperty(Model.LineLogoForeground, value, () => Model.LineLogoForeground = value);
        }

        public string LineLogoBackground
        {
            get => Model.LineLogoBackground;
            set => SetProperty(Model.LineLogoBackground, value, () => Model.LineLogoBackground = value);
        }

        public string LineLogoBorderStyle
        {
            get => Model.LineLogoBorderStyle;
            set => SetProperty(Model.LineLogoBorderStyle, value, () => Model.LineLogoBorderStyle = value);
        }

        public Origin Origin
        {
            get => Model.Origin;
            set => SetProperty(Model.Origin, value, () => Model.Origin = value);
        }

        public Destination Destination
        {
            get => Model.Destination;
            set => SetProperty(Model.Destination, value, () => Model.Destination = value);
        }

        public string OriginInfo => $"{Origin.StopName} Läge {Origin.Track}";
        public string DestinationInfo => $"{Destination.StopName} Läge {Destination.Track}";
        public string StepInfo
        {
            get
            {
                string message;
                switch (VehicleType)
                {
                    case VehicleType.Walk:
                        message = $"Gå till hållplats {DestinationInfo}";
                        break;
                    case VehicleType.Train:
                        message = $"Ta tåg {JourneyNumber} mot {Direction}";
                        break;
                    case VehicleType.Bus:
                    case VehicleType.Boat:
                    case VehicleType.Tram:
                        message = $"Ta {FullName} mot {Direction}";
                        break;
                    default:
                        message = "Unknown";
                        break;
                }

                return message;
            }
        }

        public string ExtraInfo
        {
            get
            {
                if (!DepartureIsRescheduled && !ArrivalIsRescheduled)
                    return string.Empty;

                string message = "Empty";
                var dict = new Dictionary<VehicleType, string>
                {
                    [VehicleType.Bus] = "Bussen",
                    [VehicleType.Train] = "Tåget",
                    [VehicleType.Tram] = "Spårvagnen",
                    [VehicleType.Boat] = "Färjan"
                };
                

                if (DepartureIsRescheduled && ArrivalIsRescheduled)
                {
                    double adjustedDepartureMinutes = Math.Round(((DateTime)Origin.RealisticDepartureDateTime - Origin.DepartureDateTime).TotalMinutes);
                    double adjustedArrivalMinutes = Math.Round(((DateTime)Destination.RealisticArrivalDateTime - Destination.ArrivalDateTime).TotalMinutes);
                    string template = "{0} avgår från {1} {2} {3} {4} och ankommer till {5} {6} {7} {8} än vanligt";
                    message = string.Format(template,
                        dict[VehicleType],
                        Origin.StopName,
                        Math.Abs(adjustedDepartureMinutes),
                        Math.Abs(adjustedDepartureMinutes) is 1 ? "minut" : "minuter",
                        adjustedDepartureMinutes < 0 ? "tidigare" : "senare",
                        Destination.StopName,
                        Math.Abs(adjustedArrivalMinutes),
                        Math.Abs(adjustedArrivalMinutes) is 1 ? "minut" : "minuter",
                        adjustedArrivalMinutes < 0 ? "tidigare" : "senare"
                    );
                }
                else if (DepartureIsRescheduled)
                {
                    double adjustedDepartureMinutes = Math.Round(((DateTime)Origin.RealisticDepartureDateTime - Origin.DepartureDateTime).TotalMinutes);
                    string template = "{0} avgår från {1} {2} {3} {4} än vanligt";
                    message = string.Format(template,
                        dict[VehicleType],
                        Origin.StopName,
                        Math.Abs(adjustedDepartureMinutes),
                        Math.Abs(adjustedDepartureMinutes) is 1 ? "minut" : "minuter",
                        adjustedDepartureMinutes < 0 ? "tidigare" : "senare"
                    );
                }
                else if (ArrivalIsRescheduled)
                {
                    double adjustedArrivalMinutes = Math.Round(((DateTime)Destination.RealisticArrivalDateTime - Destination.ArrivalDateTime).TotalMinutes);
                    string template = "{0} ankommer till {1} {2} {3} {4} än vanligt";
                    message = string.Format(template,
                        dict[VehicleType],
                        Destination.StopName,
                        Math.Abs(adjustedArrivalMinutes),
                        Math.Abs(adjustedArrivalMinutes) is 1 ? "minut" : "minuter",
                        adjustedArrivalMinutes < 0 ? "tidigare" : "senare"
                    );
                }

                return message;
            }
        }

        public string DepartureTimeInfo
        {
            get
            {
                string message;
                if (DepartureIsRescheduled)
                {
                    double minutes = Math.Round(((DateTime)Origin.RealisticDepartureDateTime - Origin.DepartureDateTime).TotalMinutes);
                    message = string.Format("{0} {1}", Origin.DepartureDateTime.ToString("HH:mm"), (minutes < 0 ? "-" : "+") + Math.Abs(minutes));
                }
                else
                    message = Origin.DepartureDateTime.ToString("HH:mm");

                return message;
            }
        }

        public string ArrivalTimeInfo
        {
            get
            {
                string message;
                if (ArrivalIsRescheduled)
                {
                    double minutes = Math.Round(((DateTime)Destination.RealisticArrivalDateTime - Destination.ArrivalDateTime).TotalMinutes);
                    message = string.Format("{0} {1}", Destination.ArrivalDateTime.ToString("HH:mm"), (minutes < 0 ? "-" : "+") + Math.Abs(minutes));
                }
                else
                    message = Destination.ArrivalDateTime.ToString("HH:mm");

                return message;
            }
        }

        public string LineNumberInfo => VehicleType == VehicleType.Walk ? "GÅ" : ShortName;
        public string LineForegroundColor => VehicleType == VehicleType.Walk ? "White" : LineLogoForeground;
        public string LineBackgroundColor => VehicleType == VehicleType.Walk ? "Gray" : LineLogoBackground;

        public string Visibility => IsLastStep ? "Visible" : "Collapsed";
        public string ExtraInfoVisibility => DepartureIsRescheduled || ArrivalIsRescheduled ? "Visible" : "Collapsed";

        public StepViewModel(Step step = null) : base(step)
        {
        }
    }
}
