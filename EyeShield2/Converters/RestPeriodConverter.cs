using System.Globalization;
using System.Windows.Data;

namespace EyeShield2.Converters;

class RestPeriodConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            var restTime = TimeSpan.FromSeconds((int)doubleValue);

            return restTime.ToString(@"mm\:ss");
        }

        return nameof(RestPeriodConverter) + "failed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
