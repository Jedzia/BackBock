namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class BindingConvertor : ExpressionConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
                return true;
            else return false;
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                         System.Globalization.CultureInfo culture,
                                         object value, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
            {
                BindingExpression bindingExpression = value as BindingExpression;
                if (bindingExpression == null)
                    throw new Exception("Error in BindingConvertor.ConvertTo: bindingExpression == null");
                return bindingExpression.ParentBinding;
            }
            else
            {

            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}