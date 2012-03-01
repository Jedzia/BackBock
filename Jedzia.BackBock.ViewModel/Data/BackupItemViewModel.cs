using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Jedzia.BackBock.ViewModel.MVVM.Ioc;
using Jedzia.BackBock.Tasks;
using Jedzia.BackBock.ViewModel.Serialization;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Jedzia.BackBock.ViewModel.Data
{
    public partial class BackupItemViewModel
    {
        public enum WindowTypes
        {
            [CheckType(typeof(Window))]
            TaskEditor,
            ClassFieldOptPage,
            ClassMethodOptPage,
            ClassPropertyOptPage,
            ClassEventOptPage,
            SettingsPage,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BackupItemViewModel"/> class.
        /// </summary>
        public BackupItemViewModel()
        {
            this.data = new Jedzia.BackBock.Model.Data.BackupItemType();
        }

        partial void PathCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (PathViewModel item in e.NewItems)
                    {
                        data.Path.Add(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (PathViewModel item in e.OldItems)
                    {
                        data.Path.Remove(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public Type NumerableType
        {
            get
            {
                //return typeof(PathViewModel);
                return null;
            }
        }

        #region EditCollection Command

        private RelayCommand editCollectionCommand;

        public ICommand EditCollectionCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.editCollectionCommand == null)
                {
                    this.editCollectionCommand = new RelayCommand(this.EditCollectionExecuted, this.EditCollectionEnabled);
                }

                return this.editCollectionCommand;
            }
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

        private bool EditCollectionEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        /*#region AddType Command

        private RelayCommand addTypeCommand;

        public ICommand AddTypeCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.addTypeCommand == null)
                {
                    this.addTypeCommand = new RelayCommand(this.AddTypeExecuted, this.AddTypeEnabled);
                }

                return this.addTypeCommand;
            }
        }


        private void AddTypeExecuted(object o)
        {
            //this.Paths.Add((PathViewModel)o);
            //this.AddType();
        }

        private bool AddTypeEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        #region RemoveType Command

        private RelayCommand removeTypeCommand;

        public ICommand RemoveTypeCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.removeTypeCommand == null)
                {
                    this.removeTypeCommand = new RelayCommand(this.RemoveTypeExecuted, this.RemoveTypeEnabled);
                }

                return this.removeTypeCommand;
            }
        }


        private void RemoveTypeExecuted(object o)
        {
            //this.Paths.Remove((PathViewModel)o);
        }

        private bool RemoveTypeEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion*/

        #region TaskDataClicked Command

        private RelayCommand taskDataClickedCommand;

        public ICommand TaskDataClickedCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.taskDataClickedCommand == null)
                {
                    this.taskDataClickedCommand = new RelayCommand(this.TaskDataClickedExecuted, this.TaskDataClickedEnabled);
                }

                return this.taskDataClickedCommand;
            }
        }


        private void TaskDataClickedExecuted(object o)
        {
            var wnd = ApplicationViewModel.GetInstanceFromType<Window>(WindowTypes.TaskEditor);
            var taskService = SimpleIoc.Default.GetInstance<ITaskService>();
            var task = taskService[this.Task.TypeName];

            //wnd.DataContext = this.Task;
            //var str = XamlSerializer.Save(task);
            //SerializeTest(task);

            wnd.DataContext = task;
            wnd.ShowDialog();
        }

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
        #endregion

        #region RunTask Command

        private RelayCommand runTaskCommand;

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


        private void RunTaskExecuted(object o)
        {
            var taskTypeName = this.Task.TypeName;
            var msg = "Running " + taskTypeName + "-Task: '" + this.ItemName + "'";
            //this.MessengerInstance.Send(msg);
            this.MessengerInstance.Send(
                new MVVM.Messaging.DialogMessage(this, msg, null) { Caption = "Executing Task" }
                );
            //ApplicationViewModel..DialogService.ShowMessage(msg, "Executing Task", "Ok", null);
            this.RunTask();
        }

        public void RunTask()
        {
            if (!this.IsEnabled)
            {
                return;
            }

            // do something.
            var taskService = SimpleIoc.Default.GetInstance<ITaskService>();
            var task = taskService[this.Task.TypeName];
            var success = task.Execute();
        }

        private bool RunTaskEnabled(object sender)
        {
            bool canExecute = this.IsEnabled;
            return canExecute;
        }
        #endregion

    }
}
