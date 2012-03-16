namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.ComponentModel;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex.Conversion;

    /// <summary>
    /// Attempts to utilize an existing <see cref="TypeConverter"/> for conversion
    /// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class ComponentModelConverter : AbstractTypeConverter
    {
        public override bool CanHandleType(Type type)
        {
            // Mono 1.9+ thinks it can convert strings to interface
            if (type.IsInterface)
            {
                return false;
            }

            var converter = TypeDescriptor.GetConverter(type);
            return (converter != null && converter.CanConvertFrom(typeof(String)));
        }

        public override object PerformConversion(String value, Type targetType)
        {
            var converter = TypeDescriptor.GetConverter(targetType);

            try
            {
                return converter.ConvertFrom(value);
            }
            catch (Exception ex)
            {
                var message = String.Format(
                    "Could not convert from '{0}' to {1}",
                    value, targetType.FullName);

                throw new ConverterException(message, ex);
            }
        }

        public override object PerformConversion(IConfiguration configuration, Type targetType)
        {
            return PerformConversion(configuration.Value, targetType);
        }
    }
}