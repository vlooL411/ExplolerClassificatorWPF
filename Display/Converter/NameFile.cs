using System;
using System.Globalization;
using System.Windows.Data;

namespace ExplolerClassificatorWPF.Display.Converter
{
    public class NameFile : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is InfoFile infoFile && infoFile.Name != null)
                if (parameter is string num && int.TryParse(num, out var number))
                    if (infoFile.Name != null)
                    {
                        if (infoFile.Name[0] == '.' || infoFile.Name.Length < number) return infoFile.Name;
                        if (infoFile.Type == TypeInfo.File && infoFile.Extension != null)
                        {
                            var header = infoFile.Name.Substring(0, number - infoFile.Extension.Length);
                            return header += $"..{infoFile.Extension}";
                        }
                        return $"{infoFile.Name.Substring(0, number)}..";
                    }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}