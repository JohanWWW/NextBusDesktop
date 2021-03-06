﻿using System;

namespace NextBusDesktop.ViewModels
{
    public class LanguageViewModel : ViewModelBase
    {
        private string _language;
        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        public string LanguageCode { get; private set; }

        public LanguageViewModel(string language, string languageCode)
        {
            _language = language;
            LanguageCode = languageCode;
        }
    }
}
