using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.ViewModels
{
    public class DepartureViewModel : NotificationBase<Departure>
    {
        private Translator _translator;

        public string FullName => This.FullName;
        public string ShortName => This.ShortName;
        public string Track => This.Track ?? "-";
        public string DirectionInfo => GetDirectionMessage();
        public string DepartureTimeInfo => GetDepartureTimeMessage();
        public bool IsRescheduled => This.RealisticDeparture != null && This.ScheduledDeparture != This.RealisticDeparture;
        public string TimeLeftInfo => GetTimeLeftMessage();
        public string StatusIndicatorColor => IsRescheduled ? "Yellow" : null;
        public string LineLogoBackground => This.LineLogoBackgroundColor;
        public string LineLogoForeground => This.LineLogoTextColor;

        public DepartureViewModel(Departure departure) : base(departure)
        {
            _translator = new Translator("DeparturesWindow");
        }

        private string GetDirectionMessage() => _translator["DirectionOf", This.Direction];

        private string GetDepartureTimeMessage()
        {
            DateTime scheduledDeparture = This.ScheduledDeparture;
            DateTime? realisticDeparture = This.RealisticDeparture;
            string message;
            if (IsRescheduled)
                message = string.Format("{0} {1}", realisticDeparture?.ToString("HH:mm"), _translator["NewTime"]);
            else
                message = scheduledDeparture.ToString("HH:mm");

            return message;
        }

        private string GetTimeLeftMessage()
        {
            TimeSpan timeLeft;
            if (IsRescheduled)
                timeLeft = (DateTime)This.RealisticDeparture - DateTime.Now;
            else
                timeLeft = This.ScheduledDeparture - DateTime.Now;

            string format;
            if (timeLeft.TotalMinutes < 1)
                format = @"\n\o\w";
            else if (timeLeft.TotalMinutes < 60)
                format = @"mm\ \m\i\n";
            else if (timeLeft.TotalHours < 24)
                format = @"hh\h\ mm\m\i\n";
            else
                format = @"dd\ \d";

            return timeLeft.ToString(format).TrimStart('0');
        }
    }
}
