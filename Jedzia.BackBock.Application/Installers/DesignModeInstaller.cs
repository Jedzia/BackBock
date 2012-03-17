namespace Jedzia.BackBock.Application.Installers
{
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Design;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using System.Windows;
    using Jedzia.BackBock.ViewModel.Data;
    using System.Reflection;
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Core.Internal;
    using Castle.MicroKernel.SubSystems.Configuration;

    public class DesignModeInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var extraAssemblyName = "Jedzia.BackBock.Data.Xml";
            if (ViewModelBase.IsInDesignModeStatic)
            {
                //container.Register(Component.For<IDialogService>().ImplementedBy<DesignDialogService>());
            }
            else
            {
            }
            //.Install(container, store);


            /*var eassembly = ReflectionUtil.GetAssemblyNamed(extraAssemblyName);
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
                    container.Register(
                            AllTypes
                            .FromAssembly(Assembly.LoadFile(assembly.Location))
                            .BasedOn<BackupDataRepository>().WithServiceBase()
                            );
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    //if (_log.IsErrorEnabled)
                    //_log.ErrorFormat(@"An error has occured while loading assembly{0}\n{1}", assembly.FullName, e);
                }
            }*/



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
