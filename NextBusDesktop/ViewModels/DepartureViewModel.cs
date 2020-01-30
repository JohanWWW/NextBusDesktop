using System;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.Utilities;
using Windows.UI;

namespace NextBusDesktop.ViewModels
{
    public class DepartureViewModel : ViewModelBase<Departure>
    {
        private readonly Translator _translator;

        public string FullName => Model.FullName;
        public string ShortName => Model.ShortName;
        public string Track => Model.Track ?? "-";
        public string DirectionInfo => GetDirectionMessage();
        public string DepartureTimeInfo => GetDepartureTimeMessage();
        public bool IsRescheduled => Model.RealisticDeparture != null && Model.ScheduledDeparture != Model.RealisticDeparture;

        private string _timeLeftInfo;
        public string TimeLeftInfo
        {
            get => _timeLeftInfo;
            set => SetProperty(ref _timeLeftInfo, value);
        }

        private Color _statusIndicatorColor;
        public Color StatusIndicatorColor
        {
            get => _statusIndicatorColor;
            set => SetProperty(ref _statusIndicatorColor, value);
        }

        public Color LineLogoBackground { get; private set; }

        public Color LineLogoForeground { get; private set; }

        public DepartureViewModel(Departure departure) : base(departure)
        {
            _translator = new Translator("DeparturesWindow");
            LineLogoBackground = HexToRgb.HexToColor(Model.LineLogoBackgroundColor);
            LineLogoForeground = HexToRgb.HexToColor(Model.LineLogoTextColor);
            _timeLeftInfo = GetTimeLeftMessage();
        }

        public void TriggerTimeUpdate() => TimeLeftInfo = GetTimeLeftMessage();

        private string GetDirectionMessage() => _translator["DirectionOf", Model.Direction];

        private string GetDepartureTimeMessage()
        {
            DateTime scheduledDeparture = Model.ScheduledDeparture;
            DateTime? realisticDeparture = Model.RealisticDeparture;
            string message;
            if (IsRescheduled)
            {
                double minutes = Math.Round(((DateTime)realisticDeparture - scheduledDeparture).TotalMinutes);
                message = string.Format("{0} {1}", scheduledDeparture.ToString("HH:mm"), (minutes < 0 ? "-" : "+") + Math.Abs(minutes));
            }
            else
                message = scheduledDeparture.ToString("HH:mm");

            return message;
        }

        private string GetTimeLeftMessage()
        {
            TimeSpan timeLeft;
            if (IsRescheduled)
                timeLeft = (DateTime)Model.RealisticDeparture - DateTime.Now;
            else
                timeLeft = Model.ScheduledDeparture - DateTime.Now;

            // Localize time units
            if (timeLeft.TotalMinutes < 1)
                return _translator["Now"];
            else if (timeLeft.TotalMinutes < 60)
                return string.Format("{0} {1}", timeLeft.ToString("mm"), _translator["MinutesAbbr"]).TrimStart('0');
            else if (timeLeft.TotalHours < 24)
                return string.Format("{0}{1} {2}{3}", timeLeft.ToString("hh"), _translator["HoursAbbr"], timeLeft.ToString("mm"), _translator["MinutesAbbr"]).TrimStart('0');
            else
                return string.Format("{0} {1}", timeLeft.ToString("dd"), _translator["DaysAbbr"]).TrimStart('0');
        }
    }
}
