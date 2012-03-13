namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System.Collections.Generic;

    public class CompositeInstaller : IWindsorInstaller
    {

        // Fields
        private readonly HashSet<IWindsorInstaller> installers = new HashSet<IWindsorInstaller>();

        // Methods
        public void Add(IWindsorInstaller instance)
        {
            this.installers.Add(instance);
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (IWindsorInstaller installer in this.installers)
            {
                installer.Install(container, store);
            }
        }
    }
}