namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    public class ItemsControlTypeDescriptionProvider : TypeDescriptionProvider
    {
        private static readonly TypeDescriptionProvider defaultTypeProvider = TypeDescriptor.GetProvider(typeof(ItemsControl));

        public ItemsControlTypeDescriptionProvider()
            : base(defaultTypeProvider)
        {
        }

        public static void Register()
        {
            TypeDescriptor.AddProvider(new ItemsControlTypeDescriptionProvider(), typeof(ItemsControl));
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);
            return instance == null ? defaultDescriptor : new ItemsControlCustomTypeDescriptor(defaultDescriptor);
        }
    }
}