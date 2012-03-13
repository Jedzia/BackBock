namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System.Collections.Generic;

    public class CompositeInstaller : IEssexInstaller
    {

        // Fields
        private readonly HashSet<IEssexInstaller> installers = new HashSet<IEssexInstaller>();

        // Methods
        public void Add(IEssexInstaller instance)
        {
            this.installers.Add(instance);
        }

        public void Install(IEssexContainer container, IConfigurationStore store)
        {
            foreach (IEssexInstaller installer in this.installers)
            {
                installer.Install(container, store);
            }
        }
    }
}