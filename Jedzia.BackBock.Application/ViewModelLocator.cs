/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Jedzia.BackBock.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/


namespace Jedzia.BackBock.Application
{
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// Use the <strong>mvvmlocatorproperty</strong> snippet to add ViewModels
    /// to this locator.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
		//public ViewModelLocator()
        //{
		//	throw new NotImplementedException("Mooo");
		//}
		
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //IEssexContainer container = new EssexContainer();
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
				// For out DesignPeeps, hold a Blendable backforce here.
                //SimpleIoc.Default.Register<ITaskService, Design.DesignTaskService>();
                //SimpleIoc.Default.Register<IOService, Design.DesignIOService>();
                //SimpleIoc.Default.Register<IDialogService, Design.DesignDialogService>();
                //SimpleIoc.Default.Register<IMainWindow, Design.DesignMainWindow>();
                //return;
                //container.Install(FromAssembly.InThisApplication());
            }
            else
            {
            	//container.Install(FromAssembly.InThisEntry());
                //SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
                //SimpleIoc.Default.Register<TaskWizardViewModel>();
                // SimpleIoc.Default.Register<IDataService, DataService>();
            }

            //SimpleIoc.Default.Register<ApplicationViewModel>();
            //SimpleIoc.Default.Register<MainWindowViewModel>();
            //SimpleIoc.Default.Register<TaskWizardViewModel>();
            //MessageBox.Show("M00");
            //throw new NotImplementedException("m00");
//SimpleIoc.Default.Register<MainWindowViewModel>();
            //SimpleIoc.Default.Register<TaskWizardViewModel>();
            //SimpleIoc.Default.Register<TaskWizardViewModel>(new TransitionLifetime())/*.Release(null)*/;
            //SimpleIoc.Default.Register<TaskWizardViewModel>(new TransitionLifetime()).Release((o) => o.Cleanup());

            //var installer = FromAssembly.InThisEntry();
            //container.Install(installer);
            //installer.Install(cnt, null);
 //throw new NotImplementedException("Mooo");

        }

        /*private static ApplicationViewModel CreateApplicationViewModel()
        {
            //return new ApplicationViewModel(new FileIOService(), (IMainWindow)Application.Current.MainWindow);
            return new ApplicationViewModel(null, (IMainWindow)Application.Current.MainWindow);
        }
        private static ApplicationViewModel CreateApplicationViewModelDesign()
        {
            //return new ApplicationViewModel(new FileIOService(), (IMainWindow)Application.Current.MainWindow);
            return new ApplicationViewModel(new Design.DesignIOService(), null);
        }*/

        //private MainWindowViewModel CreateMainWindowViewModel()
        //{
            //return new MainWindowViewModel(App.ApplicationViewModel, this); 
        //}

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public static MainWindowViewModel MainStatic
        {
            get
            {
                if (_main == null)
                {
                    CreateMain();
                }

                return _main;
            }
        }
        /// <summary>
        /// Provides a deterministic way to create the Main property.
        /// </summary>
        public static void CreateMain()
        {
            if (_main == null)
            {
                //_main = new MainWindowViewModel();
                _main = ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        private static MainWindowViewModel _main;

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainWindowViewModel Main
        {
            get
            {
                return MainStatic;
            }
        }



        /// <summary>
        /// Gets the TaskWizard property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public TaskWizardViewModel TaskWizard
        {
            get
            {
                //var key = Guid.NewGuid().ToString();
                //return ServiceLocator.Current.GetInstance<TaskWizardViewModel>(key);
                return ServiceLocator.Current.GetInstance<TaskWizardViewModel>();
                //return new TaskWizardViewModel();
            }
        }



        /// <summary>
        /// Provides a deterministic way to delete the Main property.
        /// </summary>
        public static void ClearMain()
        {
            if (_main != null)
            {
                _main.Cleanup();
                _main = null;
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearMain();
        }


    }
}