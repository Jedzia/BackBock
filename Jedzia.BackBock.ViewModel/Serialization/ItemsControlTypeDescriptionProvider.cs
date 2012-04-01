namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Provides custom metadata for <see cref="ItemsControl"/>'s XAML-Markup serialization.
    /// </summary>
    public class ItemsControlTypeDescriptionProvider : TypeDescriptionProvider
    {
        private static readonly TypeDescriptionProvider defaultTypeProvider = TypeDescriptor.GetProvider(typeof(ItemsControl));

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsControlTypeDescriptionProvider"/> class.
        /// </summary>
        public ItemsControlTypeDescriptionProvider()
            : base(defaultTypeProvider)
        {
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        public static void Register()
        {
            TypeDescriptor.AddProvider(new ItemsControlTypeDescriptionProvider(), typeof(ItemsControl));
        }

        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
        /// <param name="instance">An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"/>.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"/> that can provide metadata for the type.
        /// </returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);
            return instance == null ? defaultDescriptor : new ItemsControlCustomTypeDescriptor(defaultDescriptor);
        }
    }
}