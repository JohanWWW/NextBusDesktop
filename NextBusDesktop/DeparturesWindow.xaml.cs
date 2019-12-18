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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeparturesWindow : Page
    {
        public DepartureBoardViewModel DepartureBoard { get; set; }


        public DeparturesWindow()
        {
            InitializeComponent();
            DepartureBoard = new DepartureBoardViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void OnTrackChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
