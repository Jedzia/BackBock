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
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor.Installer;
using Jedzia.BackBock.DataAccess;
using Jedzia.BackBock.ViewModel.MainWindow;
using Castle.DynamicProxy;
using Castle.Facilities.TypedFactory;
using Castle.Core.Configuration;

namespace Jedzia.BackBock.Application.Installers
{

    public class LoggerInterceptor : IInterceptor
    {
        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }

        #endregion
    }

    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                container.Register(Component.For<ITaskService>().ImplementedBy<DesignTaskService>());

                //container.Register(Component.For<TaskContext>().ImplementedBy<DesignTaskContext>());
                container.Register(AllTypes.FromAssemblyContaining<DesignTaskService>()
                    .InSameNamespaceAs<DesignTaskService>()
                    .If(fil => fil.Name == "DesignTaskContext")
                    .Configure(e =>
                    {
                        var bs = e.Implementation.BaseType;
                        e.Forward(bs);
                        e.Named("TaskContext");
                    })
                    );
                
                container.Register(Component.For<IOService>().ImplementedBy<DesignIOService>());

                //container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.DesignBackupDataRepository>());
                container.Register(Component.For<ISettingsProvider>().ImplementedBy<DesignSettingsProvider>());
                container.Register(Component.For<IBackupDataService>().ImplementedBy<ViewModel.Design.Data.DesignBackupDataService>());
            }
            else
            {
                container.Register(Component.For<ITaskService>()
                    .UsingFactoryMethod(
                    (a, b) =>
                    {
                        var taskService = TaskRegistry.GetInstance();
                        // taskService.Register( ... additional tasks)
                        return taskService;
                    })
                );

                //container.Register(Component.For<DesignTaskContext>()
                  //  .ImplementedBy<DesignTaskContext>().
                    //.WithServiceBase());

                
                //container.Register(Component.For<TaskContext>().ImplementedBy<ApplicationTaskContext>());
                container.Register(AllTypes.FromAssemblyNamed("Jedzia.BackBock.TaskContext")
                    .Where(type => type.Name == "ApplicationTaskContext")
                    .Configure(e =>
                    {
                        var bs = e.Implementation.BaseType;
                        e.Forward(bs);
                    })
                    );



                container.Register(Component.For<IOService>().ImplementedBy<FileIOService>());

                // Register collection resolver, needed by the BackupDataService dependencies.
                container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, false));
                //container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.DesignBackupDataRepository>());

                container.Register(Component.For<BackupDataRepository>().ImplementedBy<RepositoryLogger>()
                    //.ServiceOverrides(ServiceOverride.ForKey("logger"))
                    //.Named("logger")
                    );

                container.Register(AllTypes.FromAssemblyContaining<IBackupDataService>()
                    .BasedOn(typeof(BackupDataRepository)).WithServiceBase()
                    //.Configure(c => c.ServiceOverrides(new { First = "first" }))
                    //.Configure(c => c.Interceptors<LoggerInterceptor>())
                    );
                container.Register(AllTypes.FromAssemblyContaining<MainWindowViewModel>()
                    .BasedOn(typeof(BackupDataRepository)).WithServiceBase()
                    .If(t => t.Name.EndsWith("Repository"))
                    //.Configure(c => c.Interceptors<LoggerInterceptor>())
                    /*.Configure(c => c.Interceptors(typeof(RepositoryLogger)))*/);
                container.Register(AllTypes.FromAssemblyNamed("Jedzia.BackBock.Data.Xml")
                    .BasedOn(typeof(BackupDataRepository)).WithServiceBase()
                    //.Configure(c => c.ServiceOverrides(ServiceOverride.ForKey("logger")))
                    //.Configure(c => c.Interceptors<LoggerInterceptor>())
                    );



                //container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.TestBackupDataRepository>());
                //container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.Data.Xml.XmlDataRepository>());

                //container.Register(Component.For<IBackupDataService>().ImplementedBy<BackupDataService>());
                //container.Register(Component.For<IBackupDataService>().ImplementedBy<BackupDataService>()
                //    .Interceptors(typeof(BackupDataRepository), typeof(RepositoryLogger)));
                //container.Register(Component.For<IBackupDataService>().ImplementedBy<BackupDataService>()
                //     .Proxy.AdditionalInterfaces(typeof(BackupDataRepository))
                //    .Interceptors(typeof(RepositoryLogger)));

                //container.Register(Component.For<IBackupDataService>()
                //  .ServiceOverrides(new { repositories = "logger" })
                //.ImplementedBy<BackupDataService>()
                // );

                //container.Register(Component.For<LoggerInterceptor>());
                container.Register(Component
                    .For<IBackupDataService>()
                    .ImplementedBy<BackupDataService>()
                    //.Interceptors<LoggerInterceptor>()
                    );

                container.Register(Component.For<Settings>().Instance(Settings.Default));
                container.Register(Component.For<ISettingsProvider>().ImplementedBy<SettingsProvider>());
            }


                //container.Register(Component.For<ProxyDings>().Named("Heiner"));
                container.Register(
                    // Decorator
                    Component.For<IDings>().ImplementedBy<ProxyDings>().LifestyleTransient(),
                    // Decorated object.
                    Component.For<IDings>().ImplementedBy<Dings>()
                    );

                container.AddFacility<TypedFactoryFacility>();

                container.Register(Component
                    .For<IDingsFactory>()
                    .AsFactory()
                    .LifestyleTransient()
                    );


        }
    }
}
