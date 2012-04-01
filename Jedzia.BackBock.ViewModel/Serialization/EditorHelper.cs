namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Registers <see cref="TypeConverterAttribute"/>'s.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="T">Type that gets the attribute added.</typeparam>
        /// <typeparam name="TC">The type of the Attribute.</typeparam>
        public static void Register<T, TC>()
        {
            Attribute[] attr = new Attribute[1];
            TypeConverterAttribute vConv = new TypeConverterAttribute(typeof(TC));
            attr[0] = vConv;
            TypeDescriptor.AddAttributes(typeof(T), attr);
        }
    }
}