namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System.Reflection;

    public static class AllTypes
    {
        public static FromAssemblyDescriptor FromThisAssembly()
        {
            return Classes.FromAssembly(Assembly.GetCallingAssembly());
        }
    }
}
