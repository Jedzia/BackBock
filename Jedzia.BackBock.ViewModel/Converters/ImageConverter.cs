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

    // Todo: Move this to Jedzia.BackBock.ViewModel.Converters
    [ValueConversion(typeof(Image), typeof(string))]
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            System.Windows.Controls.Image img;
            if (value is string)
            {
                img = new System.Windows.Controls.Image();
                img.Source = new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));
                return img;
            }
            IntPtr bitmap = ((Bitmap)value).GetHbitmap();
            try
            {
                // http://forums.asp.net/t/1411512.aspx/1
                // http://danbystrom.se/2009/01/05/imagegetthumbnailimage-and-beyond/
                // http://stackoverflow.com/questions/1789778/how-do-i-set-menuitems-icon-using-itemcontainerstyle
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                    );
                img = new System.Windows.Controls.Image();
                img.Source = bitmapSource;
            }
            finally
            {
                DeleteObject(bitmap);
            }

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int DeleteObject(IntPtr o);
    }
}