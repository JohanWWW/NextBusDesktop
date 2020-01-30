using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NextBusDesktop.ViewModels;

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWindow : Page
    {
        private Action _translateMainPageSideBar;

        public SettingsViewModel Settings { get; set; }

        public SettingsWindow() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _translateMainPageSideBar = e.Parameter as Action;
            Settings = new SettingsViewModel(_translateMainPageSideBar);
        }
    }
}
