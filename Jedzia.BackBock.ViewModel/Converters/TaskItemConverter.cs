// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskItemConverter.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Provides value conversion for <see cref="TaskItem"/>'s.
    /// </summary>
    [ValueConversion(typeof(TaskItem), typeof(string))]
    public class TaskItemConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TaskItem a = new TaskItem(;
            if (value is TaskItem)
            {
                var val = (TaskItem)value;
                return val.ItemSpec;
            }

            // return value == null ? null : ((double)value).ToString(System.Threading.Thread.CurrentThread.CurrentCulture);
            return value;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return new TaskItem((string)value);
            }

            return value;
        }
    }
}