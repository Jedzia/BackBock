// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupItemViewModel.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Input;
    using Jedzia.BackBock.Model.Data;
    using Jedzia.BackBock.ViewModel.Commands;

    public partial class BackupItemViewModel : IDataErrorInfo
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

        #region Properties

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        ///   </returns>
        public string Error
        {
            get
            {
                return "We have an Error!";
            }
        }

        /*private void LogMessageEvent(BuildMessageEventArgs e)
        {
            MessengerInstance.Send(e);
        }*/
        public Type NumerableType
        {
            get
            {
                // return typeof(PathViewModel);
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
            // this.EditCollection();
            // MessageBox.Show("Edit Collection");
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

        /*
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
*/
        private bool TaskDataClickedEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }

        // private ITaskService taskProvider = TaskRegistry.GetInstance();

        private void TaskDataClickedExecuted(object o)
        {
            this.data.ModifyTaskData(
                this.Log,
                rtask =>
                {
                    this.Task.TaskInstance = rtask;

                    // Todo: Maybe move the ViewModel creation to the Composition Root of the view.!
                    // var wnd = ControlRegistrator.GetInstanceOfType<Window>(WindowTypes.TaskEditor);
                    var wnd = ApplicationContext.TaskWizardProvider.GetTaskEditor();
                    wnd.DataContext = this;
                    // wnd.DataContext = task;
                    this.Task.PropertyChanged += this.TaskPropertyChanged;
                    var result = wnd.ShowDialog();
                    this.Task.PropertyChanged -= this.TaskPropertyChanged;
                    return result;
                });
        }

        private void TaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TypeName")
            {
                this.Task.data.AnyAttr.Clear();
                this.Task.TaskInstance = this.data.UpdateTaskData(this.Log);
                this.Task.data.OnPropertyChanged("AnyAttr");

                // var tvm = (TaskType)sender;
                // tvm.AnyAttr.Clear();

                // var task = this.InitTaskEditor(tvm.TypeName, tvm.AnyAttr);
                // Todo: fix.
                /*tvm.TaskInstance = task;

                tvm.data.OnPropertyChanged("AnyAttr");*/
            }
        }

        #endregion

        #region RunTask Command

        #region Fields

        private bool EnableLogging = true;
        private RelayCommand runTaskCommand;

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

        // TaskSetupEngine tse;
        public void RunTask()
        {
            this.data.RunTask(this.Log);
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

            // this.MessengerInstance.Send(msg);
            // MessengerInstance.Send(
            // new DialogMessage(this, msg, null) { Caption = "Executing Task" }
            // );
            MessengerInstance.Send("Executing Task" + msg);

            // ApplicationViewModel..DialogService.ShowMessage(msg, "Executing Task", "Ok", null);
            this.RunTask();
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        ///   </returns>
        public string this[string columnName]
        {
            get
            {
                return columnName + " Column Error";
            }
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            this.Validate("OnPropertyChanged " + propertyName);
            base.OnPropertyChanged(propertyName);
        }

        partial void DataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Validate("DataPropertyChanged " + e.PropertyName);
        }

        private void Log(string e)
        {
            if (this.EnableLogging)
            {
                MessengerInstance.Send(e + Environment.NewLine);
            }
        }

        partial void PathCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Mit ObservableCollection kann das ViewModel automatisch auf entfernen und
            // hinzufügen von Objekten reagieren.
            this.Validate("PathCollectionChanged " + e.Action);

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


        private void Validate(string propertyName)
        {
            {
                // if (this.Task)
            }

            MessengerInstance.Send(propertyName);

            // MessageBox.Show(propertyName);
        }

        #region EditorOpening Command

        #region Fields

        private List<PathDataType> back;
        private RelayCommand editorOpeningCommand;

        #endregion

        #region Properties

        [EditorBrowsable(EditorBrowsableState.Never),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICommand EditorOpeningCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.editorOpeningCommand == null)
                {
                    this.editorOpeningCommand = new RelayCommand(this.EditorOpeningExecuted, this.EditorOpeningEnabled);
                }

                return this.editorOpeningCommand;
            }
        }

        #endregion

        public virtual List<PathDataType> ClonePath(List<PathDataType> sources)
        {
            List<PathDataType> result = null;
            using (var ms = new MemoryStream())
            {
                // BinaryWriter bw = new BinaryWriter(ms);
                // ISerializable
                var bf = new BinaryFormatter();
                bf.Serialize(ms, sources);
                ms.Seek(0, SeekOrigin.Begin);
                result = (List<PathDataType>)bf.Deserialize(ms);
            }

            /*List<PathDataType> result = new List<PathDataType>();
                        foreach (var source in sources)
                        {
                            PathDataType n = new PathDataType();
                            n.Path = source.Path;
                            n.UserData = source.UserData;
                            n.Exclusion = source.Exclusion;
                            n.Inclusion = source.Inclusion;
                            //return ((PathDataType)(this.MemberwiseClone()));
                            result.Add(n);
                            //yield return n;
                        }*/
            return result;
        }

        private bool EditorOpeningEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }

        private void EditorOpeningExecuted(object o)
        {
            // this is not a good way to make undo.
            this.back = this.ClonePath(this.data.Path);

            // back = this.Clone();

            // MessageBox.Show("EditorOpeningExecuted");
            // this.EditorOpening();
        }

        #endregion

        #region EditorCancel Command

        #region Fields

        private RelayCommand editorCancelCommand;

        #endregion

        #region Properties

        [EditorBrowsable(EditorBrowsableState.Never),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICommand EditorCancelCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.editorCancelCommand == null)
                {
                    this.editorCancelCommand = new RelayCommand(this.EditorCancelExecuted, this.EditorCancelEnabled);
                }

                return this.editorCancelCommand;
            }
        }

        #endregion

        private bool EditorCancelEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }

        private void EditorCancelExecuted(object o)
        {
            // return;
            // this is not a good way to make undo.
            // backing field to null, so auto renew the ObservableCollection on the next request.
            this.path = null;

            // restore saved data from saved backup.
            this.data.Path = this.back;

            /*this.path = new System.Collections.ObjectModel.ObservableCollection<PathViewModel>();
            foreach (var item in this.back)
            {
                var colItem = new PathViewModel(item);
                colItem.PropertyChanged += OnDataPropertyChanged;
                this.path.Add(colItem);
            }
            this.path.CollectionChanged += OnPathCollectionChanged;*/
            RaisePropertyChanged(() => this.Paths);

            // this.EditorCancel();
        }

        #endregion
    }
}