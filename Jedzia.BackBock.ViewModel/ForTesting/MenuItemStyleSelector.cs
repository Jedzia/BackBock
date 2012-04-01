namespace Jedzia.BackBock.ViewModel.ForTesting
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Custom style selector for <see cref="MenuItem"/>'s.
    /// </summary>
    public class MenuItemStyleSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets the static item style.
        /// </summary>
        /// <value>
        /// The static item style.
        /// </value>
        public Style StaticItemStyle { get; set; }
       
        /// <summary>
        /// Gets or sets the dynamic item style.
        /// </summary>
        /// <value>
        /// The dynamic item style.
        /// </value>
        public Style DynamicItemStyle { get; set; }

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.Style"/> based on custom logic.
        /// </summary>
        /// <param name="item">The content.</param>
        /// <param name="container">The element to which the style will be applied.</param>
        /// <returns>
        /// Returns an application-specific style to apply; otherwise, null.
        /// </returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Separator) return null;

            if (item is MenuItem) return StaticItemStyle;
            return DynamicItemStyle;
        }
    }
}
