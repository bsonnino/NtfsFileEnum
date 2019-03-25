using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Windows.Data;

namespace NtfsFileEnum
{
    public class ItemsSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            var items = value as ReadOnlyObservableCollection<object>;
            return items?.Sum(n => (double) ((INode)n).Size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}