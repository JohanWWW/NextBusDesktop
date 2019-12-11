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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeparturesWindow : Page
    {
        private DataProvider.API _api;

        public DeparturesWindow()
        {
            InitializeComponent();
        }

        private void OnSearchTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string searchQuery = SearchTextBox.Text;
                var locationList = _api.GetLocationList(searchQuery);

                foreach (var stopLocation in locationList.Data.StopLocations)
                {
                    ListBoxItem listItem = new ListBoxItem();
                    listItem.Content = new TextBlock { Text = stopLocation.Name, TextWrapping = TextWrapping.Wrap };
                    listItem.Tag = stopLocation.Id;
                    SearchResultList.Items.Add(listItem);
                }

                MainSplitView.IsPaneOpen = true;
            }
        }

        private void OnSearchResultItemClick(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedListItem = SearchResultList.SelectedItem as ListBoxItem;
            string stopId = selectedListItem.Tag as string;

            DepartureListBox.Items.Clear();

            var departureBoard = _api.GetDepartureBoard(stopId);
            foreach (var departure in departureBoard.Data.Departures)
            {
                ListBoxItem departureBox = new ListBoxItem { Height = 150 };
                StackPanel panel = new StackPanel();
                TextBlock textBlock = new TextBlock { Text = departure.Name, FontSize = 56, Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)) };

                panel.Children.Add(textBlock);
                departureBox.Content = panel;
                DepartureListBox.Items.Add(departureBox);
            }

            MainSplitView.IsPaneOpen = false;

            // Unsubscribe in order to avoid invocation when the list is cleared.
            SearchResultList.SelectionChanged -= OnSearchResultItemClick;
            SearchResultList.Items.Clear();
            // Subscribe.
            SearchResultList.SelectionChanged += OnSearchResultItemClick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var api = e.Parameter as DataProvider.API;
            _api = api;
        }
    }
}
