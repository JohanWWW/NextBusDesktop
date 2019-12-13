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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeparturesWindow : Page
    {
        private DataProvider.ITripPlannerProviderAsync _tripPlannerProvider;
        private Translator _translator;

        public DeparturesWindow()
        {
            InitializeComponent();
            _translator = new Translator(nameof(DeparturesWindow));
        }

        private async void OnSearchTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string searchQuery = SearchTextBox.Text;
                var locationList = await _tripPlannerProvider.GetLocationListAsync(searchQuery);

                if (locationList is null)
                {
                    Frame.Navigate(typeof(ErrorWindow), ErrorType.Unknown);
                    return;
                }

                if (locationList.StopLocations is null)
                {
                    Frame.Navigate(typeof(ErrorWindow), ErrorType.SearchResultEmpty);
                    return;
                }

                // Avoid invoking event when the list is cleared.
                SearchResultList.SelectionChanged -= OnSearchResultItemClick;
                SearchResultList.Items.Clear();
                SearchResultList.SelectionChanged += OnSearchResultItemClick;

                foreach (var stopLocation in locationList.StopLocations)
                {
                    SearchResultList.Items.Add(new ListBoxItem
                    {
                        Content = new TextBlock
                        {
                            Text = stopLocation.Name,
                            TextWrapping = TextWrapping.Wrap
                        },
                        Tag = stopLocation.Id
                    });
                }

                MainSplitView.IsPaneOpen = true;
            }
        }

        private async void OnSearchResultItemClick(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedListItem = SearchResultList.SelectedItem as ListBoxItem;
            SearchTextBox.Text = ((TextBlock)selectedListItem.Content).Text;
            string stopId = selectedListItem.Tag as string;

            DepartureListBox.Items.Clear();

            var departureBoard = await _tripPlannerProvider.GetDepartureBoardAsync(stopId);

            if (departureBoard is null)
            {
                Frame.Navigate(typeof(ErrorWindow), ErrorType.Unknown);
                return;
            }

            if (departureBoard.Departures is null)
            {
                Frame.Navigate(typeof(ErrorWindow), ErrorType.DeparturesNotFound);
                return;
            }

            foreach (var departure in departureBoard.Departures)
            {
                DepartureListBox.Items.Add(new ListBoxItem
                {
                    Content = CreateDepartureBox(departure)
                });
            }

            MainSplitView.IsPaneOpen = false;

            // Avoid invoking event when the list is cleared.
            SearchResultList.SelectionChanged -= OnSearchResultItemClick;
            SearchResultList.Items.Clear();
            SearchResultList.SelectionChanged += OnSearchResultItemClick;
        }

        private UIElement CreateDepartureBox(ResponseModels.Departure departure)
        {
            // Flip bgColor with fgColor because helper inverts the color for some unknown reason.
            Color bgColor = (Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), departure.ForegroundColor);
            Color fgColor = (Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), departure.BackgroundColor);
            Color white = Color.FromArgb(255, 255, 255, 255);

            DateTime scheduledDepartureDateTime = DateTime.ParseExact($"{departure.ScheduledDate} {departure.ScheduledTime}", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime realisticDepartureDateTime =
                string.IsNullOrEmpty(departure.RealisticTime) || string.IsNullOrEmpty(departure.RealisticDate) ? default :
                DateTime.ParseExact($"{departure.RealisticDate} {departure.RealisticTime}", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan departsIn = scheduledDepartureDateTime - DateTime.Now;

            string directionOfText = _translator["DirectionOf", departure.Direction];
            string timeUnit;
            string timeSpanFormat;
            if (departsIn.TotalMinutes < 60)
            {
                timeUnit = _translator["Minutes"];
                timeSpanFormat = "mm";
            }
            else if (departsIn.TotalHours < 24)
            {
                timeUnit = _translator["Hours"];
                timeSpanFormat = "h\\:mm";
            }
            else
            {
                timeUnit = _translator["Days"];
                timeSpanFormat = "dd";
            }

            string departsInText = _translator["DepartsIn", scheduledDepartureDateTime.ToString("HH:mm"), departsIn.ToString(timeSpanFormat).TrimStart('0'), timeUnit];

            // departure box
            return AddChildren(new StackPanel
            {
                Orientation = Orientation.Horizontal
            },
                // line logo
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Background = new SolidColorBrush(bgColor),
                    Width = 100,
                    Height = 60,
                    CornerRadius = new CornerRadius(10)
                },
                    new TextBlock
                    {
                        FontSize = 24,
                        Foreground = new SolidColorBrush(fgColor),
                        Text = departure.SName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 13, 0, 0)
                    }
                ),
                // direction
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                    new TextBlock
                    {
                        Text = directionOfText,
                        FontSize = 24,
                        Foreground = new SolidColorBrush(white),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                ),
                // departure information
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                    new TextBlock
                    {
                        Text = departsInText,
                        FontSize = 24,
                        Foreground = new SolidColorBrush(white),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                )
            );
        }

        private UIElement AddChildren(Panel panelElement, params UIElement[] children)
        {
            foreach (var child in children)
                panelElement.Children.Add(child);

            return panelElement;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var api = e.Parameter as DataProvider.ITripPlannerProviderAsync;
            _tripPlannerProvider = api;
        }
    }
}
