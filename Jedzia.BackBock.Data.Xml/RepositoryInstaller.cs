using Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex;
using Jedzia.BackBock.ViewModel.Data;
namespace Jedzia.BackBock.Data.Xml
{

    public class RepositoryInstaller : IEssexInstaller
    {
        public void Install(IEssexContainer container, IConfigurationStore store)
        {
            //    SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
            container.Register(Component.For<BackupDataRepository>().ImplementedBy<XmlDataRepository>().LifestyleTransient());
        }
    }
}
