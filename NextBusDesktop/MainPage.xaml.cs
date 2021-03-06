﻿using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NextBusDesktop.DataProvider;
using NextBusDesktop.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly Translator _translator;

        public MainPage()
        {
            InitializeComponent();

            _translator = new Translator("MainPage");

            UtilitiyListBox.SelectedIndex = 0;
            MainContentFrame.Navigate(typeof(TripPlannerWindow));
        }

        private void OnPointerEnterMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = true;

        private void OnPointerExitMainSplitViewPane(object sender, PointerRoutedEventArgs e) => MainSplitView.IsPaneOpen = false;

        private void OnUtilityListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TripPlannerListItem.IsSelected)
                MainContentFrame.Navigate(typeof(TripPlannerWindow));

            if (DeparturesListItem.IsSelected)
                MainContentFrame.Navigate(typeof(DeparturesWindow));

            if (SettingsListItem.IsSelected)
                MainContentFrame.Navigate(typeof(SettingsWindow), new Action(() => 
                {
                    // Translate sidebar whenever the language setting is modified
                    DeparturesText.Text = _translator["Departures.Text"];
                    TripPlannerText.Text = _translator["TripPlanner.Text"];
                    SettingsText.Text = _translator["Settings.Text"];
                }));
        }
    }
}
