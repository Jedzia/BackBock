using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;

namespace Jedzia.BackBock.Application.Installers
{

    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<FileIOService>());
            container.Register(Component.For<IOService>().ImplementedBy<FileIOService>());
            //container.Register(AllTypes.FromThisAssembly().BasedOn<ViewModelBase>());
            //container.Register(AllTypes.FromThisAssembly().Pick()
            //    .If(Component.IsInSameNamespaceAs<FileIOService>()));
            //container.Register(AllTypes.FromThisAssembly().Pick()
            //                    .If(Component.IsInSameNamespaceAs<FormsAuthenticationService>())
            //                    .LifestyleTransient()
            //                    .WithService.DefaultInterfaces());
        }
    }
}
