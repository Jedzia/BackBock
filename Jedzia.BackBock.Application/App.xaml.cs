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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //IEssexContainer container = new EssexContainer();
            //container.Install(FromAssembly.InThisEntry());
            
            //container.Install(FromAssembly.InThisApplication());
            //var sauce = container.Resolve<FileIOService>(); 
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // this.Resources["Locator"]
            ViewModelLocator.Cleanup();
        }

    }

}
