using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;

namespace NextBusDesktop
{
    public class Translator
    {
        private readonly ResourceLoader _loader;

        public static string[] SupportedLanguages => ApplicationLanguages.ManifestLanguages.ToArray();
        public static string CurrentLanguage => ApplicationLanguages.PrimaryLanguageOverride;

        public Translator(string name) => _loader = ResourceLoader.GetForCurrentView(name);

        public string this[string resource]
        {
            get
            {
                string replaceDot = resource.Replace('.', '\\');
                return _loader.GetString(replaceDot);
            }
        }

        public string this[string resource, params object[] parameters]
        {
            get
            {
                string translation = this[resource];
                return string.Format(translation, parameters);
            }
        }

        public static void SetLanguage(string language) => ApplicationLanguages.PrimaryLanguageOverride = language;
    }
}
