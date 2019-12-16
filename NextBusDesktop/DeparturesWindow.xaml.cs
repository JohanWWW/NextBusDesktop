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
        private ResponseModels.DepartureBoard _currentDepartureBoard;

        public DeparturesWindow()
        {
            InitializeComponent();
            _translator = new Translator(nameof(DeparturesWindow));
            DateTime now = DateTime.Now;
            DepartureTimePicker.Time = new TimeSpan(now.Hour, now.Minute, 00);
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

            TimeSpan depTime = DepartureTimePicker.Time;
            DateTime selectedDepartureTime = DateTime.ParseExact(DateTime.Today.ToString("yyyy-MM-dd") + " " + depTime.ToString(@"hh\:mm"), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            DepartureListBox.Items.Clear();

            var departureBoard = await _tripPlannerProvider.GetDepartureBoardAsync(stopId, selectedDepartureTime);
            _currentDepartureBoard = departureBoard;

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
                //DepartureListBox.Items.Add(CreateDepartureBox(departure));
                DepartureListBox.Items.Add(new ListBoxItem
                {
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    Content = new DepartureBoxControl { Departure = departure }
                });
            }

            MainSplitView.IsPaneOpen = false;

            // Avoid invoking event when the list is cleared.
            SearchResultList.SelectionChanged -= OnSearchResultItemClick;
            SearchResultList.Items.Clear();
            SearchResultList.SelectionChanged += OnSearchResultItemClick;

            TrackComboBox.SelectionChanged -= OnTrackChanged;
            TrackComboBox.Items.Clear();

            TrackComboBox.Items.Add(new ComboBoxItem
            {
                Content = new TextBlock
                {
                    Text = _translator["All"]
                },
                Tag = "*" // All
            });

            foreach (var track in departureBoard.Departures.Select(d => d.Track).Distinct().OrderBy(t => t))
            {
                TrackComboBox.Items.Add(new ComboBoxItem
                {
                    Content = new TextBlock
                    {
                        Text = track ?? _translator["Unprovided"],
                        Language = _translator.CurrentLanguage
                    },
                    Tag = track
                });
            }

            TrackComboBox.SelectedIndex = 0;
            TrackComboBox.SelectionChanged += OnTrackChanged;

        }

        [Obsolete]
        private UIElement CreateDepartureBox(ResponseModels.Departure departure)
        {
            // Flip bgColor with fgColor because helper inverts the color for some unknown reason.
            Color bgColor = ColorHelper(departure.ForegroundColor);
            Color fgColor = ColorHelper(departure.BackgroundColor);
            Color white = Color.FromArgb(255, 255, 255, 255);

            DateTime scheduledDepartureDateTime = departure.GetScheduledDateTime();
            DateTime? realisticDepartureDateTime = departure.GetRealisticDateTime();

            bool reschedule;
            if (realisticDepartureDateTime != null)
                reschedule = scheduledDepartureDateTime != realisticDepartureDateTime;
            else
                reschedule = false;

            TimeSpan departsIn = reschedule ? (DateTime)realisticDepartureDateTime - DateTime.Now : scheduledDepartureDateTime - DateTime.Now;

            string directionOfText = _translator["DirectionOf", departure.Direction];
            string timeUnit;
            string timeSpanFormat;
            if (departsIn.TotalMinutes < 1)
            {
                timeUnit = _translator["Minutes"];
                timeSpanFormat = @"\n\o\w";
            }
            else if (departsIn.TotalMinutes < 60)
            {
                timeUnit = _translator["Minutes"];
                timeSpanFormat = @"mm\ \m\i\n";
            }
            else if (departsIn.TotalHours < 24)
            {
                timeUnit = _translator["Hours"];
                timeSpanFormat = @"h\h\ mm\m\i\n";
            }
            else
            {
                timeUnit = _translator["Days"];
                timeSpanFormat = @"dd\d";
            }

            string departsInText = _translator["DepartsIn", scheduledDepartureDateTime.ToString("HH:mm"), departsIn.ToString(timeSpanFormat).TrimStart('0'), timeUnit];

            Grid grid = AddDefinitions(new Grid(),
                (new ColumnDefinition
                {
                    Width = new GridLength(100, GridUnitType.Pixel)
                },
                null),
                (new ColumnDefinition
                {
                    Width = new GridLength(40, GridUnitType.Star)
                },
                null),
                (new ColumnDefinition
                {
                    Width = new GridLength(20, GridUnitType.Star)
                },
                null),
                (new ColumnDefinition
                {
                    Width = new GridLength(5, GridUnitType.Pixel)
                }, 
                null),
                (new ColumnDefinition
                {
                    Width = new GridLength(30, GridUnitType.Star)
                }, 
                null),
                (new ColumnDefinition
                {
                    Width = new GridLength(10, GridUnitType.Star)
                },
                null)
            );

            grid = (Grid)AddChildren(grid,
                // line logo
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Background = new SolidColorBrush(bgColor),
                    Height = 60,
                    CornerRadius = new CornerRadius(10)
                },
                    new TextBlock
                    {
                        Text = departure.SName,
                        FontSize = 24,
                        Foreground = new SolidColorBrush(fgColor),
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
                        FontSize = 18,
                        Foreground = new SolidColorBrush(white),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                ),
                // departure time
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                    new TextBlock
                    {
                        Text = reschedule ? 
                            $"{((DateTime)realisticDepartureDateTime).ToString("HH:mm")} {_translator["NewTime"]}" :
                            scheduledDepartureDateTime.ToString("HH:mm"),
                        FontSize = 18,
                        Foreground = new SolidColorBrush(white),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                ),
                // status indicator
                new StackPanel
                {
                    Background = reschedule ? 
                        new SolidColorBrush(ColorHelper("yellow")) : 
                        null,
                    CornerRadius = new CornerRadius(5)
                },
                // time remaining until departure
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                    new TextBlock
                    {
                        Text = departsIn.ToString(timeSpanFormat).TrimStart('0'),
                        FontSize = 18,
                        Foreground = new SolidColorBrush(white),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                ),
                // track
                AddChildren(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                    new TextBlock
                    {
                        Text = departure.Track ?? "-",
                        FontSize = 18,
                        Foreground = new SolidColorBrush(white),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                )
            );

            for (int i = 0; i < grid.Children.Count; i++)
            {
                Grid.SetColumn((FrameworkElement)grid.Children.ElementAt(i), i);
            }


            return new ListBoxItem
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Content = grid
            };
        }

        private UIElement AddChildren(Panel panelElement, params UIElement[] children)
        {
            foreach (var child in children)
                panelElement.Children.Add(child);

            return panelElement;
        }

        private Grid AddDefinitions(Grid grid, params (ColumnDefinition columnDefinition, RowDefinition rowDefinition)[] definitions)
        {
            foreach (var (columnDefinition, rowDefinition) in definitions)
            {
                if (columnDefinition != null)
                    grid.ColumnDefinitions.Add(columnDefinition);

                if (rowDefinition != null)
                    grid.RowDefinitions.Add(rowDefinition);

            }

            return grid;
        }

        private Grid SetColumnRow(Grid grid, params (int column, int row, UIElement child)[] children)
        {
            foreach (var child in children)
            {
                Grid.SetColumn(grid, child.column);
                Grid.SetRow(grid, child.row);
            }

            return grid;
        }

        private Color ColorHelper(string value) => 
            (Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), value);

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var api = e.Parameter as DataProvider.ITripPlannerProviderAsync;
            _tripPlannerProvider = api;
        }

        private void OnTrackChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear list and refill according to filter.
            DepartureListBox.Items.Clear();

            ComboBoxItem selectedTrack = TrackComboBox.SelectedItem as ComboBoxItem;
            string track = selectedTrack.Tag as string;

            if (track is "*") // All
            {
                foreach (var departure in _currentDepartureBoard.Departures)
                    //DepartureListBox.Items.Add(CreateDepartureBox(departure));
                    DepartureListBox.Items.Add(new ListBoxItem
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        Content = new DepartureBoxControl { Departure = departure }
                    });
            }
            else
            {
                var filteredDepartures = _currentDepartureBoard.Departures.Where(d => d.Track == track);
                foreach (var departure in filteredDepartures)
                    //DepartureListBox.Items.Add(CreateDepartureBox(departure));
                    DepartureListBox.Items.Add(new ListBoxItem
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        Content = new DepartureBoxControl { Departure = departure }
                    });
            }

        }
    }
}
