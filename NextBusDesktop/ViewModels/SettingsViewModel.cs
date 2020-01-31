using NextBusDesktop.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NextBusDesktop.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly Translator _translator;
        private readonly Action _externalUpdate;

        private ObservableCollection<LanguageViewModel> _languages;
        public ObservableCollection<LanguageViewModel> Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        private int _selectedLanguageIndex;
        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                SetProperty(ref _selectedLanguageIndex, value);

                if (value is -1) _selectedLanguage = null;
                else _selectedLanguage = _languages.ElementAt(value);
            }
        }

        private LanguageViewModel _selectedLanguage;
        public LanguageViewModel SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_languages != null)
                    SetProperty(ref _selectedLanguage, value);
            }
        }

        private string _languageLabel;
        public string LanguageLabel
        {
            get => _languageLabel;
            set => SetProperty(ref _languageLabel, value);
        }

        public SettingsViewModel()
        {
            _selectedLanguageIndex = -1;
            _translator = new Translator("SettingsWindow");
            _languageLabel = _translator["Language.Text"];
            _languages = new ObservableCollection<LanguageViewModel>(Translator.SupportedLanguages.Select(lang => new LanguageViewModel(_translator[lang], lang)));
        }

        public SettingsViewModel(Action externalUpdate) : this() => _externalUpdate = externalUpdate;

        public async void UpdateLanguage()
        {
            string code = SelectedLanguage.LanguageCode;
            Translator.SetLanguage(code);

            await Task.Delay(10);

            LanguageLabel = _translator["Language.Text"];

            foreach (var language in Languages)
                language.Language = _translator[language.LanguageCode];

            _externalUpdate?.Invoke();

            System.Diagnostics.Debug.WriteLine($"Updated language to: {Translator.CurrentLanguage}");
        }
    }
}
