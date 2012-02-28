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
namespace Jedzia.BackBock.ViewModel.MainWindow
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
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

        //private readonly IMainWindow mainWindow;
        private MainWindowCommandModel mainWindowCommands;

        #endregion

        /*public IOService MainIOService
        {
            get { return applicationViewModel.MainIOService; }
            //set { ioService = value; }
        }*/


        #region Constructors

        public MainWindowViewModel(ApplicationViewModel applicationViewModel/*, IMainWindow mainWindow*/)
        {
            //MessageBox.Show("MainWindowViewModel create");
            this.applicationViewModel = applicationViewModel;
            //this.mainWindow = applicationViewModel.MainWindow;
            if (ViewModelBase.IsInDesignModeStatic)
            {
                mainWindow_Initialized(null, null);
            }
            else
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
                applicationViewModel.MainWindow.Initialized += new EventHandler(mainWindow_Initialized);
                this.generalCommands = new GeneralCommandsModel(/*applicationViewModel,*/ applicationViewModel.MainWindow);
            }
            /*if (this.mainWindow.Designer == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }*/
        }

        void mainWindow_Initialized(object sender, EventArgs e)
        {
            Data = GetSampleData();
            //MessageBox.Show("MainWindowViewModel mainWindow_Initialized");
            //this.mainWindow.Designer.DataContext = bdvm;
        }

        public BackupDataViewModel GetSampleData()
        {
            //System.Diagnostics.Debugger.Launch();
            this.Data2 = SampleResourceProvider.GenerateSampleData();
            //MessageBox.Show("MainWindowViewModel GetSampleData");
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
                    this.mainWindowCommands = new MainWindowCommandModel(/*this.mainWindow*/);
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
        }

        private bool TestEnabled(object sender)
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

    }
}