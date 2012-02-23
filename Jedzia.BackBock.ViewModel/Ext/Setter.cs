using System;
using System.Windows;
using System.Windows.Markup;

namespace Jedzia.BackBock.ViewModel.Ext
{
    /// <summary>
    /// Represents a single property value for the <see cref="ActivatorExtension"/>.
    /// </summary>
    [ContentProperty("Value")]
    public class Setter : DependencyObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        /// <value>The property name.</value>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Setter), new PropertyMetadata());

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>The property value.</value>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(Setter), new PropertyMetadata());

        #endregion
    }
}
