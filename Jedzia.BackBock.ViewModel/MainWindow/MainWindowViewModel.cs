﻿// <copyright file="$FileName$" company="$Company$">
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
namespace Jedzia.BackBock.ViewModel.MainWindow
{
    public sealed class MainWindowViewModel //: INotifyPropertyChanged
    {
        #region Fields

        public ApplicationViewModel ApplicationViewModel
        {
            get
            {
                return this.applicationViewModel;
            }
        }

        private readonly ApplicationViewModel applicationViewModel;
        private readonly ApplicationCommandModel applicationCommands;

        //public static readonly DependencyProperty DesignerCommandsProperty = DependencyProperty.Register(
        //"DesignerCommands", typeof(DesignerCanvasCommandModel), typeof(DesignerCanvas));

        private readonly IMainWindow mainWindow;
        private MainWindowCommandModel mainWindowCommands;

        #endregion

        #region Constructors

        public MainWindowViewModel(ApplicationViewModel applicationViewModel, IMainWindow mainWindow)
        {
            /*if (mainWindow.Designer == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }*/
            this.applicationViewModel = applicationViewModel;
            this.mainWindow = mainWindow;
            this.mainWindow.Initialized += new EventHandler(mainWindow_Initialized);
            this.applicationCommands = new ApplicationCommandModel(mainWindow);
        }

        BackupDataViewModel bvm;
        void mainWindow_Initialized(object sender, EventArgs e)
        {
            bvm = GetSampleData();
            this.mainWindow.Designer.DataContext = bvm;
        }

        public static BackupDataViewModel GetSampleData()
        {
            var data = SampleResourceProvider.GenerateSampleData();
            var bvm = new BackupDataViewModel(data);
            return bvm;
        }

        #endregion

        #region Properties

        public ApplicationCommandModel MainApplicationCommands
        {
            get
            {
                /*if (applicationCommands == null)
                {
                    this.applicationCommands = new ApplicationCommandModel(mainWindow);
                }*/
                return this.applicationCommands;
            }
        }

        public MainWindowCommandModel MainWindowCommands
        {
            get
            {
                if (this.mainWindowCommands == null)
                {
                    this.mainWindowCommands = new MainWindowCommandModel(this.mainWindow);
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
            bvm.BackupItems.Add(new BackupItemViewModel(new Jedzia.BackBock.Model.Data.BackupItemType()));
        }

        private bool TestEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion


        private Type classSpecificationWindowType;
    }
}