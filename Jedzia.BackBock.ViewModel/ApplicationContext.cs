// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationViewModel.cs" company="EvePanix">
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
    using System.ComponentModel;
    using System.Windows;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Microsoft.Build.Utilities;
    using Jedzia.BackBock.ViewModel.Util;

    internal sealed class KDA : TypeDescriptionProvider
    {
    }

    public class ApplicationContext : /*IFolderExplorerViewModel,*/ INotifyPropertyChanged
    {
        // public enum ServiceTypes { TaskEditor,  }
        #region Fields

        private static object initialized;
        private readonly IOService ioService;
        private readonly ISettingsProvider settings;

        public ISettingsProvider Settings
        {
            get { return this.settings; }
        }

        private static ITaskService taskService;
        private static IViewProvider taskWizardProvider;

        #endregion

        #region Constructors
        static ApplicationContext()
        {
            //TypeDescriptor.AddProvider(xxx, typeof(TaskItem));
            var conv = TypeDescriptor.GetConverter(typeof(TaskItem));
        }
        
        //public ApplicationViewModel(IOService ioService)
        // refactor into servicefacade
        public ApplicationContext(IOService ioService,
            ISettingsProvider settings,
            /*IDialogService dialogService , */
            IMainWindow mainWindow,
            ITaskService taskService,
            IViewProvider taskWizardProvider
            )
        {
            if (initialized != null)
            {
                throw new ApplicationException("Double initialization of the ApplicationViewModel.");
            }
            else
            {
                initialized = new object();
            }

            Guard.NotNull(() => ioService, ioService);
            Guard.NotNull(() => settings, settings);
            Guard.NotNull(() => mainWindow, mainWindow);
            Guard.NotNull(() => taskService, taskService);
            Guard.NotNull(() => taskWizardProvider, taskWizardProvider);

            this.settings = settings;
            this.ioService = ioService;
            ApplicationContext.taskService = taskService;
            ApplicationContext.taskWizardProvider = taskWizardProvider;

            // ApplicationViewModel.dialogService = dialogService;
            this.MainWindow = mainWindow;
            //TaskRegistry.GetInstance();

            // this.ApplicationCommands = new ApplicationCommandModel(this);

            /*this.parent = parent;
            //ClassData2 = ClassDataProvider.CreateSampleClassData();
            //ICollectionView view = CollectionViewSource.GetDefaultView(ClassData2);
            ICollectionView view = CollectionViewSource.GetDefaultView(parent.ClassRawData);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription(ClassDataItemViewModel.ItemTypeDescriptor));
            ClassData = view;
            this.ClassDataCommands = new ClassDataCommandModel(parent);*/
            //taskWizardProvider.GetWizard();
        }

        #endregion

        /*public ApplicationCommandModel ApplicationCommands
        {
            get;
            private set;
        }*/
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public IOService MainIOService
        {
            get
            {
                if (ioService == null)
                {
                    throw new ApplicationException(
                        "Fetching the MainIOService before an ApplicationViewModel was instantiated.");
                }

                return ioService;
            }

            // set { ioService = value; }
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public IMainWindow MainWindow { get; private set; }

        public static ITaskService TaskService
        {
            get
            {
                return taskService;
            }
        }

        public static IViewProvider TaskWizardProvider
        {
            get
            {
                //var key = Guid.NewGuid().ToString();
                //return ServiceLocator.Current.GetInstance<TaskWizardViewModel>(key);
                return taskWizardProvider;
                //return new TaskWizardViewModel();
            }
        }


        #endregion

        internal static void Reset()
        {
            // used in unit tests
            initialized = null;
            //ioService = null;

            // dialogService = null;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// RegisterControl contains 1 read-write and 0 read-only dependency properties.
    /// </summary>
    public class ControlRepository : DependencyObject
    {
        // Using a DependencyProperty as the backing store for RegControl.  This enables animation, styling, binding, etc...
        #region Fields

        public static readonly DependencyProperty RegControlProperty =
            DependencyProperty.RegisterAttached(
                "RegControl",
                typeof(ControlDescriptor),
                typeof(ControlRepository),
                new UIPropertyMetadata(null),
                CallBack);

        #endregion

        public static bool CallBack(object value)
        {
            if (value != null)
            {
            }

            return true;
        }

        public static ControlDescriptor GetRegControl(DependencyObject obj)
        {
            return (ControlDescriptor)obj.GetValue(RegControlProperty);
        }

        public static void SetRegControl(DependencyObject obj, ControlDescriptor value)
        {
            obj.SetValue(RegControlProperty, value);
        }
    }

    public class ControlDescriptor : DependencyObject
    {
        /*public DesignerCanvas.WindowTypes Identity
        {
            get { return (DesignerCanvas.WindowTypes)GetValue(IdentityProperty); }
            set { SetValue(IdentityProperty, value); }
        }*/
        #region Fields

        public static readonly DependencyProperty ControlTypeProperty =
            DependencyProperty.Register(
                "ControlType", typeof(Type), typeof(ControlDescriptor), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IdentityProperty =
            DependencyProperty.Register("Identity", typeof(Enum), typeof(ControlDescriptor));

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                "Name", typeof(string), typeof(ControlDescriptor), new UIPropertyMetadata(string.Empty));

        #endregion

        #region Properties

        public Type ControlType
        {
            get
            {
                return (Type)GetValue(ControlTypeProperty);
            }

            set
            {
                SetValue(ControlTypeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ControlType.  This enables animation, styling, binding, etc...

        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }

            set
            {
                SetValue(NameProperty, value);
            }
        }

        #endregion

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
    }
}