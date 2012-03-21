using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Jedzia.BackBock.Tasks;
using Jedzia.BackBock.ViewModel.Design;
using Jedzia.BackBock.ViewModel.Data;
using Jedzia.BackBock.Application.Editors.TaskWizard;
using Jedzia.BackBock.Application.Properties;

namespace Jedzia.BackBock.Application.Installers
{

    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                container.Register(Component.For<ITaskService>().ImplementedBy<DesignTaskService>());
                container.Register(Component.For<IOService>().ImplementedBy<DesignIOService>());

                container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.DesignBackupDataRepository>());
                container.Register(Component.For<ISettingsProvider>().ImplementedBy<DesignSettingsProvider>());
            }
            else
            {
                container.Register(Component.For<ITaskService>().UsingFactoryMethod((a, b) => TaskRegistry.GetInstance()));
                container.Register(Component.For<IOService>().ImplementedBy<FileIOService>());

                container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.DesignBackupDataRepository>());
                //container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.Data.Xml.XmlDataRepository>());

                container.Register(Component.For<Settings>().Instance(Settings.Default));
                container.Register(Component.For<ISettingsProvider>().ImplementedBy<SettingsProvider>());
            }

            container.Register(Component.For<IBackupDataService>().ImplementedBy<Jedzia.BackBock.ViewModel.Data.BackupDataService>());

        }
    }
}
