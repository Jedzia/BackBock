namespace Jedzia.BackBock.ViewModel.ForTesting
{
    using System.Windows;
    using System.Windows.Controls;

    public class MenuItemStyleSelector : StyleSelector
    {
        public Style StaticItemStyle { get; set; }
        public Style DynamicItemStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Separator) return null;

            if (item is MenuItem) return StaticItemStyle;
            return DynamicItemStyle;
        }
    }
}
