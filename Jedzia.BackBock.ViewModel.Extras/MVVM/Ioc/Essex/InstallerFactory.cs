namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

    public class InstallerFactory
    {
        // Methods
        public virtual IWindsorInstaller CreateInstance(Type installerType)
        {
            return installerType.CreateInstance<IWindsorInstaller>(new object[0]);
        }

        public virtual IEnumerable<Type> Select(IEnumerable<Type> installerTypes)
        {
            return installerTypes;
        }
    }
}