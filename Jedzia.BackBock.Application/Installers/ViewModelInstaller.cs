﻿namespace Jedzia.BackBock.Application.Installers
{
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.MicroKernel.SubSystems.Configuration;

    public class ViewModelInstaller : IWindsorInstaller
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

            }
            else
            {
                /*SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                SimpleIoc.Default.Register<ApplicationViewModel>();*/
                // SimpleIoc.Default.Register<IDataService, DataService>();
            }

            //SimpleIoc.Default.Register<ApplicationViewModel>();
            //SimpleIoc.Default.Register<MainWindowViewModel>();
            //SimpleIoc.Default.Register<TaskWizardViewModel>();

            container.Register(Component.For<ApplicationViewModel>());
            container.Register(Component.For<MainWindowViewModel>());
            container.Register(Component.For<TaskWizardViewModel>());
            //container.Register(Component.For<TaskWizardViewModel>().LifestyleTransient());
            //SimpleIoc.Default.Register<TaskWizardViewModel>(new TransitionLifetime());
            
            /*SimpleIoc.Default.RegisterWithLifetime(new TransitionLifetime(),() =>
            {
                var taskWizardViewModel = new TaskWizardViewModel();
                return taskWizardViewModel;
            })
            .WireRelease((a, b) => a.Closed += b)
            .OnDestroy(model => SimpleIoc.Default.Unregister(model));*/

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
