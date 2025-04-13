using System.Globalization;
using System.Windows.Data;

namespace EyeShield2.Converters;

class WorkPeriodConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            int minutes = (int)doubleValue;

            var postfix = minutes == 1 ? "minute" : "minutes";

            return $"{minutes} {postfix}";
        }

        return nameof(WorkPeriodConverter) + "failed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
