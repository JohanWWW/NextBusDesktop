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
using System.Threading.Tasks;
using System.Threading;
using NextBusDesktop.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeparturesWindow : Page
    {
        private const byte _opaque = 255;

        private readonly DispatcherTimer _departureBoardRefreshTimer;
        private readonly DispatcherTimer _departureBoxTimer;
        private readonly Task _statusIndicatorTask;
        private readonly CancellationTokenSource _statusIndicatorTaskCancellationToken;

        public readonly DepartureBoardViewModel DepartureBoard = new DepartureBoardViewModel 
        { 
            Logger = new OutputLogger(nameof(DepartureBoardViewModel)),
            EnableLogging = true
        };

        public DeparturesWindow()
        {
            InitializeComponent();

            // Initialize timers
            _departureBoardRefreshTimer = new DispatcherTimer();
            _departureBoardRefreshTimer.Interval = TimeSpan.FromMinutes(1f);
            _departureBoardRefreshTimer.Tick += OnDepartureBoardRefreshTimerTick;

            _departureBoxTimer = new DispatcherTimer();
            _departureBoxTimer.Interval = TimeSpan.FromSeconds(1f);
            _departureBoxTimer.Tick += OnDepartureBoxTimerTick;

            _departureBoardRefreshTimer.Start();
            _departureBoxTimer.Start();

            // Initialize gui update tasks
            _statusIndicatorTaskCancellationToken = new CancellationTokenSource();
            _statusIndicatorTask = StatusIndicatorColorUpdate(_statusIndicatorTaskCancellationToken.Token);

            Unloaded += OnUnloaded;
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

        private Task StatusIndicatorColorUpdate(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            int x = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(30);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    byte amplitude = _opaque / 2;
                    byte yOffset = amplitude;
                    double frequency = 0.1;

                    // Yellow
                    Color statusColor = Color.FromArgb(
                        (byte)(amplitude * Math.Cos(frequency * x) + yOffset), // y = a * cos(x) + m
                        255,
                        255,
                        0
                    );

                    foreach (var departure in DepartureBoard.Departures)
                        if (departure.IsRescheduled)
                            departure.StatusIndicatorColor = statusColor;
                });
                x++;
            }
        }, cancellationToken);

        private async void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // Destroy timers
            _departureBoardRefreshTimer.Stop();
            _departureBoardRefreshTimer.Tick -= OnDepartureBoardRefreshTimerTick;
            _departureBoxTimer.Stop();
            _departureBoxTimer.Tick -= OnDepartureBoxTimerTick;

            // Cancel status indicator task
            _statusIndicatorTaskCancellationToken.Cancel();
            await _statusIndicatorTask; // Let task run to completion before disposal
            _statusIndicatorTask.Dispose();
        }
    }
}
