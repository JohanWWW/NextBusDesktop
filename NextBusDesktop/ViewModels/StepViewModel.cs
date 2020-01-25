using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;
using NextBusDesktop.Models.TripPlanner;
using Windows.UI;

namespace NextBusDesktop.ViewModels
{
    public class StepViewModel : ViewModelBase<Step>
    {
        private Translator _translator;

        private static readonly Dictionary<VehicleType, string> _temp = new Dictionary<VehicleType, string>
        {
            [VehicleType.Bus] = "TheBus",
            [VehicleType.Train] = "TheTrain",
            [VehicleType.Tram] = "TheTram",
            [VehicleType.Boat] = "TheBoat",
            [VehicleType.Unknown] = "TheVehicle"
        };

        private bool DepartureIsRescheduled => Origin.RealisticDepartureDateTime != null && Origin.RealisticDepartureDateTime != Origin.DepartureDateTime;
        private bool ArrivalIsRescheduled => Destination.RealisticArrivalDateTime != null && Destination.RealisticArrivalDateTime != Destination.ArrivalDateTime;

        public bool IsLastStep { get; set; }
        public bool HasExtraInfo => DepartureIsRescheduled || ArrivalIsRescheduled;

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

        public string OriginInfo => !string.IsNullOrEmpty(Origin.Track) ? string.Format("{0} {1} {2}", Origin.StopName, _translator["Track"], Origin.Track) : Origin.StopName;
        public string DestinationInfo => !string.IsNullOrEmpty(Destination.Track) ? string.Format("{0} {1} {2}", Destination.StopName, _translator["Track"], Destination.Track) : Destination.StopName;
        public string StepInfo
        {
            get
            {
                string message;
                switch (VehicleType)
                {
                    case VehicleType.Walk:
                        message = !string.IsNullOrEmpty(Destination.Track) ? 
                            string.Format("{0} {1} {2}", _translator["WalkInfoMessage", Destination.StopName], _translator["Track"], Destination.Track) : 
                            _translator["WalkInfoMessage", Destination.StopName];
                        break;
                    case VehicleType.Train:
                        message = _translator["TrainInfoMessage", JourneyNumber, Direction];
                        break;
                    case VehicleType.Bus:
                    case VehicleType.Boat:
                    case VehicleType.Tram:
                        message = _translator["StepInfoMessage", Model.FullName, Model.Direction];
                        break;
                    default:
                        message = _translator["Unknown"];
                        break;
                }

                //System.Diagnostics.Debug.WriteLine($"JourneyNumber: {JourneyNumber}");
                //System.Diagnostics.Debug.WriteLine($"Fullname: {FullName}");
                //System.Diagnostics.Debug.WriteLine($"Shortname: {ShortName}");
                //System.Diagnostics.Debug.WriteLine($"");

                return message;
            }
        }

        public string DepartureArrivalInfoMessage
        {
            get
            {
                if (!DepartureIsRescheduled && !ArrivalIsRescheduled)
                    return string.Empty;

                string message = "Empty";

                // Returns time unit string
                Func<double, string> timeUnit = (totalMinutes) => Math.Abs(totalMinutes) is 1 ? _translator["Minute"] : _translator["Minutes"];
                // Returns string that describes when something is going to occur
                Func<double, string> timeChronology = (totalMinutes) => totalMinutes < 0 ? _translator["Earlier"] : _translator["Later"];

                if (DepartureIsRescheduled && ArrivalIsRescheduled)
                {
                    double adjustedDepartureMinutes = Math.Round(((DateTime)Origin.RealisticDepartureDateTime - Origin.DepartureDateTime).TotalMinutes);
                    double adjustedArrivalMinutes = Math.Round(((DateTime)Destination.RealisticArrivalDateTime - Destination.ArrivalDateTime).TotalMinutes);

                    message = _translator["DepartureArrivalInfoMessage",
                        _translator[_temp[VehicleType]],
                        Origin.StopName,
                        Math.Abs(adjustedDepartureMinutes),
                        timeUnit(adjustedDepartureMinutes),
                        timeChronology(adjustedDepartureMinutes),
                        Destination.StopName,
                        Math.Abs(adjustedArrivalMinutes),
                        timeUnit(adjustedArrivalMinutes),
                        timeChronology(adjustedArrivalMinutes)
                    ];
                }
                else if (DepartureIsRescheduled)
                {
                    double adjustedDepartureMinutes = Math.Round(((DateTime)Origin.RealisticDepartureDateTime - Origin.DepartureDateTime).TotalMinutes);
                    message = _translator["DepartureInfoMessage",
                        _translator[_temp[VehicleType]],
                        Origin.StopName,
                        Math.Abs(adjustedDepartureMinutes),
                        timeUnit(adjustedDepartureMinutes),
                        timeChronology(adjustedDepartureMinutes)
                    ];
                }
                else if (ArrivalIsRescheduled)
                {
                    double adjustedArrivalMinutes = Math.Round(((DateTime)Destination.RealisticArrivalDateTime - Destination.ArrivalDateTime).TotalMinutes);
                    message = _translator["ArrivalInfoMessage",
                        _translator[_temp[VehicleType]],
                        Destination.StopName,
                        Math.Abs(adjustedArrivalMinutes),
                        timeUnit(adjustedArrivalMinutes),
                        timeChronology(adjustedArrivalMinutes)
                    ];
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

        public string LineNumberInfo => VehicleType == VehicleType.Walk ? _translator["Walk"] : ShortName;
        public Color LineForegroundColor { get; private set; }
        public Color LineBackgroundColor { get; private set; }

        public StepViewModel(Step step = null) : base(step)
        {
            _translator = new Translator("TripPlannerResources");
            LineForegroundColor = VehicleType == VehicleType.Walk ? Color.FromArgb(255, 255, 255, 255) : HexToRgb.HexToColor(Model.LineLogoForeground);
            LineBackgroundColor = VehicleType == VehicleType.Walk ? HexToRgb.HexToColor("#b3b3b3") : HexToRgb.HexToColor(Model.LineLogoBackground);
        }
    }
}
