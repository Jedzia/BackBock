namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System.Reflection;

    public static class AllTypes
    {
        public static FromAssemblyDescriptor FromThisAssembly()
        {
            return Classes.FromAssembly(Assembly.GetCallingAssembly());
        }

        /// <summary>
        ///   Prepares to register types from an assembly.
        /// </summary>
        /// <param name = "assembly">The assembly.</param>
        /// <returns>The corresponding <see cref = "FromDescriptor" /></returns>
        public static FromAssemblyDescriptor FromAssembly(Assembly assembly)
        {
            return Classes.FromAssembly(assembly);
        }
    }
}
