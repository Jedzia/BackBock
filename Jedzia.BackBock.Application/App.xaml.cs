namespace Jedzia.BackBock.Application
{
    using System.Windows;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.Tasks;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Test()
        {
            //this.key
        }

        /*private static ApplicationViewModel applicationViewModel;

        public static ApplicationViewModel ApplicationViewModel
        {
            get { return App.applicationViewModel; }
            //set { App.applicationViewModel = value; }
        }*/

        /* Todo: better use this
        /// <summary>Fired when the app starts</summary>
        /// <param name="e">Startup event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Jedzia.BackBock.ViewModel.MVVM.Threading.DispatcherHelper.Initialize();
        }

        /// <summary>Fired when the app closes</summary>
        /// <param name="e">Exit event arguments</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ViewModelLocator.Cleanup();
        }*/


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //applicationViewModel = new ApplicationViewModel(new FileIOService());
            //SimpleIoc.Default.Register<ApplicationViewModel>(CreateApplicationViewModel);

            //SimpleIoc.Default.Register<MainWindowViewModel>(CreateMainWindowViewModel);
            //SimpleIoc.Default.Register<IOService, FileIOService>();
            //SimpleIoc.Default.Register<IDialogService, DialogService>();
            //SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
            //SimpleIoc.Default.Register<IMainWindow>(GetMainWindow);

            //IEssexContainer container = new EssexContainer();
            //container.Install(FromAssembly.InThisEntry());
            
            //container.Install(FromAssembly.InThisApplication());
            //var sauce = container.Resolve<FileIOService>(); 
        }

        /*private ApplicationViewModel CreateApplicationViewModel()
        {
            return new ApplicationViewModel(new FileIOService(), (IMainWindow)this.MainWindow);
        }*/

        private IMainWindow GetMainWindow()
        {
            return (IMainWindow)this.MainWindow;
        }

        private IDialogService GetDialogService()
        {
            return (IDialogService)this.MainWindow;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // this.Resources["Locator"]
            ViewModelLocator.Cleanup();
        }

    }

}
