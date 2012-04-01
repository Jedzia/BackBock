// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using Jedzia.BackBock.Model;
using Jedzia.BackBock.ViewModel.Data;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows.Input;
using Jedzia.BackBock.Model.Data;
using System.Text;
using System.Xml;
using System.IO;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using Jedzia.BackBock.ViewModel.MVVM.Threading;
    using Jedzia.BackBock.DataAccess;
    using Jedzia.BackBock.ViewModel.Util;

    public sealed class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private BackupDataViewModel bdvm;

        public ApplicationContext ApplicationViewModel
        {
            get
            {
                return this.applicationContext;
            }
        }

        public BackupDataViewModel Data
        {
            get
            {
                return this.bdvm;
            }
            set
            {
                if (this.bdvm == value)
                {
                    return;
                }
                this.bdvm = value;
                RaisePropertyChanged("Data");
                RaisePropertyChanged("Groups");
            }
        }

        public IEnumerable<string> Groups
        {
            get
            {
                if (this.Data == null || this.Data.BackupItems == null)
                {
                    return new List<string>();
                }

                var groups = this.Data.BackupItems
                    .Select((o) => o.ItemGroup)
                    .Distinct()
                    .OrderBy((x) => x);
                return groups;
            }
        }

        private BackupData data2;
        public BackupData Data2
        {
            get
            {
                return this.data2;
            }
            set
            {
                if (this.data2 == value)
                {
                    return;
                }
                this.data2 = value;
                RaisePropertyChanged("Data2");
            }
        }

        private readonly ApplicationContext applicationContext;
        private readonly GeneralCommandsModel generalCommands;

        //public static readonly DependencyProperty DesignerCommandsProperty = DependencyProperty.Register(
        //"DesignerCommands", typeof(DesignerCanvasCommandModel), typeof(DesignerCanvas));

        private readonly IMainWindow mainWindow;
        private MainWindowCommandModel mainWindowCommands;

        #endregion

        /*public IOService MainIOService
        {
            get { return applicationViewModel.MainIOService; }
            //set { ioService = value; }
        }*/


        #region Constructors
        
        public override void Cleanup()
        {
            if (this.generalCommands != null)
            {
                this.generalCommands.CleanUp();
            }

            if (this.generalCommands != null)
            {
                this.bdvm.Cleanup();
            }

            // etc.
            this.bdvm = null;
            //this.generalCommands = null;
            base.Cleanup();
        }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public IEnumerable<string> Repositories
        {
            get
            {
                return this.dataprovider.LoadedServices;
            }
        }

        private readonly IBackupDataService dataprovider;
        private readonly ILogger logger;
        public MainWindowViewModel(ApplicationContext applicationContext,
            IBackupDataService dataprovider, ILogger logger)
        {
            Guard.NotNull(() => applicationContext, applicationContext);
            Guard.NotNull(() => dataprovider, dataprovider);
            Guard.NotNull(() => logger, logger);
            //MessageBox.Show("MainWindowViewModel create0");
            this.applicationContext = applicationContext;
            //MessageBox.Show("MainWindowViewModel create1");
            this.mainWindow = applicationContext.MainWindow;
            this.dataprovider = dataprovider;
            this.logger = logger;
            //MessageBox.Show("MainWindowViewModel create2");
            if (ViewModelBase.IsInDesignModeStatic)
            {
                //MessageBox.Show("MainWindowViewModel create3"); 
                mainWindow_Initialized(null, null);
                //MessageBox.Show("MainWindowViewModel create4");
                //MessageBox.Show("MainWindowViewModel mainWindow_Initialized");
            }
            else
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
                applicationContext.MainWindow.Initialized += this.mainWindow_Initialized;
                this.generalCommands = new GeneralCommandsModel(/*applicationViewModel,*/ 
                    this,
                    applicationContext.MainWindow);
            }
            /*if (this.mainWindow.Designer == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }*/

            //ListBox lb;
            //this.MessengerInstance.Register<BuildMessageEventArgs>(this, logger.LogMessageEvent);
            //this.MessengerInstance.Register<TaskCommandLineEventArgs>(this, LogMessageEvent);
            //this.MessengerInstance.Register<string>(this, MainWindowMessageReceived);
            this.MessengerInstance.Register<string>(this, logger.LogMessageEvent);
            this.MessengerInstance.Register<MVVM.Messaging.DialogMessage>(this, MainWindowMessageReceived);
            this.MessengerInstance.Register<Exception>(this, true, MainWindowExceptionReceived);

        }

        /*private void LogMessageEvent(Exception e)
        {
            logsb.Append(DateTime.Now);
            logsb.Append(": ");
            logsb.Append(e.Message);
            logsb.Append(Environment.NewLine);
            RaisePropertyChanged(LogTextPropertyName);
            mainWindow.UpdateLogText();
        }*/

        void MainWindowExceptionReceived(Exception e)
        {
            this.mainWindow.DialogService.ShowMessage(e.Message, e.Source, "Ok", null);
        }

        /// <summary>
        /// Gets the list of available task types.
        /// </summary>
        public IEnumerable<string> TaskList
        {
            get
            {
                //var taskService = SimpleIoc.Default.GetInstance<ITaskService>();
                //throw new NotImplementedException("m999");
                //var tasklist = TaskContext.GetRegisteredTasks();
                var taskList = ApplicationContext.TaskContext.TaskService.GetRegisteredTasks();
                return taskList;
            }
        }

        void MainWindowMessageReceived(string e)
        {
            this.mainWindow.DialogService.ShowMessage(e, "Message", "Ok", null);
        }

        void MainWindowMessageReceived(MVVM.Messaging.DialogMessage e)
        {
            this.mainWindow.DialogService.ShowMessage(e.Content, e.Caption, "Ok", null);
        }

        void mainWindow_Initialized(object sender, EventArgs e)
        {
            try
            {
                Data = GetSampleData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("GetSampleData Exception - " + ex.ToString());
            }

            //MessageBox.Show("MainWindowViewModel mainWindow_Initialized");
            //this.mainWindow.Designer.DataContext = bdvm;
        }

        /// <summary>
        /// Represents a users security context.
        /// </summary>
        public class MyPrincipal : System.Security.Principal.IPrincipal
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="T:MyPrincipal"/> class
            /// </summary>
            public MyPrincipal()
            {
            }

            /// <summary>
            /// Gets the identity of the current principal.
            /// </summary>
            /// <value>The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.</value>
            public System.Security.Principal.IIdentity Identity
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Determines whether the current principal belongs to the specified role.
            /// </summary>
            /// <param name="role">The name of the role for which to check membership.</param>
            /// <returns>
            /// <c>true</c> if the current principal is a member of the specified role; otherwise, <c>false</c>.
            /// </returns>
            public bool IsInRole(string role)
            {
                throw new NotImplementedException();
            }
        }

        public BackupDataViewModel GetSampleData()
        {
            //System.Diagnostics.Debugger.Launch();

            //MessageBox.Show("Before MainWindowViewModel GetSampleData");
            //string connection = null;
            /*if (dataprovider.ServiceType == BackupRepositoryType.FileSystemProvider)
            {
                var startupPath = applicationContext.Settings.GetStartupDataFile();
                connection = startupPath;
            }*/
            // this.Data2 = dataprovider.GetBackupData(new MyPrincipal());
            // this.Data2 = dataprovider.GetBackupData(connection, new MyPrincipal(), null);

            // load static => this is the sample data. BackupRepositoryType.FileSystemProvider is
            // "load from file" and BackupRepositoryType.Database from a database.
            this.Data2 = dataprovider.GetBackupData(BackupRepositoryType.Static, new MyPrincipal(), null);
            //MessageBox.Show("After MainWindowViewModel GetSampleData");
            return new BackupDataViewModel(this.Data2);
        }

        #endregion

        #region Properties

        public GeneralCommandsModel MainApplicationCommands
        {
            get
            {
                /*if (applicationCommands == null)
                {
                    this.applicationCommands = new ApplicationCommandModel(mainWindow);
                }*/
                return this.generalCommands;
            }
        }

        public MainWindowCommandModel MainWindowCommands
        {
            get
            {
                if (this.mainWindowCommands == null)
                {
                    this.mainWindowCommands = new MainWindowCommandModel(this, this.mainWindow);
                }
                return this.mainWindowCommands;
            }
        }

        public static string MainWindowTitle
        {
            get
            {
                return "Das ist mein Diagram Designer";
            }
        }

        #endregion


        /*private RelayCommand addAttributeCommand;
        public ICommand AddAttributeCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.addAttributeCommand == null)
                {
                    this.addAttributeCommand = new RelayCommand(e => this.AddAttribute_Executed(), this.AddAttribute_Enabled );
                }

                return this.addAttributeCommand;
            }
        }


        private void AddAttribute_Executed()
        {
            designerCanvas.AddAttributeType();
        }

        private bool AddAttribute_Enabled(object sender)
        {
            bool canExecute = designerCanvas.SelectionService.CurrentSelection.Count() > 0;
            return canExecute;
        }*/

        #region Test Command

        private RelayCommand testCommand;

        public ICommand TestCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.testCommand == null)
                {
                    this.testCommand = new RelayCommand(this.TestExecuted, this.TestEnabled);
                }

                return this.testCommand;
            }
        }


        private void TestExecuted(object o)
        {
            //this.Test();
            //MessageBox.Show("Mooo");

            Data.BackupItems.Add(new BackupItemViewModel(new Jedzia.BackBock.Model.Data.BackupItemType()));
            //this.dataprovider.InsertBackup

            this.mainWindow.DialogService.ShowMessage("MainWindowViewModel.TestExecuted", "Test!", "Ok", null);
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?>" + Environment.NewLine +
@"<Project>" + Environment.NewLine +
@"<CreateItem Include=""C:\Temp\*.*"" Exclude=""*.abc"" Condition=""'$(FuckReports)'==''"" >" +
                //@"<Output TaskParameter=""TargetOutputs"" ItemName=""SandcastleOut"" " +
                //@"xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"" />" +
@"</CreateItem>" +
@"</Project>";

            var xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(xml);
                var ele = xdoc.CreateElement("TheElement");
                /*var bi = new Jedzia.BackBock.Tasks.BuildEngine.BuildItem(xdoc, "CreateItem", "TheItemInclude");
                var itemEle = xdoc["Project"]["CreateItem"];
                bi.InitializeFromItemElement(itemEle);
                bi.Include = "C:\\Temp\\*.*";
                bi.Exclude = "*.txt";
                bi.Condition = "'$(ShowReports)'==''";
                //Jedzia.BackBock.Tasks.TaskExtension bu = new ;
                
                
                var eleStr = bi.ItemElement.OuterXml;
                xdoc.DocumentElement.AppendChild(bi.ItemElement);*/
                var sw = new StringWriter();
                xdoc.Save(sw);
                var str = sw.ToString();
            }
            catch (Exception ex)
            {
                MainWindowExceptionReceived(ex);
                return;
            }
            //var obj = System.Windows.Markup.XamlReader.Parse(xml);
        }

        private bool TestEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        #region ClearLog Command

        private RelayCommand clearLogCommand;

        public ICommand ClearLogCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.clearLogCommand == null)
                {
                    this.clearLogCommand = new RelayCommand(this.ClearLogExecuted, this.ClearLogEnabled);
                }

                return this.clearLogCommand;
            }
        }


        private void ClearLogExecuted(object o)
        {
            this.ClearLog();
        }

        private void ClearLog()
        {
            this.logger.Reset();
        }

        private bool ClearLogEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion


        //private Type classSpecificationWindowType;

        internal void Open()
        {
            var path = applicationContext.MainIOService.OpenFileDialog(string.Empty);
            if (!string.IsNullOrEmpty(path))
            {
                this.OpenFile(path);
                //designerCanvas.DesignerCanvasFileProcessor.OpenExecuted(o, args);
            }
        }
        
        internal void OpenFile(string path)
        {
            // Todo: switch this to this.dataprovider.Load( ... );
            //this.Data2 = ModelLoader.LoadBackupData(path);
            this.Data2 = dataprovider.Load(path, new MyPrincipal(), null);
            Data = new BackupDataViewModel(this.Data2);
            //this.mainWindow.Designer.DataContext = bdvm;
        }

        internal void Cancel()
        {
            bdvm.CancelEdit();
        }

        internal void Save()
        {
            bdvm.EndEdit();
            if (bdvm.HasErrors)
                return;
            var path = applicationContext.MainIOService.SaveFileDialog(string.Empty);

            if (!string.IsNullOrEmpty(path))
            {
                this.SaveFile(path);
            }
        }

        internal void SaveFile(string path)
        {
            // Todo: switch this to this.dataprovider.Save( ... );
            //ModelSaver.SaveBackupData(bdvm.DataObject, path);
        }

        internal void RunAllTasks()
        {
            foreach (var item in this.Data.BackupItems)
            {
                item.RunTask();
            }
        }


        internal void RunTaskWizard()
        {
            try
            {
                //var wnd = ControlRegistrator.GetInstanceOfType<Window>(WindowTypes.TaskWizard);
                var wnd = ApplicationContext.TaskWizardProvider.GetWizard();
                //wnd.DataContext = this;
                //wnd.DataContext = task;
                //this.Task.PropertyChanged += Task_PropertyChanged;
                var result = wnd.ShowDialog();
            }
            catch (Exception ex)
            {
                this.MessengerInstance.Send(ex);
            }
            finally
            {
            }
        }



    }
}