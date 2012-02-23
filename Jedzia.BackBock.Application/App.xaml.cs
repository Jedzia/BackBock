namespace Jedzia.BackBock.Application
{
    using System.Windows;
    using Jedzia.BackBock.ViewModel;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Test()
        {
            //this.key
        }

        private static ApplicationViewModel applicationViewModel;

        public static ApplicationViewModel ApplicationViewModel
        {
            get { return App.applicationViewModel; }
            //set { App.applicationViewModel = value; }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            applicationViewModel = new ApplicationViewModel(new FileIOService());
        }
    }
}
