using System.Globalization;
using System.Windows.Data;

namespace EyeShield2.Converters;

class TimeRemainingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes >= 2)
            {
                return $"{(int)(timeSpan.TotalMinutes)} minutes remaining {timeSpan:mm\\:ss\\:f}";
            }

            if (timeSpan.TotalSeconds >= 6)
            {
                return timeSpan.ToString(@"mm\:ss");
            }

            return timeSpan.ToString(@"mm\:ss\:ff");
        }

        return nameof(TimeRemainingConverter) + " error";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
