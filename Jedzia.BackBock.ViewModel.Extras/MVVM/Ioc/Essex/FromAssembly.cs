namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System.Collections.Generic;
    using System.Reflection;

    //[Serializable, DebuggerTypeProxy(typeof(KernelDebuggerProxy)), DebuggerDisplay("{name,nq}")]


    public class FromAssembly
    {
        public static IEssexInstaller InThisApplication()
        {
            //System.Windows.MessageBox.Show(Assembly.GetCallingAssembly().ToString());
            //throw new NotImplementedException("FUCK");
            //throw new NotImplementedException(Assembly.GetCallingAssembly().ToString());
            return ApplicationAssemblies(Assembly.GetCallingAssembly(), new InstallerFactory());
        }
        
        public static IEssexInstaller InThisEntry()
        {
			//throw new NotImplementedException(Assembly.GetEntryAssembly().ToString());
            return ApplicationAssemblies(Assembly.GetEntryAssembly(), new InstallerFactory());
        }

        public static IEssexInstaller InThisApplication(InstallerFactory installerFactory)
        {
       			//throw new NotImplementedException(Assembly.GetCallingAssembly().ToString());
     return ApplicationAssemblies(Assembly.GetCallingAssembly(), installerFactory);
        }

        //... 



        private static IEssexInstaller ApplicationAssemblies(Assembly rootAssembly, InstallerFactory installerFactory)
        {
            HashSet<Assembly> set = new HashSet<Assembly>(ReflectionUtil.GetApplicationAssemblies(rootAssembly));
            CompositeInstaller installer = new CompositeInstaller();
            foreach (Assembly assembly in set)
            {
                if (assembly == typeof(FromAssembly).Assembly)
                {
                    continue;
                }
                installer.Add(Instance(assembly, installerFactory));
            }
            return installer;
        }

        public static IEssexInstaller Instance(Assembly assembly, InstallerFactory installerFactory)
        {
            return new AssemblyInstaller(assembly, installerFactory);
        }
    }
}
