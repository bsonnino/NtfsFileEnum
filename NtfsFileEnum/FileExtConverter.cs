using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace NtfsFileEnum
{
    class FileExtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            var fileName = value as string;
            return string.IsNullOrWhiteSpace(fileName) ? 
                null : 
                Path.GetExtension(fileName).ToLowerInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
