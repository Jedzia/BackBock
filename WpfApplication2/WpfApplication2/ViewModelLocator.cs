/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:WpfApplication2"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace WpfApplication2
{
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
        static WindsorContainer container;

        public static WindsorContainer Container
        {
            get { return ViewModelLocator.container; }
        }
        
        static ViewModelLocator()
        {
            container = new WindsorContainer();
            container.Install(FromAssembly.InThisApplication());
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ViewModelBase Main
        {
            get
            {
                //ApplicationHost.Current.Container.Resolve<TViewModel>();
                return container.Resolve<MainViewModel>();

                //return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
            container.Dispose();
        }

        public static IWindow GetMainWindow()
        {
            return Container.Resolve<IWindow>();
        }
    }
}