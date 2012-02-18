namespace Jedzia.BackBock.ViewModel.Serialization
{
    using System;
    using System.ComponentModel;

    public static class EditorHelper
    {
        public static void Register<T, TC>()
        {
            Attribute[] attr = new Attribute[1];
            TypeConverterAttribute vConv = new TypeConverterAttribute(typeof(TC));
            attr[0] = vConv;
            TypeDescriptor.AddAttributes(typeof(T), attr);
        }
    }
}