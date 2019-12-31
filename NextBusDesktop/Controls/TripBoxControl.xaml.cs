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
using NextBusDesktop.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NextBusDesktop.Controls
{
    public sealed partial class TripBoxControl : UserControl
    {
        public TripViewModel Trip { get; set; }

        public static DependencyProperty TripProperty = DependencyProperty.Register("Trip", typeof(TripViewModel), typeof(TripBoxControl), null);

        public TripBoxControl() => InitializeComponent();

        private void CreateControl()
        {

        }
    }
}
