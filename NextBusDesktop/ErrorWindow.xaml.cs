using System;
using System.Net;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NextBusDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ErrorWindow : Page
    {
        public ErrorWindow()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var statusCode = (HttpStatusCode)e.Parameter;

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    ErrorSymbolTextBlock.Text = "\uE72E";
                    ErrorMessageTextBlock.Text = "Unauthorized";
                    break;
                default:

                    break;
            }
        }
    }
}
