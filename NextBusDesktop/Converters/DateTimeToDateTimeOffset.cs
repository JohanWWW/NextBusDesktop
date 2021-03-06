﻿using System;
using Windows.UI.Xaml.Data;

namespace NextBusDesktop.Converters
{
    public class DateTimeToDateTimeOffset : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTime = (DateTime)value;
            return new DateTimeOffset(dateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var dateTimeOffset = (DateTimeOffset)value;
            return dateTimeOffset.DateTime;
        }
    }
}
