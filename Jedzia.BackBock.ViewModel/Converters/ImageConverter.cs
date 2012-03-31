// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.ViewModel.Converters
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Provides value conversion for <see cref="System.Windows.Controls.Image"/>'s.
    /// </summary>
    [ValueConversion(typeof(Image), typeof(string))]
    public class ImageConverter : IValueConverter
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
            if (value == null)
            {
                return null;
            }

            System.Windows.Controls.Image img;
            if (value is string)
            {
                img = new System.Windows.Controls.Image
                          {
                              Source = new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute))
                          };
                return img;
            }
            
            IntPtr bitmap = ((Bitmap)value).GetHbitmap();
            try
            {
                // http://forums.asp.net/t/1411512.aspx/1
                // http://danbystrom.se/2009/01/05/imagegetthumbnailimage-and-beyond/
                // http://stackoverflow.com/questions/1789778/how-do-i-set-menuitems-icon-using-itemcontainerstyle
                var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                img = new System.Windows.Controls.Image { Source = bitmapSource };
            }
            finally
            {
                DeleteObject(bitmap);
            }

            return img;
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
            return null;
        }

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <param name="o">The object to delete.</param>
        /// <returns>the handle to the deleted object.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int DeleteObject(IntPtr o);
    }
}