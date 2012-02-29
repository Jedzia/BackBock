namespace Jedzia.BackBock.ViewModel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Xml;
    using Jedzia.BackBock.ViewModel.Commands;
    using System;
    using System.Linq;
    using Jedzia.BackBock.ViewModel.Util;
    using Jedzia.BackBock.ViewModel.MainWindow;

    public class ApplicationViewModel : /*IFolderExplorerViewModel,*/ INotifyPropertyChanged
    {
        //public enum ServiceTypes { TaskEditor,  }
        private static IOService ioService;
        private static IDialogService dialogService;

        public static IDialogService DialogService
        {
            get
            {
                if (dialogService == null)
                {
                    throw new ApplicationException("Fetching the DialogService before an ApplicationViewModel was instantiated.");
                }
                return dialogService;
            }
        }
        
        public static IOService MainIOService
        {
            get 
            {
                if (ioService == null)
                {
                    throw new ApplicationException("Fetching the MainIOService before an ApplicationViewModel was instantiated.");
                }
                return ioService; 
            }
            //set { ioService = value; }
        }

        private static object initialized = null;
        
        internal static void Reset()
        {
            // used in unit tests
            initialized = null;
            ioService = null;
            dialogService = null;
        }
        /// <summary>
        /// The summary.
        /// </summary>
        private IMainWindow mainWindow;

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public IMainWindow MainWindow
        {
            get
            {
                return this.mainWindow;
            }

            private set
            {
                this.mainWindow = value;
            }
        }
        public ApplicationViewModel(IOService ioService, IDialogService dialogService, IMainWindow mainWindow)
        {
            if (initialized != null)
            {
                throw new ApplicationException("Double initialization of the ApplicationViewModel.");
            }
            else
            {
                initialized = new object();
            }

            //Guard.NotNull(() => ioService, ioService);

            ApplicationViewModel.ioService = ioService;
            ApplicationViewModel.dialogService = dialogService;
            this.mainWindow = mainWindow;
            Tasks.TaskRegistry.GetInstance();
            //this.ApplicationCommands = new ApplicationCommandModel(this);

            /*this.parent = parent;
            //ClassData2 = ClassDataProvider.CreateSampleClassData();
            //ICollectionView view = CollectionViewSource.GetDefaultView(ClassData2);
            ICollectionView view = CollectionViewSource.GetDefaultView(parent.ClassRawData);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription(ClassDataItemViewModel.ItemTypeDescriptor));
            ClassData = view;
            this.ClassDataCommands = new ClassDataCommandModel(parent);*/
        }

        /*public ApplicationCommandModel ApplicationCommands
        {
            get;
            private set;
        }*/


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // Todo: move this to a little ioc class
        private static Dictionary<Enum, Type> registeredControlTypes = new Dictionary<Enum, Type>();
        public static void RegisterControl(Enum kind, Type type)
        {
            Guard.NotNull(() => kind, kind);
            Guard.NotNull(() => type, type);
            // Todo: move this stuff to a ControlRegistrator class.
            //classSpecificationWindowType = type;
            //var xxx = Data.BackupItemViewModel.WindowTypes.TaskEditor.GetType();
            //var yyy = xxx.GetCustomAttributes(false);
            var kindType = kind.GetType();
            var member = kindType.GetMembers().FirstOrDefault((e) => e.Name == kind.ToString());
            if (member != null)
            {
                var attrs = member.GetCustomAttributes(false);
                var ctattr = attrs.OfType<CheckTypeAttribute>();
                foreach (var item in ctattr)
                {
                    if (!type.IsSubclassOf(item.Type))
                    {
                        throw new NotSupportedException("Can't register type. The type "
                            + type.ToString() + " is no instance of " + item.Type.ToString());
                    }
                    /*if (!type.IsInstanceOfType(item.Type))
                    {
                        throw new NotSupportedException("Can't register type. The type "
                            + type.ToString() + " is no instance of " + item.Type.ToString());
                    }*/
                }
            }
            var values = Enum.GetValues(kindType);
            //var attrs = kindType.GetCustomAttributes(false);
            registeredControlTypes.Add(kind, type);
            //var w = CreateInstanceFromType<Window>(type);
        }

        public static T GetInstanceFromType<T>(Enum kind) where T : class
        {
            return GetInstanceFromType<T>(kind, null);
        }

        public static T GetInstanceFromType<T>(Enum kind, object[] parameters) where T : class
        {
            var t = registeredControlTypes[kind];
            var instance = CreateInstanceFromType<T>(t, parameters);
            return instance;
        }

        private static T CreateInstanceFromType<T>(Type type, object[] parameters) where T : class
        {
            Type[] types = new Type[0];
            if (parameters != null)
            {
                types = new Type[parameters.Length];

                for (int index = 0; index < parameters.Length; index++)
                {
                    types[index] = parameters[index].GetType();
                }
            }
            var cnstr = type.GetConstructor(types);
            var instance = cnstr.Invoke(parameters) as T;
            return instance;
        }

    }

    /// <summary>
    /// Summary
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CheckTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public Type Type
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CheckTypeAttribute"/> class. 
        /// </summary>
        public CheckTypeAttribute(Type type)
        {
            this.Type = type;
        }
    }

    /// <summary>
    /// RegisterControl contains 1 read-write and 0 read-only dependency properties.
    /// </summary>
    public class ControlRepository : DependencyObject
    {
        public ControlRepository()
        {

        }

        public static ControlDescriptor GetRegControl(DependencyObject obj)
        {
            return (ControlDescriptor)obj.GetValue(RegControlProperty);
        }

        public static void SetRegControl(DependencyObject obj, ControlDescriptor value)
        {
            obj.SetValue(RegControlProperty, value);
        }

        // Using a DependencyProperty as the backing store for RegControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegControlProperty =
            DependencyProperty.RegisterAttached("RegControl", typeof(ControlDescriptor), typeof(ControlRepository), new UIPropertyMetadata(null), CallBack);

        public static bool CallBack(object value)
        {
            if (value != null)
            {

            }
            return true;
        }

    }

    public class ControlDescriptor : DependencyObject
    {



        /*public DesignerCanvas.WindowTypes Identity
        {
            get { return (DesignerCanvas.WindowTypes)GetValue(IdentityProperty); }
            set { SetValue(IdentityProperty, value); }
        }*/

        // Using a DependencyProperty as the backing store for Identity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdentityProperty =
            DependencyProperty.Register("Identity", typeof(Enum), typeof(ControlDescriptor));



        public Type ControlType
        {
            get { return (Type)GetValue(ControlTypeProperty); }
            set { SetValue(ControlTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ControlType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlTypeProperty =
            DependencyProperty.Register("ControlType", typeof(Type), typeof(ControlDescriptor), new UIPropertyMetadata(null));



        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(ControlDescriptor), new UIPropertyMetadata(string.Empty));


        public ControlDescriptor()
        {

        }
    }


}