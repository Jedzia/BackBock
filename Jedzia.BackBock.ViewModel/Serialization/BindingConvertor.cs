namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Converts binding level XAML-Markup Expressions by forwarding the 
    /// <see cref="BindingExpression.ParentBinding"/> of the <see cref="BindingExpression"/>.
    /// </summary>
    public class BindingConvertor : ExpressionConverter
    {
        /// <summary>
        /// Determines if a type can be converted to XAML-Markup.
        /// </summary>
        /// <param name="context">Execution context of the binder.</param>
        /// <param name="destinationType">The destination type of the conversion.</param>
        /// <returns><c>true</c> if the type can be converted, otherwise <c>false</c>.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
                return true;
            else return false;
        }

        /// <summary>
        /// Converts an object to XAML-Markup.
        /// </summary>
        /// <param name="context">Execution context of the binder.</param>
        /// <param name="culture">Currently used <see cref="System.Globalization.CultureInfo"/>
        /// of the conversion in progress.</param>
        /// <param name="value">The value of te object that should be converted.</param>
        /// <param name="destinationType">The destination type of the conversion.</param>
        /// <returns>
        /// The object to convert.
        /// </returns>
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