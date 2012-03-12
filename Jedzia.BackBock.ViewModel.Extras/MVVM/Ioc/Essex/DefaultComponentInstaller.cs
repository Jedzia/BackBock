using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    public interface IComponentsInstaller
    {
        // Methods
        void SetUp(IWindsorContainer container, IConfigurationStore store);
    }

    public class DefaultComponentInstaller : IComponentsInstaller
    {
        public void SetUp(IWindsorContainer container, IConfigurationStore store)
        {
            //IConversionManager subSystem = container.Kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey) as IConversionManager;
            //this.SetUpInstallers(store.GetInstallers(), container, subSystem);
            //this.SetUpFacilities(store.GetFacilities(), container, subSystem);
            //this.SetUpComponents(store.GetComponents(), container, subSystem);
            //SetUpChildContainers(store.GetConfigurationForChildContainers(), container);
        }

    }
}
