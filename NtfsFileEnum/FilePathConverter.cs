using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace NtfsFileEnum
{
    class FilePathConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            var fileName = value as string;
            return string.IsNullOrWhiteSpace(fileName) ?
                null :
                GetTopPath(fileName);
        }

        private string GetTopPath(string fileName)
        {
            var paths = fileName.Split(Path.DirectorySeparatorChar).Take(2);
            return string.Join(Path.DirectorySeparatorChar.ToString(), paths);
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
