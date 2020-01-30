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
        private readonly DispatcherTimer _departureBoardRefreshTimer;
        private readonly DispatcherTimer _departureBoxTimer;

        private readonly Task _statusIndicatorTask;
        private readonly CancellationTokenSource _statusIndicatorTaskCancellationToken;

        private readonly Task _loadingIndicatorTask;
        private readonly CancellationTokenSource _loadingIndicatorTaskCancellationToken;

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

            _loadingIndicatorTaskCancellationToken = new CancellationTokenSource();
            _loadingIndicatorTask = LoadingIndicatorUpdate(_loadingIndicatorTaskCancellationToken.Token);

            LoadingIndicator.Fill = new SolidColorBrush();

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
                    // Yellow
                    Color statusColor = Color.FromArgb(
                        (byte)((255 / 2) * Math.Cos(0.1f * x) + (255 / 2)), // y = a * cos(f * x) + m
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
            System.Diagnostics.Debug.WriteLine("Cancel StatusIndicatorColorUpdate");
        }, cancellationToken);

        /// <summary>
        /// Quickly transition indicator to visible state when loading, 
        /// pulse while loading and slowly transition to invisible state when done loading.
        /// </summary>
        private Task LoadingIndicatorUpdate(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            // Linear function: y = k * x + m
            Func<int, int, int, int, byte> linear = (currentX, endX, startY, endY) =>
            {
                // Solve k
                float k = (float)(endY - startY) / endX;
                return (byte)(k * currentX + startY);
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(30);
                if (DepartureBoard.IsLoading)
                {
                    // Transition up
                    int x = 0;
                    while (x < 15)
                    {
                        await Task.Delay(30);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            LoadingIndicator.Fill = new SolidColorBrush(
                                color: Color.FromArgb(
                                    linear(x, 15, 0, 255),
                                    255,
                                    128,
                                    0
                                )
                            );
                        });
                        x++;
                    }

                    // The indicator pulses while loading
                    int i = 0;
                    while (DepartureBoard.IsLoading)
                    {
                        await Task.Delay(30);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            LoadingIndicator.Fill = new SolidColorBrush(
                                color: Color.FromArgb(
                                    (byte)((255 / 2 - 50) * Math.Cos(0.1f * i) + (255 / 2 + 50)),
                                    255,
                                    128,
                                    0
                                )
                            );
                        });
                        i++;
                    }

                    // Store last known alpha value for smooth transition
                    byte lastKnownValue = (byte)((255 / 2 - 50) * Math.Cos(0.1f * i) + (255 / 2 + 50));

                    // Transition down
                    x = 0;
                    while (x <= 30)
                    {
                        await Task.Delay(30);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            LoadingIndicator.Fill = new SolidColorBrush(
                                color: Color.FromArgb(
                                    linear(x, 30, lastKnownValue, 0),
                                    255,
                                    128,
                                    0
                                )
                            );
                        });
                        x++;
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("Cancel LoadingIndicatorUpdate");
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
            _loadingIndicatorTaskCancellationToken.Cancel();
            await _statusIndicatorTask; // Let task run to completion before disposal
            await _loadingIndicatorTask;

            // Dispose of resources
            _statusIndicatorTask.Dispose();
            _loadingIndicatorTask.Dispose();
        }
    }
}
