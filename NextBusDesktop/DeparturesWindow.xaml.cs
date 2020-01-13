using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class DeparturesWindow : Page
    {
        private DispatcherTimer _departureBoardRefreshTimer;
        private DispatcherTimer _departureBoxTimer;

        public readonly DepartureBoardViewModel DepartureBoard = new DepartureBoardViewModel();

        public DeparturesWindow()
        {
            InitializeComponent();

            _departureBoardRefreshTimer = new DispatcherTimer();
            _departureBoardRefreshTimer.Interval = TimeSpan.FromMinutes(1f);
            _departureBoardRefreshTimer.Tick += OnDepartureBoardRefreshTimerTick;

            _departureBoxTimer = new DispatcherTimer();
            _departureBoxTimer.Interval = TimeSpan.FromSeconds(1f);
            _departureBoxTimer.Tick += OnDepartureBoxTimerTick;

            _departureBoardRefreshTimer.Start();
            _departureBoxTimer.Start();

            Unloaded += (s, e) => // Destroy timers
            {
                _departureBoardRefreshTimer.Stop();
                _departureBoardRefreshTimer.Tick -= OnDepartureBoardRefreshTimerTick;

                _departureBoxTimer.Stop();
                _departureBoxTimer.Tick -= OnDepartureBoxTimerTick;
            };
        }

        private void OnDepartureBoxTimerTick(object sender, object e)
        {
            foreach (var departureVm in DepartureBoard.Departures)
                departureVm.TriggerTimeUpdate();
        }

        private async void OnDepartureBoardRefreshTimerTick(object sender, object e) => await DepartureBoard.RefreshBoard();

        private void OnSearchTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                DepartureBoard.GetLocationList();
        }
    }
}
