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
using NextBusDesktop.Models;
using Windows.UI;
using System.ComponentModel;
using NextBusDesktop.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NextBusDesktop
{
    public sealed partial class DepartureBoxControl : UserControl
    {
        private string _line;
        public string Line
        {
            get => _line;
            set
            {
                _line = value;
                LineNumber.Text = value;
            }
        }

        private string _directionInfo;
        public string DirectionInfo
        {
            get => _directionInfo;
            set
            {
                _directionInfo = value;
                Direction.Text = value;
            }
        }

        private string _departureTimeInfo;
        public string DepartureTimeInfo
        {
            get => _departureTimeInfo;
            set
            {
                _departureTimeInfo = value;
                DepartureTime.Text = value;
            }
        }

        private Brush _statusIndicatorColor;
        public Brush StatusIndicatorColor
        {
            get => _statusIndicatorColor;
            set
            {
                _statusIndicatorColor = value;
                StatusIndicator.Background = value;
            }
        }

        private string _timeLeftInfo;
        public string TimeLeftInfo
        {
            get => _timeLeftInfo;
            set
            {
                _timeLeftInfo = value;
                TimeLeft.Text = value;
            }
        }

        private string _track;
        public string Track
        {
            get => _track;
            set
            {
                _track = value;
                TrackNumber.Text = value;
            }
        }

        public DepartureBoxControl()
        {
            InitializeComponent();

            _line = LineNumber.Text;
            _directionInfo = Direction.Text;
            _departureTimeInfo = DepartureTime.Text;
            _statusIndicatorColor = StatusIndicator.Background;
            _timeLeftInfo = TimeLeft.Text;
            _track = TrackNumber.Text;
        }

        private Color ColorHelper(string value) =>
            (Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), value);
    }
}
