namespace Jedzia.BackBock.Application.Installers
{
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Design;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex;
    using System.Windows;
    using Jedzia.BackBock.ViewModel.Data;
    using System.Reflection;
    using System;

    public class DesignModeInstaller : IEssexInstaller
    {
        public void Install(IEssexContainer container, IConfigurationStore store)
        {
            var extraAssemblyName = "Jedzia.BackBock.Data.Xml";
            //MessageBox.Show("M00");
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
                container.Register(Component.For<ITaskService>().ImplementedBy<DesignTaskService>());
                container.Register(Component.For<IOService>().ImplementedBy<DesignIOService>());
                container.Register(Component.For<IDialogService>().ImplementedBy<DesignDialogService>());
                container.Register(Component.For<IMainWindow>().ImplementedBy<DesignMainWindow>());
                container.Register(Component.For<ITaskWizardProvider>().ImplementedBy<DesignTaskWizardProvider>());

                container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.DesignBackupDataRepository>().LifestyleTransient());
            }
            else
            {
                /*SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                SimpleIoc.Default.Register<ApplicationViewModel>();*/
                // SimpleIoc.Default.Register<IDataService, DataService>();
                //container.Register(Component.For<ITaskService>().ImplementedBy<DesignMainWindow>());
                SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                container.Register(Component.For<ITaskWizardProvider>().ImplementedBy<TaskWizardProvider>());

                //container.Install(FromAssembly.Named(extraAssemblyName));
                container.Register(Component.For<BackupDataRepository>().ImplementedBy<Jedzia.BackBock.ViewModel.Design.Data.DesignBackupDataRepository>().LifestyleTransient());
            }
            //.Install(container, store);


            var eassembly = ReflectionUtil.GetAssemblyNamed(extraAssemblyName);
            var assemblies = new[] { eassembly };
            foreach (var assembly in assemblies)
            {
                try
                {
                    container.Register(
                                            AllTypes
                                            .FromAssembly(Assembly.LoadFile(assembly.Location))
                                            .BasedOn<IDisposable>()
                                            );
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    //if (_log.IsErrorEnabled)
                    //_log.ErrorFormat(@"An error has occured while loading assembly{0}\n{1}", assembly.FullName, e);
                }
            }


            container.Register(Component.For<IBackupDataService>().ImplementedBy<Jedzia.BackBock.ViewModel.Data.BackupDataService>());

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
