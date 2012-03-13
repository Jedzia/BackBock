namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class AssemblyInstaller : IEssexInstaller
    {
        // Fields
        private readonly Assembly assembly;
        private readonly InstallerFactory factory;

        // Methods
        public AssemblyInstaller(Assembly assembly, InstallerFactory factory)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            this.assembly = assembly;
            this.factory = factory;
        }

        private IEnumerable<Type> FilterInstallerTypes(IEnumerable<Type> types)
        {
            return (from t in types
                    where ((t.IsClass && !t.IsAbstract) && !t.IsGenericTypeDefinition) && t.Is<IEssexInstaller>()
                    select t);
        }

        public void Install(IEssexContainer container, IConfigurationStore store)
        {
            IEnumerable<Type> enumerable = this.factory.Select(this.FilterInstallerTypes(this.assembly.GetExportedTypes()));
            if (enumerable != null)
            {
                foreach (Type type in enumerable)
                {
                    var ci = this.factory.CreateInstance(type);
                    ci.Install(container, store);
                    //this.factory.CreateInstance(type).Install(container, store);
                }
            }
        }
    }
}