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
using NextBusDesktop.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TripPlannerWindow : Page
    {
        private readonly DispatcherTimer _tripListRefreshTimer;
        private readonly DispatcherTimer _tripBoxTimer;

        public TripPlannerViewModel TripPlanner = new TripPlannerViewModel
        {
            Logger = new OutputLogger(nameof(TripPlannerViewModel)),
            EnableLogging = true
        };

        public TripPlannerWindow()
        {
            InitializeComponent();

            _tripListRefreshTimer = new DispatcherTimer();
            _tripListRefreshTimer.Interval = TimeSpan.FromMinutes(1f);
            _tripListRefreshTimer.Tick += OnTripListRefreshTimerTick;

            _tripBoxTimer = new DispatcherTimer();
            _tripBoxTimer.Interval = TimeSpan.FromSeconds(1f);
            _tripBoxTimer.Tick += OnTripBoxTimerTick;

            _tripListRefreshTimer.Start();
            _tripBoxTimer.Start();

            Unloaded += (s, e) => // Destroy timers
            {
                _tripListRefreshTimer.Stop();
                _tripListRefreshTimer.Tick -= OnTripListRefreshTimerTick;

                _tripBoxTimer.Stop();
                _tripBoxTimer.Tick -= OnTripBoxTimerTick;
            };
        }

        private async void OnTripListRefreshTimerTick(object sender, object e)
        {
            System.Diagnostics.Debug.WriteLine("Triggered refresh trip list.");
            //await TripPlanner.GetTripList();
            await TripPlanner.RefreshTripList();
        }

        private void OnTripBoxTimerTick(object sender, object e)
        {
            //System.Diagnostics.Debug.WriteLine("Triggered time update.");
            foreach (var trip in TripPlanner.Trips)
                trip.TriggerTimeUpdate();
        }

        private async void OnOriginTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                await TripPlanner.GetOriginLocationList();
        }

        private async void OnDestinationTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                await TripPlanner.GetDestinationLocationList();
        }
    }
}
