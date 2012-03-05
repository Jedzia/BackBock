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
    using System.Collections;
    using System.Linq;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Xml;
    using System.Xml.Serialization;
    using Jedzia.BackBock.Model.Data;
    using Jedzia.BackBock.Tasks;
    using Jedzia.BackBock.ViewModel.Commands;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc;
    using Jedzia.BackBock.ViewModel.Tasks;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Tasks;
    using Microsoft.Build.Utilities;

    public partial class BackupItemViewModel : ILogger
    {
        #region WindowTypes enum

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

        #endregion

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

        private void TaskDataClickedExecuted(object o)
        {
            var wnd = ControlRegistrator.GetInstanceOfType<Window>(WindowTypes.TaskEditor);
            var taskService = SimpleIoc.Default.GetInstance<ITaskService>();
            var task = taskService[this.Task.TypeName];
            if (task == null)
            {
                return;
            }
            
            this.Task.TaskInstance = task;

            foreach (var item in this.Task.data.AnyAttr)
            {
                var taskType = task.GetType();
                var property = taskType.GetProperty(item.Name);
                if (property != null)
                {
                    object val = item.Value;
                    if (property.PropertyType.Name == "ITaskItem")
                    {
                        val = new TaskItem(item.Value);
                    }
                    else
                    {
                        val = Convert.ChangeType(item.Value, property.PropertyType);
                    }
                    property.SetValue(task, val, null);
                }
            }

            var dst = this.Task.data.AnyAttr.Where((e) => e.Name == "DestinationFolder").FirstOrDefault();
            if (dst != null)
            {
                var val = dst.Value;
            }
            PrepareTask(task);
            //var str = XamlSerializer.Save(task);
            //SerializeTest(task);

            wnd.DataContext = this;
            //wnd.DataContext = task;
            var result = wnd.ShowDialog();
        }
        const string fullrecursivePattern = "**";

        private void PrepareTask(ITask task)
        {
            // Todo: put this task generation extra.
            if (task is Backup)
            {
                var btask = (Backup)task;
                for (int index = 0; index < this.Paths.Count; index++)
                {
                    var item = this.Paths[index];
                }
                int xasd = 0;
                var result = this.Paths.Select((e) =>
                {
                    var cr = new CreateItem();
                    if (e.Inclusions.Count > 0)
                    {
                        cr.Include = e.Inclusions.Select(
                            (t) =>
                                {
                                    if (e.Path.EndsWith("\\"))
                                    {
                                        return new TaskItem(e.Path + t.Pattern);
                                    }
                                    else
                                        return new TaskItem(e.Path + "\\" + t.Pattern);
                                }
                            ).ToArray();
                    }
                    else
                    {
                        ITaskItem taskItem;
                        var finfo = new FileInfo(e.Path);
                        if (Directory.Exists(e.Path))
                        {
                            // a directory
                            taskItem = new TaskItem(e.Path + "\\" + fullrecursivePattern);
                        }
                        else
                        {
                            // a file
                            taskItem = new TaskItem(e.Path);
                        }
                        cr.Include = new[] { taskItem };
                    }

                    cr.Exclude = e.Exclusions.Select((t) => { return new TaskItem(e.Path + "\\" + t.Pattern); }).ToArray();

                    cr.Execute();

                    return cr;
                }
                );

                //var res = result.ToArray();
                var includes = result.SelectMany((e) => e.Include);
                btask.SourceFiles = includes.ToArray();
                btask.DestinationFolder = new TaskItem(@"C:\tmp\%(RecursiveDir)");
                //var itemsByType = new Hashtable();
                //foreach (var item in btask.SourceFiles)
                //{
                    //itemsByType.Add(
                //}
                //var bla = ItemExpander.ItemizeItemVector(@"@(File)", null, itemsByType);
                btask.BuildEngine = this.BuildEngine;
            }
        }
        private bool PrepareTask2(ITask task)
        {
            bool res = false;
            string xml =
                @"<Project ToolsVersion=""3.5"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">" +
                Environment.NewLine +
                "\t" + @"<UsingTask AssemblyFile=""C:\Program Files\MSBuild\ExtensionPack\MSBuild.ExtensionPack.dll"" " +
                @"TaskName=""MSBuild.ExtensionPack.Compression.Zip"" />" + Environment.NewLine +
                "\t" + @"<Target Name=""Target1"">" + Environment.NewLine +
                "\t" + @"" + "\t" + @"<ItemGroup>" + Environment.NewLine +
                "\t" + @"" + "\t" + @"" + "\t" + @"<FilesToZip Include=""C:\Temp\**"" />" + Environment.NewLine +
                "\t" + @"" + "\t" + @"</ItemGroup>" + Environment.NewLine +
                "\t" + @"" + "\t" +
                @"<MSBuild.ExtensionPack.Compression.Zip ZipFileName=""D:\TestZip.zip"" TaskAction=""Create"" " +
                @"CompressFiles=""@(FilesToZip)"" />" + Environment.NewLine +
                "\t" + @"</Target>" + Environment.NewLine +
                @"</Project>";
            //var proj2 = new Project();
            //var engine = new Engine(Consts.BinPath);
            var engine = new Engine(@"D:\E\Projects\CSharp\BackBock\Jedzia.BackBock.Application\bin\Debug");
            engine.RegisterLogger(this);
            //var proj2 = engine.CreateNewProject();

            //proj2.LoadXml(xml);
            //proj2.Build();

            // Todo: put this task generation extra.);
            if (task is Backup)
            {
                var btask = (Backup)task;
                for (int index = 0; index < this.Paths.Count; index++)
                {
                    var item = this.Paths[index];
                }

                btask.SourceFiles = new[] { new TaskItem(@"C:\Temp\raabeXX.jpg"), };
                btask.DestinationFolder = new TaskItem(@"C:\tmp\%(RecursiveDir)");
                btask.BuildEngine = this.BuildEngine;
                var proj = engine.CreateNewProject();
                proj.DefaultToolsVersion = "3.5";
                //var proj = new Project(this.buildEngine, "3.5");
                //var proj = new Project(Engine.GlobalEngine, "3.5");
                var target = proj.Targets.AddNewTarget("mainTarget");
                //var grp = target.AddNewTask("ItemGroup");
                var big = proj.AddNewItemGroup();
                //var cr = new BuildItem("Item","");
                //var big = new BuildItemGroup();

                //cr = big.AddNewItem("FilesToZip", @"C:\Temp\FolderB\**\*.*");
                //cr.Exclude = @"*.msi";

                foreach (var path in this.Paths)
                {
                    //var cr = big.AddNewItem("FilesToZip", @"C:\Temp\**;C:\Temp\FolderB\**\*.*");
                    BuildItem cr;
                    //cr.Exclude = @"*.msi";
                    if (path.Inclusions.Count > 0)
                    {
                        //var inclEle = string.Empty;
                        var inclEle = new StringBuilder();
                        for (int index = 0; index < path.Inclusions.Count; index++)
                        {
                            var incl = path.Inclusions[index];
                            inclEle.Append(path.Path);
                            inclEle.Append(incl.Pattern);
                            if (index != path.Inclusions.Count - 1)
                                inclEle.Append(";");
                        }
                        cr = big.AddNewItem("FilesToZip", inclEle.ToString());
                        var exclEle = new StringBuilder();
                        for (int index = 0; index < path.Exclusions.Count; index++)
                        {
                            var excl = path.Exclusions[index];
                            exclEle.Append(path.Path);
                            exclEle.Append(excl.Pattern);
                            if (index != path.Exclusions.Count - 1)
                                exclEle.Append(";");
                        }
                        cr.Exclude = exclEle.ToString();
                    }
                    else
                    {
                        var strit = path.Path + @"\**\*.*";
                        cr = big.AddNewItem("FilesToZip", strit);
                        var exclEle = new StringBuilder();
                        for (int index = 0; index < path.Exclusions.Count; index++)
                        {
                            var excl = path.Exclusions[index];
                            exclEle.Append(path.Path);
                            exclEle.Append(excl.Pattern);
                            if (index != path.Exclusions.Count - 1)
                                exclEle.Append(";");
                        }
                        cr.Exclude = exclEle.ToString();
                    }
                    //cr.Exclude = @"*.msi";
                }

                //Type btasktype = typeof(Backup);
                Type btasktype = task.GetType();
                proj.AddNewUsingTaskFromAssemblyName(btasktype.FullName, btasktype.Assembly.FullName);
                var batask = target.AddNewTask(btasktype.FullName);
                //batask.SetParameterValue("SourceFiles", @"C:\Temp\company.xmi");
                batask.SetParameterValue("SourceFiles", @"@(FilesToZip)");
                var pars = batask.GetParameterNames();
                res = proj.Build("mainTarget");
                
                //var res = result.ToArray();
                //var includes = result.SelectMany((e) => e.Include);
                //btask.SourceFiles = includes.ToArray();
                //var itemsByType = new Hashtable();
                //foreach (var item in btask.SourceFiles)
                //{
                //itemsByType.Add(
                //}
                //var bla = ItemExpander.ItemizeItemVector(@"@(File)", null, itemsByType);
            }
            return res;
        }
        private IBuildEngine buildEngine;

        public IBuildEngine BuildEngine
        {
            get
            {
                if (this.buildEngine == null)
                {
                    //this.buildEngine = new SimpleBuildEngine(LogMessageEvent);
                    var vmb = new ViewModelBuildEngine(this.MessengerInstance);
                    vmb.Enabled = true;
                    this.buildEngine = vmb;
                }
                return buildEngine;
            }
        }
        #endregion

        #region RunTask Command

        #region Fields

        private RelayCommand runTaskCommand;
        private bool Enabled = true;

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

            // do something.
            var taskService = SimpleIoc.Default.GetInstance<ITaskService>();
            var task1 = taskService[this.Task.TypeName];
            if (task1 != null)
            {
                var success = PrepareTask2(task1);
                MessengerInstance.Send("Finished Task: " + success);
                //return;
            }
            }
            catch (Exception e  )
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
        /// <summary>
        /// Initializes the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        public void Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += Log;
            eventSource.ErrorRaised += Log;
        }

        private void Log(object sender, BuildErrorEventArgs e)
        {
            if (this.Enabled)
                MessengerInstance.Send(e);
        }

        public void Log(object sender, BuildMessageEventArgs buildMessageEventArgs)
        {
            if (this.Enabled)
                MessengerInstance.Send(buildMessageEventArgs);
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
                return  LoggerVerbosity.Detailed;
            }
            set
            {
            }
        }
    }

    /*public class SimpleBuildEngine : IBuildEngine
    {
        Action<BuildMessageEventArgs> messageCallback;
        Action<BuildErrorEventArgs> errorCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBuildEngine"/> class.
        /// </summary>
        public SimpleBuildEngine(Action<BuildMessageEventArgs> messageCallback)
        {
            Guard.NotNull(() => messageCallback, messageCallback);
            this.messageCallback = messageCallback;
        }
        #region IBuildEngine Members

        public void LogMessageEvent(BuildMessageEventArgs e)
        {

            messageCallback(e);
            //System.Console.WriteLine(
            //    e.Timestamp + ":[" + e.ThreadId + "." + e.SenderName + "]" + e.Message + e.HelpKeyword);
        }

        #endregion

        #region IBuildEngine Members


        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            errorCallback(e);
        }

        public bool ContinueOnError
        {
            get { return false; }
        }

        #endregion
    }*/
}