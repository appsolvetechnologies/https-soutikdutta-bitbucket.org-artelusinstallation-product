using System;
using System.Windows;
using System.Windows.Data;

namespace Helpers.Converter
{
    public class BoolToOppVisibilityConverter : BaseConverterMarkupExtension<BoolToOppVisibilityConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            if (bValue)
                return parameter != null && parameter.ToString() == "Collapsed" ? Visibility.Collapsed : Visibility.Hidden;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return false;
            else
                return true;
        }
    }
}
