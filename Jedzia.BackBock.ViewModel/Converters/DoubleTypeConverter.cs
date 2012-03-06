using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Globalization;
using Microsoft.Build.Utilities;

namespace Jedzia.BackBock.ViewModel.Converters
{
    public class DoubleTypeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : ((double)value).ToString(System.Threading.Thread.CurrentThread.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                       ? 0.0
                       : double.Parse((string)value, System.Threading.Thread.CurrentThread.CurrentCulture);
        }

        #endregion
    }
    public class TaskItemConverter : IValueConverter
    {
        //Todo: Move this to its own file.
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TaskItem a = new TaskItem(;
            if (value is TaskItem)
            {
                var val = (TaskItem)value;
                return val.ItemSpec;
            }
            //return value == null ? null : ((double)value).ToString(System.Threading.Thread.CurrentThread.CurrentCulture);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var val = (string)value;
                TaskItem a = new TaskItem(val);
                return a;
            }
            return value;
        }

        #endregion
    }
}
