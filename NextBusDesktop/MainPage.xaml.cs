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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private API.API _api;

        public MainPage()
        {
            InitializeComponent();

            HomeListItem.Tag = typeof(HomeWindow);
            DeparturesListItem.Tag = typeof(DeparturesWindow);

            UtilitiyListBox.SelectedIndex = 0;
            MainContentFrame.Navigate(typeof(HomeWindow));
        }

         private void OnPointerEnterMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = true;

        private void OnPointerExitMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = false;

        private void OnUtilityListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            Type pageType = (UtilitiyListBox.SelectedItem as ListBoxItem).Tag as Type;

            if (!MainContentFrame.Navigate(pageType))
                throw new NotImplementedException("Not yet implemented!");
        }
    }
}
