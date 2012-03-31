// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationContext.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System;
    using Jedzia.BackBock.Model;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.Util;

    /// <summary>
    /// Main application context.
    /// </summary>
    public class ApplicationContext
    {
        // : INotifyPropertyChanged
        // public enum ServiceTypes { TaskEditor,  }
        #region Fields

        private static object initialized;

        private static TaskContext taskContext;
        private static IViewProvider taskWizardProvider;
        private readonly IOService inoutService;
        private readonly ISettingsProvider settings;

        #endregion

        // public ApplicationViewModel(IOService inoutService)
        // refactor into servicefacade
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="inoutService">The IO-Service used in this application.</param>
        /// <param name="settings">The settings for this application.</param>
        /// <param name="mainWindow">The main window.</param>
        /// <param name="taskContext">The task service.</param>
        /// <param name="taskWizardProvider">The task wizard provider.</param>
        /// <exception cref="ApplicationException">Double initialization of the ApplicationViewModel.</exception>
        public ApplicationContext(
            IOService inoutService,
            ISettingsProvider settings,
            /*IDialogService dialogService , */
            IMainWindow mainWindow,
            TaskContext taskContext,
            IViewProvider taskWizardProvider)
        {
            if (initialized != null)
            {
                throw new ApplicationException("Double initialization of the ApplicationViewModel.");
            }

            initialized = new object();

            Guard.NotNull(() => inoutService, inoutService);
            Guard.NotNull(() => settings, settings);
            Guard.NotNull(() => mainWindow, mainWindow);
            Guard.NotNull(() => taskContext, taskContext);
            Guard.NotNull(() => taskWizardProvider, taskWizardProvider);

            this.settings = settings;
            this.inoutService = inoutService;
            
            ApplicationContext.taskContext = taskContext;
            TaskContext.Default = taskContext;

            ApplicationContext.taskWizardProvider = taskWizardProvider;

            // ApplicationViewModel.dialogService = dialogService;
            this.MainWindow = mainWindow;

            // TaskRegistry.GetInstance();

            // this.ApplicationCommands = new ApplicationCommandModel(this);

            /*this.parent = parent;
            //ClassData2 = ClassDataProvider.CreateSampleClassData();
            //ICollectionView view = CollectionViewSource.GetDefaultView(ClassData2);
            ICollectionView view = CollectionViewSource.GetDefaultView(parent.ClassRawData);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription(ClassDataItemViewModel.ItemTypeDescriptor));
            ClassData = view;
            this.ClassDataCommands = new ClassDataCommandModel(parent);*/

            // taskWizardProvider.GetWizard();
        }

        #endregion

        /*public ApplicationCommandModel ApplicationCommands
        {
            get;
            private set;
        }*/
        #region Properties

        /// <summary>
        /// Gets the main IO service.
        /// </summary>
        /// <exception cref="ApplicationException">Fetching the MainIOService before an ApplicationViewModel was instantiated.</exception>
        public IOService MainIOService
        {
            get
            {
                if (this.inoutService == null)
                {
                    throw new ApplicationException(
                        "Fetching the MainIOService before an ApplicationViewModel was instantiated.");
                }

                return this.inoutService;
            }

            // set { inoutService = value; }
        }

        /// <summary>
        /// Gets the main window.
        /// </summary>
        public IMainWindow MainWindow { get; private set; }

        /// <summary>
        /// Gets the task wizard provider.
        /// </summary>
        public static IViewProvider TaskWizardProvider
        {
            get
            {
                // var key = Guid.NewGuid().ToString();
                // return ServiceLocator.Current.GetInstance<TaskWizardViewModel>(key);
                return taskWizardProvider;

                // return new TaskWizardViewModel();
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        internal ISettingsProvider Settings
        {
            get
            {
                return this.settings;
            }
        }

        /// <summary>
        /// Gets the task service.
        /// </summary>
        internal static TaskContext TaskContext
        {
            get
            {
                return taskContext;
            }
        }

        #endregion

        /// <summary>
        /// Resets this instance for test purposes.
        /// </summary>
        internal static void Reset()
        {
            // used in unit tests
            initialized = null;

            // inoutService = null;

            // dialogService = null;
        }
    }
}