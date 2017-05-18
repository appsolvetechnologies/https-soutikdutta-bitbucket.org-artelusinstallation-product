﻿using System;
using System.Windows;
using System.Windows.Data;


namespace Helpers.Converter
{
    public class BoolToVisibilityConverter : BaseConverterMarkupExtension<BoolToVisibilityConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            if (bValue)
                return Visibility.Visible;
            else
                return parameter != null && parameter.ToString() == "Collapsed" ? Visibility.Collapsed : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
