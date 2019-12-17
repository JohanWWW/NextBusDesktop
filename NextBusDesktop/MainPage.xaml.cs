using System;
using System.Threading.Tasks;
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
using Windows.Globalization;
using System.Resources;
using NextBusDesktop.DataProvider;
using NextBusDesktop.ResponseModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IAccessTokenProviderAsync _accessTokenProvider;
        private ITripPlannerProviderAsync _tripPlannerProvider;
        private AccessTokenResponse _accessToken;
        private Translator _translator;

        public MainPage()
        {
            InitializeComponent();

            _translator = new Translator(nameof(MainPage));
            InitializeTripPlannerProvider();

            UtilitiyListBox.SelectedIndex = 0;
            MainContentFrame.Navigate(typeof(HomeWindow));
        }

        private async void InitializeTripPlannerProvider()
        {
            _accessTokenProvider = new AccessTokenProvider();
            _accessToken = await _accessTokenProvider.GetAccessTokenAsync(); // TODO: Bug that causes program to crash when searching for departure and accessToken has not been provided.
            _tripPlannerProvider = new TripPlannerProvider(_accessToken);
        }

        private void OnPointerEnterMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = true;

        private void OnPointerExitMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = false;

        private void OnUtilityListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomeListItem.IsSelected)
                MainContentFrame.Navigate(typeof(HomeWindow));

            if (DeparturesListItem.IsSelected)
                MainContentFrame.Navigate(typeof(DeparturesWindow), _tripPlannerProvider);

            if (SettingsListItem.IsSelected)
                MainContentFrame.Navigate(typeof(SettingsWindow), new Action(() => 
                {
                    // Translate sidebar whenever the language setting is modified
                    HomeText.Text = _translator["Home.Text"];
                    DeparturesText.Text = _translator["Departures.Text"];
                    ArrivalsText.Text = _translator["Arrivals.Text"];
                    TripPlannerText.Text = _translator["TripPlanner.Text"];
                    SettingsText.Text = _translator["Settings.Text"];
                }));
        }
    }
}
