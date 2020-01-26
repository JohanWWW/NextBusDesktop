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
using System.Threading.Tasks;
using System.Threading;
using Windows.UI;

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

        private readonly Task _loadingIndicatorTask;
        private readonly CancellationTokenSource _loadingIndicatorTaskCancellationToken;

        public TripPlannerViewModel TripPlanner = new TripPlannerViewModel
        {
            Logger = new OutputLogger(nameof(TripPlannerViewModel)),
            EnableLogging = true
        };

        public TripPlannerWindow()
        {
            InitializeComponent();

            // Initialize timers
            _tripListRefreshTimer = new DispatcherTimer();
            _tripListRefreshTimer.Interval = TimeSpan.FromMinutes(1f);
            _tripListRefreshTimer.Tick += OnTripListRefreshTimerTick;

            _tripBoxTimer = new DispatcherTimer();
            _tripBoxTimer.Interval = TimeSpan.FromSeconds(1f);
            _tripBoxTimer.Tick += OnTripBoxTimerTick;

            _tripListRefreshTimer.Start();
            _tripBoxTimer.Start();

            LoadingIndicator.Fill = new SolidColorBrush();

            // Initialize gui update tasks
            _loadingIndicatorTaskCancellationToken = new CancellationTokenSource();
            _loadingIndicatorTask = LoadingIndicatorUpdate(_loadingIndicatorTaskCancellationToken.Token);

            Unloaded += OnUnloaded;
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

        private Task LoadingIndicatorUpdate(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            Func<int, int, int, int, byte> linear = (currentX, endX, startY, endY) =>
            {
                float k = (float)(endY - startY) / endX;
                return (byte)(k * currentX + startY);
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(30);
                if (TripPlanner.IsLoading)
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

                    // Keep same level until done loading.
                    while (TripPlanner.IsLoading) await Task.Delay(30);

                    // Transition down
                    x = 50;
                    while (x >= 0)
                    {
                        await Task.Delay(30);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            LoadingIndicator.Fill = new SolidColorBrush(
                                color: Color.FromArgb(
                                    linear(x, 50, 0, 255),
                                    255,
                                    128,
                                    0
                                )
                            );
                        });
                        x--;
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("Cancel LoadingIndicatorUpdate");
        }, cancellationToken);

        private async void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // Destroy timers
            _tripListRefreshTimer.Stop();
            _tripListRefreshTimer.Tick -= OnTripListRefreshTimerTick;

            _tripBoxTimer.Stop();
            _tripBoxTimer.Tick -= OnTripBoxTimerTick;

            // Cancel and destroy tasks
            _loadingIndicatorTaskCancellationToken.Cancel();
            await _loadingIndicatorTask;
            _loadingIndicatorTask.Dispose();
            _loadingIndicatorTaskCancellationToken.Dispose();
        }
    }
}
