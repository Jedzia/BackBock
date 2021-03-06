﻿/*
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
    //using Jedzia.BackBock.Model;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.Wizard;
    using System.Windows;
    using System;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Jedzia.BackBock.DataAccess;

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
        private static IWindsorContainer container;

        static ViewModelLocator()
        {

            try
            {
                container = new WindsorContainer();
                //container.Install(FromAssembly.InThisEntry());
                container.Install(FromAssembly.Containing<ViewModelLocator>());
                //container.Install(FromAssembly.InThisApplication());
                //MessageBox.Show("Moo");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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
				try
				{
					var bla = container.ResolveAll<BackupDataRepository>();
					var blax = container.Resolve<IDings>();
					var factory = container.Resolve<IDingsFactory>();
					var dings = factory.Create();
	
					//TaskContext.Default = container.Resolve<ITaskService>();
					//var tskcontext = container.Resolve<TaskContext>("TaskContext");
	
					//TaskContext.Default = container.Resolve<TaskContext>();
					_main = container.Resolve<MainWindowViewModel>();
				}
				catch(Exception e)
				{
					if(MainWindowViewModel.IsInDesignModeStatic)
					{
						var msg = e.Message  + Environment.NewLine;
						//msg += e.StackTrace + Environment.NewLine;
						var inner = e.InnerException;
						if(inner != null)
							msg += inner.Message + Environment.NewLine;
						inner = inner.InnerException ;
						if(inner != null)
						{
							msg += inner.Message + Environment.NewLine;
							msg += inner.StackTrace + Environment.NewLine;
						}
						/*inner = inner.InnerException;
						if(inner != null)
							msg += inner.Message + Environment.NewLine;
						inner = inner.InnerException;
						if(inner != null)
							msg += inner.Message + Environment.NewLine;
						inner = inner.InnerException;
						if(inner != null)
							msg += inner.Message + Environment.NewLine;*/
						
						MessageBox.Show(msg);
					}
					else
						throw;
				}
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
                return container.Resolve<TaskWizardViewModel>();
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
            container.Dispose();
        }


    }
}