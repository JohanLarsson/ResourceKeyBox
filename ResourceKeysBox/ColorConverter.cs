namespace ResourceKeysBox
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    public class ColorConverter : IValueConverter
    {
        private static readonly System.Windows.Media.ColorConverter Converter = new System.Windows.Media.ColorConverter();

        public static readonly ColorConverter Default = new ColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.ConvertFrom(value);
        }
    }
}
