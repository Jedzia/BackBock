// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayModeToVisibility.cs" company="EvePanix">
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
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Provides value conversion for <see cref="DisplayMode"/> enumerations.
    /// </summary>
    [ValueConversion(typeof(DisplayMode), typeof(string))]
    public sealed class DisplayModeToVisibility : DependencyObject, IValueConverter
    {
        #region Fields

        /// <summary>
        /// Invert the behavior of this instance. Boolean <c>true</c> values are expressed as <c>Collapsed</c>.
        /// </summary>
        public static readonly DependencyProperty InvertProperty =
            DependencyProperty.Register(
                "Invert",
                typeof(bool),
                typeof(DisplayModeToVisibility),
                new FrameworkPropertyMetadata(false));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ObjectToVisibility"/> has invert behavior.
        /// </summary>
        /// <value>
        /// <c>true</c> if inverted; otherwise, <c>false</c>.
        /// </value>
        public bool Invert
        {
            get
            {
                return (bool)GetValue(InvertProperty);
            }

            set
            {
                SetValue(InvertProperty, value);
            }
        }

        #endregion

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
            if ((value != null) ^ this.Invert)
            {
                return Visibility.Visible.ToString();
            }

            // return "";
            return Visibility.Collapsed.ToString();

            // return "Class";
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
        /// <remarks>
        /// No need to implement converting back on a one-way binding.
        /// </remarks>
        /// <exception cref="NotSupportedException"><c>NotSupportedException</c>.</exception>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}