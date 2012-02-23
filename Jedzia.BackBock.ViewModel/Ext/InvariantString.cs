using System;
using System.Globalization;

namespace Jedzia.BackBock.ViewModel.Ext
{
    internal static class InvariantString
    {
        public static string Format(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
