// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskWizardViewModel.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Wizard
{
    using System;
    using System.Threading;
    using System.Windows.Input;
    using Jedzia.BackBock.ViewModel.Commands;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class TaskWizardViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The <see cref="Path" /> property's name.
        /// </summary>
        public const string PathPropertyName = "Path";

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private RelayCommand cancelCommand;

        /// <summary>
        /// The event handler for the <see cref="TaskWizardViewModel.Closed"/> event.
        /// </summary>
        private EventHandler<EventArgs> closed;

        private RelayCommand finishCommand;
        private TaskWizardFsm fsm;
        private RelayCommand nextCommand;

        private string path;
        private RelayCommand previousCommand;

        private IStateWizard stateWizard;
        private string title = "Task Wizard";

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Wizard is closed.
        /// </summary>
        public event EventHandler<EventArgs> Closed
        {
            add
            {
                this.closed += value;
            }

            remove
            {
                this.closed -= value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the wizards cancel command.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new RelayCommand(
                        o => this.fsm.Fire(Trigger.Cancel));
                }

                return this.cancelCommand;
            }
        }

        /// <summary>
        /// Gets the wizards finish command.
        /// </summary>
        public ICommand FinishCommand
        {
            get
            {
                if (this.finishCommand == null)
                {
                    this.finishCommand = new RelayCommand(
                        o => this.fsm.Fire(Trigger.Finished),
                        sender =>
                        {
                            if (this.CheckWizard(false))
                            {
                                return false;
                            }

                            return this.fsm.State == State.ReadyToAccept;
                        });
                }

                return this.finishCommand;
            }
        }

        /// <summary>
        /// Gets the next wizard page command.
        /// </summary>
        public ICommand NextCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                return this.nextCommand ?? (this.nextCommand = new RelayCommand(this.NextExecuted, this.NextEnabled));
            }
        }

        /// <summary>
        /// Gets or sets the Path property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }

            set
            {
                if (this.path == value)
                {
                    return;
                }

                this.path = value;
                RaisePropertyChanged(() => this.Path);
            }
        }

        /// <summary>
        /// Gets the previous wizard page command.
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                return this.previousCommand ??
                       (this.previousCommand = new RelayCommand(this.PreviousExecuted, this.PreviousEnabled));
            }
        }

        /// <summary>
        /// Gets or sets the Title property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        /// <value>
        /// The Title property.
        /// </value>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title == value)
                {
                    return;
                }

                this.title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the Wizard of the View.
        /// </summary>
        public IStateWizard Wizard
        {
            get
            {
                return this.stateWizard;
            }

            set
            {
                if (this.stateWizard == value)
                {
                    return;
                }

                this.stateWizard = value;

                // On a new assigned Wizard, this means a new Window, reset the fsm, etc. to initial conditions.
                this.Reset();
            }
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClosed(EventArgs e)
        {
            EventHandler<EventArgs> handler = Interlocked.CompareExchange(ref this.closed, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Checks if the wizard was injected.
        /// </summary>
        /// <param name="doThrow">if set to <c>true</c> does throw an Exception.</param>
        /// <returns><c>true</c> if the wizard instance is ready.</returns>
        /// <exception cref="NotSupportedException">The Wizard instance was not set up correctly before calling a method.</exception>
        private bool CheckWizard(bool doThrow)
        {
            if (this.Wizard == null)
            {
                this.Title = "  !!! No Wizard Instance Set. !!!  ";
                if (doThrow)
                {
                    throw new NotSupportedException(
                        "The Wizard instance was not set up correctly before calling a method.");
                }

                return true;
            }

            return false;
        }

        private void FsmCanceled(object sender, EventArgs e)
        {
            this.Tidyup();
        }

        private void FsmFinished(object sender, EventArgs e)
        {
            // do task creation
            // this.MessengerInstance.Send<SomeFormOfData>(this.data, MessengerTokens.TaskCreation);
            this.Tidyup();
        }

        private bool NextEnabled(object sender)
        {
            if (this.CheckWizard(false))
            {
                return false;
            }

            bool canExecute =
                this.fsm.State == State.Initial |
                this.fsm.State == State.ChooseTaskType |
                this.fsm.State == State.SelectFolders;
            return canExecute;
        }

        private void NextExecuted(object o)
        {
            this.CheckWizard(true);
            
            // var pgc = this.Wizard.PageCount;
            // var pgs = this.Wizard.SelectedPage;
            this.fsm.Fire(Trigger.Next);
            this.Wizard.SelectedPage++;

            // this.Next();
        }

        private bool PreviousEnabled(object sender)
        {
            if (this.CheckWizard(false))
            {
                return false;
            }

            bool canExecute =
                this.fsm.State == State.ChooseTaskType |
                this.fsm.State == State.SelectFolders |
                this.fsm.State == State.ReadyToAccept;
            return canExecute;
        }

        private void PreviousExecuted(object o)
        {
            this.CheckWizard(true);

            // this.Previous();
            this.fsm.Fire(Trigger.Previous);
            this.Wizard.SelectedPage--;
        }

        private void Reset()
        {
            this.fsm = new TaskWizardFsm();
            this.fsm.Canceled += this.FsmCanceled;
            this.fsm.Finished += this.FsmFinished;
        }

        private void Tidyup()
        {
            if (this.fsm != null)
            {
                this.fsm.Canceled -= this.FsmCanceled;
                this.fsm.Finished -= this.FsmFinished;
            }

            // this.fsm = null;
            // SimpleIoc.Default.Unregister<TaskWizardViewModel>(this);
            this.Wizard.Close();

            // this.Wizard = null;
            this.OnClosed(EventArgs.Empty);

            // this.Candidate.Destroy();
            // base.Cleanup();
        }
    }
}