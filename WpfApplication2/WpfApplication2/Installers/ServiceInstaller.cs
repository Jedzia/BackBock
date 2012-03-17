
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Windows;
using GalaSoft.MvvmLight;

namespace WpfApplication2.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
                container.Register(Component.For<IDataProvider>().ImplementedBy<DesignDataProvider>());
            }
            else
            {
                // SimpleIoc.Default.Register<IDataService, DataService>();
                container.Register(Component.For<IDataProvider>().ImplementedBy<DataProvider>());
            }
            //container.Register(Component.For<Window1>());
            //container.Register(Component.For<Window>().ImplementedBy<Window1>().LifeStyle.Transient);
            container.Register(Component.For<IWindow>().ImplementedBy<Window1>());
            container.Register(Component.For<IDepp>().ImplementedBy<OlderDepp>());
            //container.Register(Component.For<MainViewModel>()
              //  .ServiceOverrides(new { handler = "handler" }));
            container.Register(Component.For<MainViewModel>());
            //container.Register(Component.For<MainWindowAdapter>());
            //container.Register(Component.For<IWindow>().ImplementedBy<SubWindow>());
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
