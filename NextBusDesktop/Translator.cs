using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Windows.ApplicationModel.Resources;

namespace NextBusDesktop
{
    public class Translator
    {
        private ResourceLoader _loader;

        public Translator(string name) => 
            _loader = ResourceLoader.GetForCurrentView(name);

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
    }
}
