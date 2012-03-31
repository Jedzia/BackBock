// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignModeInstaller.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Application.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Jedzia.BackBock.ViewModel;

    /// <summary>
    /// Design-Time installer.
    /// </summary>
    public class DesignModeInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // var extraAssemblyName = "Jedzia.BackBock.Data.Xml";
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // container.Register(Component.For<IDialogService>().ImplementedBy<DesignDialogService>());
            }

            // .Install(container, store);

            // Wiring sequences, Dependency Injection in .NET, S.368

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

            // SimpleIoc.Default.Register<ApplicationViewModel>();
            // SimpleIoc.Default.Register<MainWindowViewModel>();
            // SimpleIoc.Default.Register<TaskWizardViewModel>();

            // container.Register(AllTypes.FromThisAssembly().BasedOn<ViewModelBase>());
            // container.Register(AllTypes.FromThisAssembly().Pick()
            // .If(Component.IsInSameNamespaceAs<FileIOService>()));
            // container.Register(AllTypes.FromThisAssembly().Pick()
            // .If(Component.IsInSameNamespaceAs<FormsAuthenticationService>())
            // .LifestyleTransient()
            // .WithService.DefaultInterfaces());
        }
    }
}