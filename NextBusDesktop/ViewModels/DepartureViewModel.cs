using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using NextBusDesktop.Models.DepartureBoard;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

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

        //private SolidColorBrush _statusIndicatorColor;
        public string StatusIndicatorColor => IsRescheduled ? "Yellow" : null;
        //public SolidColorBrush StatusIndicatorColor
        //{
        //    get => _statusIndicatorColor;
        //    set => SetProperty(ref _statusIndicatorColor, value);
        //}

        public string LineLogoBackground => Model.LineLogoBackgroundColor;
        public string LineLogoForeground => Model.LineLogoTextColor;

        public DepartureViewModel(Departure departure) : base(departure)
        {
            _translator = new Translator("DeparturesWindow");
            _timeLeftInfo = GetTimeLeftMessage();
            //_statusIndicatorColor = IsRescheduled ? new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)) : null;
        }

        public void TriggerTimeUpdate() => TimeLeftInfo = GetTimeLeftMessage();

        protected override void Deconstruct()
        {
        }

        private string GetDirectionMessage() => _translator["DirectionOf", Model.Direction];

        private string GetDepartureTimeMessage()
        {
            DateTime scheduledDeparture = Model.ScheduledDeparture;
            DateTime? realisticDeparture = Model.RealisticDeparture;
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
                timeLeft = (DateTime)Model.RealisticDeparture - DateTime.Now;
            else
                timeLeft = Model.ScheduledDeparture - DateTime.Now;

            // TODO: Localize time unit
            string format;
            if (timeLeft.TotalMinutes < 1)
                format = @"\n\o\w"; // TODO: Come up with a better solution than this
            else if (timeLeft.TotalMinutes < 60)
                format = @"mm\ \m\i\n";
            else if (timeLeft.TotalHours < 24)
                format = @"hh\h\ mm\m\i\n";
            else
                format = @"dd\ \d";

            // Debug
            //format = "hh\\:mm\\:ss";

            return timeLeft.ToString(format).TrimStart('0');
            //return timeLeft.ToString(format);
        }
    }
}
