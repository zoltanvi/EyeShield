using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace EyeShield2.Converters;

class StartStopButtonAppearanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? ControlAppearance.Caution : ControlAppearance.Info;
        }

        return ControlAppearance.Primary;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
