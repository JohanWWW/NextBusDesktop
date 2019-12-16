using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NextBusDesktop.ResponseModels;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NextBusDesktop
{
    public sealed partial class DepartureBoxControl : UserControl
    {
        private Departure _departure;
        private Translator _translator;

        public Departure Departure
        {
            get => _departure;
            set
            {
                _departure = value;

                Color white = ColorHelper("White");
                DateTime scheduledDepartureDateTime = _departure.GetScheduledDateTime();
                DateTime? realisticDepartureDateTime = _departure.GetRealisticDateTime();
                bool reschedule;
                if (realisticDepartureDateTime != null) reschedule = scheduledDepartureDateTime != realisticDepartureDateTime;
                else reschedule = false;
                TimeSpan departsIn = reschedule ? (DateTime)realisticDepartureDateTime - DateTime.Now : scheduledDepartureDateTime - DateTime.Now;

                string timeSpanFormat;
                if (departsIn.TotalMinutes < 1) timeSpanFormat = @"\n\o\w";
                else if (departsIn.TotalMinutes < 60) timeSpanFormat = @"mm\ \m\i\n";
                else if (departsIn.TotalHours < 24) timeSpanFormat = @"h\h\ mm\m\i\n";
                else timeSpanFormat = @"dd\d";

                // fgColor and bgColor are flipped because helper inverts them for some reason.
                LineLogo.Background = new SolidColorBrush(ColorHelper(value.ForegroundColor));
                LineNumber.Foreground = new SolidColorBrush(ColorHelper(value.BackgroundColor));
                LineNumber.Text = value.SName;
                Direction.Text = _translator["DirectionOf", value.Direction];
                DepartureTime.Text =
                    reschedule ?
                    string.Format("{0} {1}", ((DateTime)realisticDepartureDateTime).ToString("HH:mm"), _translator["NewTime"]) :
                    scheduledDepartureDateTime.ToString("HH:mm");
                DepartureTime.Foreground = reschedule ? new SolidColorBrush(ColorHelper("Yellow")) : new SolidColorBrush(white);
                StatusIndicator.Background = reschedule ? new SolidColorBrush(ColorHelper("Yellow")) : null;
                TimeLeft.Text = departsIn.ToString(timeSpanFormat).TrimStart('0');
                TrackNumber.Text = value.Track ?? "-";
            }
        }

        public DepartureBoxControl()
        {
            InitializeComponent();
            _translator = new Translator(nameof(DeparturesWindow)); // <- Obs! Använder DeparturesWindow resources för tillfället!
        }

        private Color ColorHelper(string value) =>
            (Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), value);
    }
}
