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
        private AccessToken _accessToken;
        private Translator _translator;

        public MainPage()
        {
            ApplicationLanguages.PrimaryLanguageOverride = "sv-SE";
            _translator = new Translator(nameof(MainPage));
            InitializeComponent();
            Startup();
            HomeListItem.Tag = typeof(HomeWindow);
            DeparturesListItem.Tag = typeof(DeparturesWindow);

            UtilitiyListBox.SelectedIndex = 0;
        }

        private async void Startup()
        {
            _accessTokenProvider = new AccessTokenProvider();
            _accessToken = await _accessTokenProvider.GetAccessTokenAsync();
            _tripPlannerProvider = new TripPlannerProvider(_accessToken);
        }

        private void OnPointerEnterMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = true;

        private void OnPointerExitMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = false;

        private void OnUtilityListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            Type pageType = (UtilitiyListBox.SelectedItem as ListBoxItem).Tag as Type;

            if (!MainContentFrame.Navigate(pageType, _tripPlannerProvider))
                throw new NotImplementedException("Not yet implemented!");
        }
    }
}
