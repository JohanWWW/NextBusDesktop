using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NextBusDesktop.ViewModels;

namespace NextBusDesktop.Controls
{
    public sealed partial class TripBoxControl : UserControl
    {
        public TripViewModel Trip { get; set; }
        public static DependencyProperty TripProperty = DependencyProperty.Register(nameof(Trip), typeof(TripViewModel), typeof(TripBoxControl), null);

        public TripBoxControl() => InitializeComponent();
    }
}
