using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows.Input;
using Jedzia.BackBock.ViewModel.Data;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    public class WorkAreaCommands
    {
        private IMainWindow mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WorkAreaCommands"/> class.
        /// </summary>
        public WorkAreaCommands(IMainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        #region RunTask Command

        private RelayCommand runTaskCommand;

        public ICommand RunTaskCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.runTaskCommand == null)
                {
                    this.runTaskCommand = new RelayCommand(this.RunTaskExecuted, this.RunTaskEnabled);
                }

                return this.runTaskCommand;
            }
        }


        private void RunTaskExecuted(object o)
        {
            var bivm = this.mainWindow.SelectedItem as BackupItemViewModel;
            if (bivm == null)
            {
                return;
            }
            bivm.RunTaskCommand.Execute(o);
        }

        private bool RunTaskEnabled(object sender)
        {
            var bivm = this.mainWindow.SelectedItem as BackupItemViewModel;
            bool canExecute = bivm != null && bivm.RunTaskCommand.CanExecute(sender);
            return canExecute;
        }
        #endregion

        #region RunAllTasks Command

        private RelayCommand runAllTasksCommand;

        public ICommand RunAllTasksCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.runAllTasksCommand == null)
                {
                    this.runAllTasksCommand = new RelayCommand(this.RunAllTasksExecuted, this.RunAllTasksEnabled);
                }

                return this.runAllTasksCommand;
            }
        }


        private void RunAllTasksExecuted(object o)
        {
            //this.RunAllTasks();
        }

        private bool RunAllTasksEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

    }
}
