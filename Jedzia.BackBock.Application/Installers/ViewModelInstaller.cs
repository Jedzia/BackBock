namespace Jedzia.BackBock.Application.Installers
{
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.MicroKernel.SubSystems.Configuration;
    using System.Windows;
    using Castle.MicroKernel;

    public class ViewModelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component.For<ApplicationContext>());
            container.Register(Component.For<MainWindowViewModel>());

            container.Register(Component.For<TaskWizardViewModel>());

            /*container.Register(Component.For<TaskWizardViewModel>()
                .LifestyleTransient()
                .OnCreate(OnCreateTaskWizardViewModel)
                //.OnDestroy((o) => MessageBox.Show(o.ToString() + " Destroyed."))
                );*/

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

        private void OnCreateTaskWizardViewModel(IKernel k, TaskWizardViewModel o)
        {
            o.Closed += (s, e) =>
            {
                k.ReleaseComponent(o);
            };
        }
    }
}
