namespace Jedzia.BackBock.Application.Installers
{
    using System;
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Design;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;
    using Jedzia.BackBock.ViewModel.Wizard;

    public class ViewsInstaller : IEssexInstaller
    {
        public void Install(IEssexContainer container, IConfigurationStore store)
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
            }
            else
            {
                /*SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                SimpleIoc.Default.Register<ApplicationViewModel>();*/
                // SimpleIoc.Default.Register<IDataService, DataService>();
                //container.Register(Component.For<ITaskService>().ImplementedBy<DesignMainWindow>());
                //SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                //container.Register(Component.For<ITaskWizardProvider>().ImplementedBy<TaskWizardProvider>());
            }
            // Todo: implement lifetime
            //container.Register(Component.For<IStateWizard>().ImplementedBy<TaskWizard>().Lifetime(dingens));
            SimpleIoc.Default.Register<IStateWizard, TaskWizard>(new TransitionLifetime());

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
    }
}
