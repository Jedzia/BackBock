namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
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
