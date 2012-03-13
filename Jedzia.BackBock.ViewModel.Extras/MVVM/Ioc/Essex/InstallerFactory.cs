namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

    public class InstallerFactory
    {
        // Methods
        public virtual IEssexInstaller CreateInstance(Type installerType)
        {
            return installerType.CreateInstance<IEssexInstaller>(new object[0]);
        }

        public virtual IEnumerable<Type> Select(IEnumerable<Type> installerTypes)
        {
            return installerTypes;
        }
    }
}