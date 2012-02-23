using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jedzia.BackBock.ViewModel.Ext
{
    /// <summary>
    /// Implements a markup extension that creates types (including generics) at runtime.
    /// </summary>
    [ContentProperty("PropertyValues")]
    public class ActivatorExtension : MarkupExtension
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivatorExtension"/> class.
        /// </summary>
        public ActivatorExtension()
        {
            _propertyValues = new List<Setter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivatorExtension"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ActivatorExtension(Type type)
            : this()
        {
            _type = type;
        }
     
        #endregion

        #region Properties

        private Type _type;
        /// <summary>
        /// Gets or sets the type to create.
        /// </summary>
        /// <value>The type to create.</value>
        [ConstructorArgument("type")]
        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private readonly List<Setter> _propertyValues;
        /// <summary>
        /// Gets the property values.
        /// <remarks>
        /// This is the content property, so it can be specified in XAML directly beneath the object.
        /// </remarks>
        /// </summary>
        /// <value>The property values.</value>
        public List<Setter> PropertyValues
        {
            get { return _propertyValues; }
        }

        private object _value;
        /// <summary>
        /// Gets the created object.
        /// </summary>
        /// <value>The created object.</value>
        public object Value
        {
            get
            {
                if (_value == null)
                {
                    if (_type == null)
                    {
                        throw new InvalidOperationException("Type was not specified.");
                    }
                    
                    _value = Activator.CreateInstance(_type);

                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(_type);
                    foreach (Setter propertyValue in _propertyValues)
                    {
                        PropertyDescriptor property = properties[propertyValue.Name];
                        if (property != null)
                        {
                            DependencyPropertyDescriptor dpDescriptor = DependencyPropertyDescriptor.FromProperty(property);
                            BindingBase binding = BindingOperations.GetBindingBase(propertyValue, Setter.ValueProperty);

                            // if the property is data-bound, transfer the binding
                            if (dpDescriptor != null && binding != null)
                            {
                                BindingOperations.ClearBinding(propertyValue, Setter.ValueProperty);
                                BindingOperations.SetBinding((DependencyObject)_value, dpDescriptor.DependencyProperty, binding);
                            }
                            else if (propertyValue.Value != null)
                            {
                                Type propertyValueType = propertyValue.Value.GetType();
                                // if the value is assignable, assign it
                                if (property.PropertyType.IsAssignableFrom(propertyValueType))
                                {
                                    property.SetValue(_value, propertyValue.Value);
                                }
                                // try to use a type converter to get the value
                                else if (property.Converter.CanConvertFrom(propertyValueType))
                                {
                                    try
                                    {
                                        property.SetValue(_value, property.Converter.ConvertFrom(propertyValue.Value));
                                    }
                                    catch (FormatException ex)
                                    {
                                        throw new XamlParseException("Cannot convert value.", ex);
                                    }
                                }
                            }
                            // if the property is nullable and the assigned value is null, assign null
                            else if (property.PropertyType.IsClass || (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                property.SetValue(_value, null);
                            }
                        }
                        else
                        {
                            throw new XamlParseException(string.Format("Invalid property name '{0}'.", propertyValue.Name));
                        }
                    }
                }
                return _value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the constructed object.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }

        #endregion        
    }
}
