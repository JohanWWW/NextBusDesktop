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

namespace NextBusDesktop.Controls
{
    public sealed partial class DepartureBoxControl : UserControl
    {
        public string Line
        {
            get => LineLogo.LineNumberText;
            set => LineLogo.LineNumberText = value;
        }

        public static DependencyProperty LineProperty = DependencyProperty.Register(nameof(Line), typeof(string), typeof(DepartureBoxControl), null);

        public string DirectionInfo
        {
            get => DirectionTextBlock.Text;
            set => DirectionTextBlock.Text = value;
        }

        public static DependencyProperty DirectionInfoProperty = DependencyProperty.Register(nameof(DirectionInfo), typeof(string), typeof(DepartureBoxControl), null);

        public string DepartureTimeInfo
        {
            get => DepartureTimeTextBlock.Text;
            set => DepartureTimeTextBlock.Text = value;
        }

        public static DependencyProperty DepartureTimeInfoProperty = DependencyProperty.Register(nameof(DepartureTimeInfo), typeof(string), typeof(DepartureBoxControl), null);

        public Brush LineLogoBackground
        {
            get => LineLogo.LineNumberBackground;
            set => LineLogo.LineNumberBackground = value;
        }

        public static DependencyProperty LineLogoBackgroundProperty = DependencyProperty.Register(nameof(LineLogoBackground), typeof(Brush), typeof(DepartureBoxControl), null);

        public Brush LineLogoForeground
        {
            get => LineLogo.LineNumberForeground;
            set => LineLogo.LineNumberForeground = value;
        }

        public static DependencyProperty LineLogoForegroundProperty = DependencyProperty.Register(nameof(LineLogoForeground), typeof(Brush), typeof(DepartureBoxControl), null);

        public Brush StatusIndicatorColor
        {
            get => StatusIndicatorStackPanel.Background;
            set => StatusIndicatorStackPanel.Background = value;
        }

        public static DependencyProperty StatusIndicatorColorProperty = DependencyProperty.Register(nameof(StatusIndicatorColor), typeof(Brush), typeof(DepartureBoxControl), null);

        public string TimeLeftInfo
        {
            get => TimeLeftTextBlock.Text;
            set => TimeLeftTextBlock.Text = value;
        }

        public static DependencyProperty TimeLeftInfoProperty = DependencyProperty.Register(nameof(TimeLeftInfo), typeof(string), typeof(DepartureBoxControl), null);

        public string Track
        {
            get => TrackNumberTextBlock.Text;
            set => TrackNumberTextBlock.Text = value;
        }

        public static DependencyProperty TrackProperty = DependencyProperty.Register(nameof(Track), typeof(string), typeof(DepartureBoxControl), null);

        public DepartureBoxControl() => InitializeComponent();
    }
}
