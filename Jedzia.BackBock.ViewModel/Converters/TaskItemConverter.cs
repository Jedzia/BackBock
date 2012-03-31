namespace Jedzia.BackBock.ViewModel.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Microsoft.Build.Utilities;

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