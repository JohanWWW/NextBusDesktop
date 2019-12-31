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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NextBusDesktop.Controls
{
    public sealed partial class LineLogoControl : UserControl
    {
        public string LineNumberText
        {
            get => LineNumberTextBlock.Text;
            set => LineNumberTextBlock.Text = value;
        }

        public DependencyProperty LineNumberTextFontProperty = DependencyProperty.Register("LineNumberTextFont", typeof(FontFamily), typeof(LineLogoControl), null);
        public FontFamily LineNumberTextFont
        {
            get => LineNumberTextBlock.FontFamily;
            set => LineNumberTextBlock.FontFamily = value;
        }

        public double LineNumberTextSize
        {
            get => LineNumberTextBlock.FontSize;
            set => LineNumberTextBlock.FontSize = value;
        }

        public Brush LineNumberForeground
        {
            get => LineNumberTextBlock.Foreground;
            set => LineNumberTextBlock.Foreground = value;
        }

        public Brush LineNumberBackground
        {
            get => LineLogoStackPanel.Background;
            set => LineLogoStackPanel.Background = value;
        }

        public LineLogoControl()
        {
            this.InitializeComponent();
        }
    }
}
