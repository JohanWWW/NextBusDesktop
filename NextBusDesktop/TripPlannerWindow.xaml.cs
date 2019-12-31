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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TripPlannerWindow : Page
    {
        public TripPlannerViewModel TripPlanner { get; set; }

        public TripPlannerWindow()
        {
            InitializeComponent();
            TripPlanner = new TripPlannerViewModel();
        }

        private void OnOriginTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                OriginResultsFlyout.ShowAt(OriginTextBox);
        }

        private void OnDestinationTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                DestinationResultsFlyout.ShowAt(DestinationTextBox);
        }

        private void OnOriginSelectionChanged(object sender, SelectionChangedEventArgs e) => OriginResultsFlyout.Hide();

        private void OnDestinationSelectionChanged(object sender, SelectionChangedEventArgs e) => DestinationResultsFlyout.Hide();
    }
}
