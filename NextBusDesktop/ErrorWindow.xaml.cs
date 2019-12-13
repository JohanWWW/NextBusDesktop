using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ErrorWindow : Page
    {
        private Translator _translator;

        public ErrorWindow()
        {
            InitializeComponent();
            _translator = new Translator(nameof(ErrorWindow));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ErrorType errorType = (ErrorType)e.Parameter;
            switch (errorType)
            {
                case ErrorType.DeparturesNotFound:
                    ErrorSymbolTextBlock.Text = "\uE9CE"; // Unknown (?)
                    ErrorMessageTextBlock.Text = _translator["DeparturesNotFound"];
                    await Task.Delay(5000);
                    Frame.GoBack();
                    break;
                case ErrorType.SearchResultEmpty:
                    ErrorSymbolTextBlock.Text = "\uE783"; // Error (!)
                    ErrorMessageTextBlock.Text = _translator["NoResults"];
                    await Task.Delay(5000);
                    Frame.GoBack();
                    break;
                case ErrorType.Unauthorized:
                    ErrorSymbolTextBlock.Text = "\uE72E"; // Lock
                    ErrorMessageTextBlock.Text = _translator["Unauthorized"];
                    break;
                case ErrorType.Unknown:
                    ErrorSymbolTextBlock.Text = "\uE783"; // Error (!)
                    ErrorMessageTextBlock.Text = _translator["ErrorOccurred"];
                    break;
            }
        }
    }
}
