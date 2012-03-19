// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.ViewModel.Data
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Xml;
    using System.Xml.Serialization;
    using Jedzia.BackBock.Model.Data;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel.Commands;
    using Jedzia.BackBock.ViewModel.Tasks;
    using Microsoft.Build.Framework;

    public partial class BackupItemViewModel : ILogger
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BackupItemViewModel"/> class.
        /// </summary>
        public BackupItemViewModel()
        {
            this.data = new BackupItemType();
        }

        #endregion

        /*private void LogMessageEvent(BuildMessageEventArgs e)
        {
            MessengerInstance.Send(e);
        }*/

        #region Properties

        public Type NumerableType
        {
            get
            {
                //return typeof(PathViewModel);
                return null;
            }
        }

        #endregion

        #region EditCollection Command

        #region Fields

        private RelayCommand editCollectionCommand;

        #endregion

        #region Properties

        public ICommand EditCollectionCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.editCollectionCommand == null)
                {
                    this.editCollectionCommand = new RelayCommand(
                        this.EditCollectionExecuted, this.EditCollectionEnabled);
                }

                return this.editCollectionCommand;
            }
        }

        #endregion

        private bool EditCollectionEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }

        private void EditCollectionExecuted(object o)
        {
            //this.EditCollection();
            //MessageBox.Show("Edit Collection");
            // Start collection editor this.Paths.
            /*PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
            var col = new WPG.Themes.TypeEditors.CollectionEditorControl();
            col.MyProperty = new WPG.Data.Property(this, properties["Paths"]);
            col.NumerableType = typeof(PathViewModel);
            col.NumerableValue = this.Paths;
            var pg = new WPG.TypeEditors.CollectionEditorWindow(col);
            pg.ShowDialog();*/
        }

        #endregion

        #region TaskDataClicked Command

        #region Fields

        private RelayCommand taskDataClickedCommand;

        #endregion

        #region Properties

        public ICommand TaskDataClickedCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.taskDataClickedCommand == null)
                {
                    this.taskDataClickedCommand = new RelayCommand(
                        this.TaskDataClickedExecuted, this.TaskDataClickedEnabled);
                }

                return this.taskDataClickedCommand;
            }
        }

        #endregion

        private static void SerializeTest(ITask task)
        {
            var xaml = XamlWriter.Save(task);
            var doc = new XmlDocument();
            doc.LoadXml(xaml);
            doc.Normalize();

            TextWriter wr = new StringWriter();
            doc.Save(wr);
            var str = wr.ToString();

            XmlSerializer ser = new XmlSerializer(task.GetType());
            TextWriter wrx = new StringWriter();
            ser.Serialize(wrx, task);
            var strx = wrx.ToString();
        }

        private bool TaskDataClickedEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }

        private ITaskService taskProvider;

        public ITaskService TaskProvider
        {
            get 
            {
                if (taskProvider == null)
                {
                    try
                    {
                        taskProvider = ApplicationViewModel.TaskService;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("TaskProvider fucking: " + ex.ToString());
                    }
                }
                return taskProvider; 
            }

            internal set 
            {
                taskProvider = value; 
            }
        }
        //private ITaskService taskProvider = TaskRegistry.GetInstance();
        
        private void TaskDataClickedExecuted(object o)
        {
            using (var tse = new TaskSetupEngine(this.TaskProvider, this, this.Paths))
            {
                var task = tse.InitTaskEditor(this.Task.TypeName, this.task.data.AnyAttr);
                //var task = InitTaskEditor(this.taskProvider);
                this.Task.TaskInstance = task;

                // Todo: Service Locator anti pattern!
                //var wnd = ControlRegistrator.GetInstanceOfType<Window>(WindowTypes.TaskEditor);
                var wnd = ApplicationViewModel.TaskWizardProvider.GetTaskEditor();
                wnd.DataContext = this;
                //wnd.DataContext = task;
                //this.Task.PropertyChanged += Task_PropertyChanged;
                var result = wnd.ShowDialog();
                //this.Task.PropertyChanged -= Task_PropertyChanged;
                tse.AfterTask(task, this.task.data.AnyAttr);
            }

        }


        #endregion

        #region RunTask Command

        #region Fields

        private RelayCommand runTaskCommand;
        private bool EnableLogging = true;

        #endregion

        #region Properties

        public ICommand RunTaskCommand
        {
            get
            {
                if (this.runTaskCommand == null)
                {
                    this.runTaskCommand = new RelayCommand(this.RunTaskExecuted, this.RunTaskEnabled);
                }

                return this.runTaskCommand;
            }
        }

        #endregion

        public void RunTask()
        {
            if (!this.IsEnabled)
            {
                return;
            }
            
            try
            {
                using (var tse = new TaskSetupEngine(this.TaskProvider, this, this.Paths, this.Task))
                {
                    var success = tse.ExecuteTask(this.Task.TypeName, this.task.data.AnyAttr);
                    MessengerInstance.Send("Finished Task: " + success);
                }
            }
            catch (Exception e)
            {
                MessengerInstance.Send("Exception: " + e);
            }
        }

        private bool RunTaskEnabled(object sender)
        {
            bool canExecute = this.IsEnabled;
            return canExecute;
        }

        private void RunTaskExecuted(object o)
        {
            var taskTypeName = this.Task.TypeName;
            var msg = "Running " + taskTypeName + "-Task: '" + this.ItemName + "'";
            //this.MessengerInstance.Send(msg);
            //MessengerInstance.Send(
            //    new DialogMessage(this, msg, null) { Caption = "Executing Task" }
            //   );
            MessengerInstance.Send("Executing Task" + msg);
            //ApplicationViewModel..DialogService.ShowMessage(msg, "Executing Task", "Ok", null);
            this.RunTask();
        }

        #endregion

        partial void PathCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Mit ObservableCollection kann das ViewModel automatisch auf entfernen und
            // hinzufügen von Objekten reagieren.

            /*if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (PathViewModel item in e.NewItems)
                {
                    backupitem.Path.Add(item.path);
                }
            }*/
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (PathViewModel item in e.NewItems)
                    {
                        this.data.Path.Add(item.data);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (PathViewModel item in e.OldItems)
                    {
                        this.data.Path.Remove(item.data);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initializes the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        public void Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += Log;
            eventSource.WarningRaised += Log;
            eventSource.ErrorRaised += Log;
        }

        private void Log(object sender, BuildErrorEventArgs e)
        {
            if (this.EnableLogging)
                MessengerInstance.Send(e);
        }

        private void Log(object sender, BuildWarningEventArgs e)
        {
            if (this.EnableLogging)
                MessengerInstance.Send(e);
        }

        private void Log(object sender, BuildMessageEventArgs e)
        {
            if (this.EnableLogging)
                MessengerInstance.Send(e);
        }

        public void Shutdown()
        {
        }

        public string Parameters
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public LoggerVerbosity Verbosity
        {
            get
            {
                return LoggerVerbosity.Detailed;
            }
            set
            {
            }
        }
    }
}