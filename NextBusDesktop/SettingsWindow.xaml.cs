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
using Windows.ApplicationModel.Resources;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWindow : Page
    {
        private Translator _translator;
        private Action _translateMainPageSideBar;

        public SettingsWindow()
        {
            _translator = new Translator(nameof(SettingsWindow));

            this.InitializeComponent();

            LanguageComboBox.Items.Add(new ComboBoxItem
            {
                Content = new TextBlock
                {
                    Text = _translator["Auto"]
                },
                Tag = System.Globalization.CultureInfo.CurrentCulture.Name // Computer culture
            });

            foreach (string language in _translator.SupportedLanguages)
            {
                LanguageComboBox.Items.Add(new ComboBoxItem
                {
                    Content = new TextBlock
                    {
                        Text = _translator[language]
                    },
                    Tag = language
                });
            }

            LanguageComboBox.SelectionChanged += LanguageComboBoxSelectionChanged;
        }

        private async void LanguageComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLanguage = ((ComboBoxItem)LanguageComboBox.SelectedItem).Tag as string;
            ApplicationLanguages.PrimaryLanguageOverride = selectedLanguage;

            await Task.Delay(100);
            _translateMainPageSideBar();

            Frame.Navigate(GetType(), _translateMainPageSideBar);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _translateMainPageSideBar = e.Parameter as Action; 
        }
    }
}
