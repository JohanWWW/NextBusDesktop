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
        public string Line
        {
            get => LineNumberTextBlock.Text;
            set => LineNumberTextBlock.Text = value;
        }

        public string DirectionInfo
        {
            get => DirectionTextBlock.Text;
            set => DirectionTextBlock.Text = value;
        }

        public string DepartureTimeInfo
        {
            get => DepartureTimeTextBlock.Text;
            set => DepartureTimeTextBlock.Text = value;
        }

        public Brush LineLogoBackground
        {
            get => LineLogoStackPanel.Background;
            set => LineLogoStackPanel.Background = value;
        }

        public Brush LineLogoForeground
        {
            get => LineNumberTextBlock.Foreground;
            set => LineNumberTextBlock.Foreground = value;
        }

        public Brush StatusIndicatorColor
        {
            get => StatusIndicatorStackPanel.Background;
            set => StatusIndicatorStackPanel.Background = value;
        }

        public string TimeLeftInfo
        {
            get => TimeLeftTextBlock.Text;
            set => TimeLeftTextBlock.Text = value;
        }

        public string Track
        {
            get => TrackNumberTextBlock.Text;
            set => TrackNumberTextBlock.Text = value;
        }

        public DepartureBoxControl()
        {
            InitializeComponent();
        }

        private Color ColorHelper(string value) =>
            (Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), value);
    }
}
