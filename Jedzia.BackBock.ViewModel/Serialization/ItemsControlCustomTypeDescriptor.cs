namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Controls;

    internal class ItemsControlCustomTypeDescriptor : CustomTypeDescriptor
    {
        public ItemsControlCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(base.GetProperties().Cast<PropertyDescriptor>().ToArray());
            return ConvertPropertys(pdc);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(base.GetProperties(attributes).Cast<PropertyDescriptor>().ToArray());
            return ConvertPropertys(pdc);
        }

        private PropertyDescriptorCollection ConvertPropertys(PropertyDescriptorCollection pdc)
        {
            PropertyDescriptor pd = pdc.Find("ItemsSource", false);
            if (pd != null)
            {
                PropertyDescriptor pdNew = TypeDescriptor.CreateProperty(typeof(ItemsControl), pd, new Attribute[]
                                                                                                       {
                                                                                                           new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible),
                                                                                                           new DefaultValueAttribute("")
                                                                                                       });
                pdc.Add(pdNew);
                pdc.Remove(pd);
            }
            return pdc;
        }
    }
}