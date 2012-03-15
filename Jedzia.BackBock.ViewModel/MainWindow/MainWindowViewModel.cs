// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
using System;
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
    using Microsoft.Build.Framework;

    public sealed class MainWindowViewModel : ViewModelBase
    {
        #region WindowTypes enum

        public enum WindowTypes
        {
            [CheckType(typeof(Window))]
            TaskWizard,
            //ClassFieldOptPage,
            //ClassMethodOptPage,
            //ClassPropertyOptPage,
            //ClassEventOptPage,
            //SettingsPage,
        }

        #endregion

        #region Fields
        private BackupDataViewModel bdvm;

        public ApplicationViewModel ApplicationViewModel
        {
            get
            {
                return this.applicationViewModel;
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

        private readonly ApplicationViewModel applicationViewModel;
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

        public MainWindowViewModel(ApplicationViewModel applicationViewModel, IMainWindow mainWindow)
        {
            //MessageBox.Show("MainWindowViewModel create0");
            this.applicationViewModel = applicationViewModel;
            //MessageBox.Show("MainWindowViewModel create1");
            this.mainWindow = applicationViewModel.MainWindow;
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
                applicationViewModel.MainWindow.Initialized += this.mainWindow_Initialized;
                this.generalCommands = new GeneralCommandsModel(/*applicationViewModel,*/ 
                    this,
                    applicationViewModel.MainWindow);
            }
            /*if (this.mainWindow.Designer == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }*/

            //ListBox lb;
            this.MessengerInstance.Register<BuildMessageEventArgs>(this, LogMessageEvent);
            //this.MessengerInstance.Register<TaskCommandLineEventArgs>(this, LogMessageEvent);
            //this.MessengerInstance.Register<string>(this, MainWindowMessageReceived);
            this.MessengerInstance.Register<string>(this, LogMessageEvent);
            this.MessengerInstance.Register<MVVM.Messaging.DialogMessage>(this, MainWindowMessageReceived);
            this.MessengerInstance.Register<Exception>(this, true, MainWindowExceptionReceived);

            var xxx = new { depp = WindowTypes.TaskWizard };
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

        private void LogMessageEvent(string e)
        {
            logsb.Append(DateTime.Now);
            logsb.Append(": ");
            logsb.Append(e);
            logsb.Append(Environment.NewLine);
            RaisePropertyChanged(LogTextPropertyName);
            mainWindow.UpdateLogText();
        }

        private void LogMessageEvent(BuildMessageEventArgs e)
        {
            //var text = e.Timestamp + ":[" + e.ThreadId + "." + e.SenderName + "]" + e.Message + e.HelpKeyword;
            logsb.Append(e.Timestamp);
            logsb.Append(":[");
            logsb.Append(e.ThreadId);
            logsb.Append(".");
            logsb.Append(e.SenderName);
            logsb.Append("]");
            logsb.Append(" ");
            logsb.Append(e.Message);
            logsb.Append("    (");
            logsb.Append(e.HelpKeyword);
            logsb.Append(")");
            logsb.Append(Environment.NewLine);
            RaisePropertyChanged(LogTextPropertyName);
            mainWindow.UpdateLogText();
            //this.LogText += text + Environment.NewLine;
        }
        private StringBuilder logsb = new StringBuilder();


        /// <summary>
        /// The <see cref="LogText" /> property's name.
        /// </summary>
        public const string LogTextPropertyName = "LogText";

        /// <summary>
        /// Sets and gets the LogText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LogText
        {
            get
            {
                //return logText;
                return logsb.ToString();
            }
            set
            {
                //Set(LogTextPropertyName, ref logText, value);
                //
            }
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
                var taskList = ViewModel.ApplicationViewModel.TaskService.GetRegisteredTasks();
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

        public BackupDataViewModel GetSampleData()
        {
            //System.Diagnostics.Debugger.Launch();

            //MessageBox.Show("Before MainWindowViewModel GetSampleData");
            this.Data2 = SampleResourceProvider.GenerateSampleData();
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
            this.logsb.Length = 0;
            RaisePropertyChanged(LogTextPropertyName);
        }

        private bool ClearLogEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion


        //private Type classSpecificationWindowType;

        internal void OpenFile(string path)
        {
            this.Data2 = ModelLoader.LoadBackupData(path);
            Data = new BackupDataViewModel(this.Data2);
            //this.mainWindow.Designer.DataContext = bdvm;
        }

        internal void SaveFile(string path)
        {
            ModelSaver.SaveBackupData(bdvm.data, path);
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
                var wnd = ApplicationViewModel.TaskWizardProvider.GetWizard();
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