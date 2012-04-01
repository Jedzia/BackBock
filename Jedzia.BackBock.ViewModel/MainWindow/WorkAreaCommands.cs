using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows.Input;
using Jedzia.BackBock.ViewModel.Data;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    /// <summary>
    /// Holds the <see cref="ICommand"/>s of the main window's work area.
    /// </summary>
    /// <remarks>Used by the <see cref="MainWindowViewModel"/>.</remarks>
    public class WorkAreaCommands
    {
        private IMainWorkArea workArea;
        private MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WorkAreaCommands"/> class.
        /// </summary>
        /// <param name="mainWindowViewModel">The main window view model.</param>
        /// <param name="workArea">The work area of the main window.</param>
        public WorkAreaCommands(MainWindowViewModel mainWindowViewModel, IMainWorkArea workArea)
        {
            this.workArea = workArea;
            this.mainWindowViewModel = mainWindowViewModel;
        }

        #region RunTask Command

        private RelayCommand runTaskCommand;

        /// <summary>
        /// Gets the run task command. Runs a selected task.
        /// </summary>
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
            var bivm = this.workArea.SelectedItem as BackupItemViewModel;
            //var bivm = this.mainWindow.SelectedItem as BackupItemViewModel;
            if (bivm == null)
            {
                return;
            }
            bivm.RunTaskCommand.Execute(o);
        }

        private bool RunTaskEnabled(object sender)
        {
            var bivm = this.workArea.SelectedItem as BackupItemViewModel;
            bool canExecute = bivm != null && bivm.RunTaskCommand.CanExecute(sender);
            return canExecute;
        }
        #endregion

        #region RunAllTasks Command

        private RelayCommand runAllTasksCommand;

        /// <summary>
        /// Gets the run all tasks command. Runs all enabled tasks.
        /// </summary>
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
            this.mainWindowViewModel.RunAllTasks();
        }

        private bool RunAllTasksEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        // RunTaskWizardCommand

        #region RunTaskWizard Command

        private RelayCommand runTaskWizardCommand;

        /// <summary>
        /// Gets the run task wizard command. Opens the task wizard.
        /// </summary>
        public ICommand RunTaskWizardCommand
        {
            get
            {
                if (this.runTaskWizardCommand == null)
                {
                    this.runTaskWizardCommand = new RelayCommand(this.RunTaskWizardExecuted, this.RunTaskWizardEnabled);
                }

                return this.runTaskWizardCommand;
            }
        }


        private void RunTaskWizardExecuted(object o)
        {
            this.mainWindowViewModel.RunTaskWizard();
        }

        private bool RunTaskWizardEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

    }
}
