namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public static class Classes
    {
        public static FromAssemblyDescriptor FromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            return new FromAssemblyDescriptor(assembly, new Predicate<Type>(Classes.Filter));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool Filter(Type type)
        {
            return (type.IsClass && !type.IsAbstract);
        }


    }
}
