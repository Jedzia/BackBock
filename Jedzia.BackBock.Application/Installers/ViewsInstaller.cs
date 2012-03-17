namespace Jedzia.BackBock.Application.Installers
{
    using System;
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Design;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel;
    using Castle.MicroKernel.Context;

    public class ViewsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
                /*SimpleIoc.Default.Register<ITaskService, Design.DesignTaskService>();
                SimpleIoc.Default.Register<IOService, Design.DesignIOService>();
                SimpleIoc.Default.Register<IDialogService, Design.DesignDialogService>();
                SimpleIoc.Default.Register<IMainWindow, Design.DesignMainWindow>();
                SimpleIoc.Default.Register<ApplicationViewModel>();*/

                //throw new System.NotImplementedException("Designmoode");
                //container.Register(Component.For<IOService>().ImplementedBy<Design.DesignIOService>());

                //SimpleIoc.Default.Register<ITaskService, Design.DesignTaskService>();
                //SimpleIoc.Default.Register<IOService, Design.DesignIOService>();
                //SimpleIoc.Default.Register<IDialogService, Design.DesignDialogService>();
                //SimpleIoc.Default.Register<IMainWindow, Design.DesignMainWindow>();

                //container.Register(Component.For<ITaskService>().ImplementedBy<DesignTaskService>());
                //container.Register(Component.For<IDialogService>().ImplementedBy<DesignDialogService>());
                //container.Register(Component.For<IMainWindow>().Instance(new DesignMainWindow()));
            }
            else
            {
                /*SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                SimpleIoc.Default.Register<ApplicationViewModel>();*/
                // SimpleIoc.Default.Register<IDataService, DataService>();
                //container.Register(Component.For<ITaskService>().ImplementedBy<DesignMainWindow>());
                //SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                //container.Register(Component.For<ITaskWizardProvider>().ImplementedBy<TaskWizardProvider>());
                container.Register(Component.For<IMainWindow>().Instance((IMainWindow)System.Windows.Application.Current.MainWindow));
            }
            
            //SimpleIoc.Default.Register<IMainWindow>(GetMainWindow);

            //container.Register(Component.For<IMainWindow>().UsingFactoryMethod((a, b) => { return (IMainWindow)System.Windows.Application.Current.MainWindow; }));
            //container.Register(Component.For<IMainWindow>().UsingFactoryMethod((a, b) => GetMainWindow(a, b)));
            
            container.Register(Component.For<IStateWizard>().ImplementedBy<TaskWizard>().LifestyleTransient());
            //SimpleIoc.Default.Register<IStateWizard, TaskWizard>(new TransitionLifetime());
            //container.Register(Component.For<IStateWizard>().LifestyleTransient());

            //SimpleIoc.Default.Register<ApplicationViewModel>();
            //SimpleIoc.Default.Register<MainWindowViewModel>();
            //SimpleIoc.Default.Register<TaskWizardViewModel>();

            //container.Register(AllTypes.FromThisAssembly().BasedOn<ViewModelBase>());
            //container.Register(AllTypes.FromThisAssembly().Pick()
            //    .If(Component.IsInSameNamespaceAs<FileIOService>()));
            //container.Register(AllTypes.FromThisAssembly().Pick()
            //                    .If(Component.IsInSameNamespaceAs<FormsAuthenticationService>())
            //                    .LifestyleTransient()
            //                    .WithService.DefaultInterfaces());
        }
        
        private static IMainWindow GetMainWindow(IKernel kernel, CreationContext cre)
        {
            return (IMainWindow)System.Windows.Application.Current.MainWindow;
        }
    }
}
