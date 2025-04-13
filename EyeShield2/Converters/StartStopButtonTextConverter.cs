using System.Globalization;
using System.Windows.Data;

namespace EyeShield2.Converters;

class StartStopButtonTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Stop" : "Start";
        }

        return nameof(StartStopButtonTextConverter) + "failed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
