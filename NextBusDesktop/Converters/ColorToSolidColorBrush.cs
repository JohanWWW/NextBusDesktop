using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace NextBusDesktop.Converters
{
    public class ColorToSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => new SolidColorBrush((Color)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => (value as SolidColorBrush).Color;
    }
}
