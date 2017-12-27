using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using Liuliu.CodeCracker.Infrastructure;

using OSharp.Utility.Extensions;

namespace Liuliu.CodeCracker.Converters
{
    public class StringToPageSegModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PageSegMode)
            {
                PageSegMode mode = (PageSegMode)value;
                return $"{(int)mode}.{mode.ToString()}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string name = ((string)value).Substring(".", "");
                return Enum.Parse(typeof(PageSegMode), name);
            }
            return null;
        }
    }
}
