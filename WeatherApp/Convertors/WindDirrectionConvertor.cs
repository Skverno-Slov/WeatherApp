using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Convertors
{
    public class WindDirrectionConvertor : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not null)
            {
                var degrees = System.Convert.ToInt32(value);
                string[] directions = { "С", "СВ", "В", "ЮВ", "Ю", "ЮЗ", "З", "СЗ" };
                int index = (int)((degrees + 22.5) / 45 % 8);
                return directions[index];
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
