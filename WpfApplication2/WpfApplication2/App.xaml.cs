using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private bool _contentLoaded;
        [STAThread]
        public static void Main()
        {
            var application = new App();
            application.Run();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //var bs = new Bootstrapper();
            //bs.Run();

            //ViewModelLocator.Container.Resolve<Window1>().Show();
            ViewModelLocator.GetMainWindow().Show();
            //var wnd = ViewModelLocator.Container.Resolve<Window1>();
            //this.MainWindow = wnd;
            //wnd.BeginInit();
            //wnd.EndInit();
            //this.MainWindow = wnd;
            //wnd.Show();
            //Run(wnd);
            //var container = new WindsorContainer();
            //container.Install(FromAssembly.InThisApplication());

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message.ToString());
            //this.Shutdown(1);
        }

        protected virtual void OnExit(ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();
            base.OnExit(e);
        }
    }
}
