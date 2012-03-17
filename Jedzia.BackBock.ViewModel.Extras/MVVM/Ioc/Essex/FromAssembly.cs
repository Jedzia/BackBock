namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System.Collections.Generic;
    using System.Reflection;
    using System;

    //[Serializable, DebuggerTypeProxy(typeof(KernelDebuggerProxy)), DebuggerDisplay("{name,nq}")]


    public class FromAssembly
    {
        /// <summary>
        ///   Scans the assembly containing specified type for types implementing <see cref = "IWindsorInstaller" />, instantiates them and returns so that <see
        ///    cref = "IWindsorContainer.Install" /> can install them.
        /// </summary>
        /// <returns></returns>
        public static IEssexInstaller Containing(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            var assembly = type.Assembly;
            return Instance(assembly);
        }
        
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

        /// <summary>
        ///   Scans the assembly with specified name for types implementing <see cref = "IWindsorInstaller" />, instantiates them and returns so that <see
        ///    cref = "IWindsorContainer.Install" /> can install them.
        /// </summary>
        /// <returns></returns>
        public static IEssexInstaller Named(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            return Instance(assembly);
        }

        /// <summary>
        ///   Scans the assembly with specified name for types implementing <see cref = "IWindsorInstaller" />, instantiates using given <see
        ///    cref = "InstallerFactory" /> and returns so that <see cref = "IWindsorContainer.Install" /> can install them.
        /// </summary>
        /// <returns></returns>
        public static IEssexInstaller Named(string assemblyName, InstallerFactory installerFactory)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            return Instance(assembly, installerFactory);
        }




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

        /// <summary>
        ///   Scans assembly that contains code calling this method for types implementing <see cref = "IWindsorInstaller" />, 
        ///   instantiates them and returns so that <see cref = "IWindsorContainer.Install" /> can install them.
        /// </summary>
        /// <returns></returns>
        public static IEssexInstaller This()
        {
            return Instance(Assembly.GetCallingAssembly());
        }
        
        /// <summary>
        ///   Scans the specified assembly with specified name for types implementing <see cref = "IWindsorInstaller" />, instantiates them and returns so that <see
        ///    cref = "IWindsorContainer.Install" /> can install them.
        /// </summary>
        /// <returns></returns>
        public static IEssexInstaller Instance(Assembly assembly)
        {
            return Instance(assembly, new InstallerFactory());
        }

        /// <summary>
        ///   Scans the specified assembly with specified name for types implementing <see cref = "IWindsorInstaller" />, instantiates using given <see
        ///    cref = "InstallerFactory" /> and returns so that <see cref = "IWindsorContainer.Install" /> can install them.
        /// </summary>
        /// <returns></returns>
        public static IEssexInstaller Instance(Assembly assembly, InstallerFactory installerFactory)
        {
            return new AssemblyInstaller(assembly, installerFactory);
        }
    }
}
