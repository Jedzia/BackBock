
using System.Windows.Input;
using Jedzia.BackBock.ViewModel.Commands;
using System;

namespace Jedzia.BackBock.ViewModel.Wizard
{
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
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string title = "Task Wizard";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                {
                    return;
                }

                title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Path" /> property's name.
        /// </summary>
        public const string PathPropertyName = "Path";

        private string path;

        /// <summary>
        /// Sets and gets the Path property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                if (path == value)
                {
                    return;
                }

                path = value;
                RaisePropertyChanged(() => Path);
            }
        }

        public IStateWizard stateWizard;
        /// <summary>
        /// Gets or sets 
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
                Reset();
            }
        }
        /// <summary>
        /// Initializes a new instance of the TaskWizardViewModel class.
        /// </summary>
        public TaskWizardViewModel(/*IStateWizard instance*/)
        {
            //MessengerInstance.Send(pagecount.ToString());
            //baseWizard = new BaseWizard();
        }
        private TaskWizardFSM fsm;

        #region Next Command

        private RelayCommand nextCommand;

        public ICommand NextCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.nextCommand == null)
                {
                    this.nextCommand = new RelayCommand(this.NextExecuted, this.NextEnabled);
                }

                return this.nextCommand;
            }
        }


        private void NextExecuted(object o)
        {
            CheckWizard(true);
            var pgc = this.Wizard.PageCount;
            var pgs = this.Wizard.SelectedPage;
            fsm.Fire(Trigger.Next);
            this.Wizard.SelectedPage++;
            //this.Next();
        }

        private bool NextEnabled(object sender)
        {
            if (CheckWizard(false))
            {
                return false;
            }

            bool canExecute = 
                fsm.State == State.Initial | 
                fsm.State == State.ChooseTaskType | 
                fsm.State == State.SelectFolders;
            return canExecute;
        }
        #endregion

                #region Previous Command

        private RelayCommand previousCommand;

        public ICommand PreviousCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.previousCommand == null)
                {
                    this.previousCommand = new RelayCommand(this.PreviousExecuted, this.PreviousEnabled);
                }

                return this.previousCommand;
            }
        }


        private void PreviousExecuted(object o)
        {
            CheckWizard(true);
            //this.Previous();
            fsm.Fire(Trigger.Previous);
            this.Wizard.SelectedPage--;
        }

        /// <summary>
        /// Checks if the wizard was injected.
        /// </summary>
        /// <param name="doThrow">if set to <c>true</c> does throw an Exception.</param>
        /// <returns><c>true</c> if the wizard instance is ready.</returns>
        private bool CheckWizard(bool doThrow)
        {
            if (Wizard == null)
            {
                Title = "  !!! No Wizard Instance Set. !!!  ";
                if (doThrow)
                {
                    throw new NotSupportedException("The Wizard instance was not set up correctly before calling a method.");
                }
                return true;
            }
            return false;
        }

        private bool PreviousEnabled(object sender)
        {
            if (CheckWizard(false))
            {
                return false;
            }

            bool canExecute =
                fsm.State == State.ChooseTaskType |
                fsm.State == State.SelectFolders |
                fsm.State == State.ReadyToAccept;
            return canExecute;
        }
        #endregion

        private void Reset()
        {

            this.fsm = new TaskWizardFSM();
            this.fsm.Canceled += fsm_Canceled;
            this.fsm.Finished += fsm_Finished;
        }

        private void Tidyup()
        {
            if (this.fsm != null)
            {
                this.fsm.Canceled -= fsm_Canceled;
                this.fsm.Finished -= fsm_Finished;
            }

            //this.fsm = null;
            //SimpleIoc.Default.Unregister<TaskWizardViewModel>(this);

            this.Wizard.Close();
            //this.Wizard = null;
            OnClosed(EventArgs.Empty);
            //this.Candidate.Destroy();
            //base.Cleanup();
        }

        /// <summary>
        /// The event handler for the <see cref="E:TaskWizardViewModel.Closed"/> event.
        /// </summary>
        private EventHandler<EventArgs> closed;

        /// <summary>
        /// Occurs when 
        /// </summary>
        public event EventHandler<EventArgs> Closed
        {
            add
            {
                // TODO: write your implementation of the add accessor here
                this.closed += value;
            }

            remove
            {
                // TODO: write your implementation of the remove accessor here
                this.closed -= value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClosed(EventArgs e)
        {
            EventHandler<EventArgs> handler = System.Threading.Interlocked.CompareExchange(ref this.closed, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }

        void fsm_Finished(object sender, EventArgs e)
        {
            // do task creation
            // this.MessengerInstance.Send<SomeFormOfData>(this.data, MessengerTokens.TaskCreation);
            this.Tidyup();
        }

        void fsm_Canceled(object sender, EventArgs e)
        {
            this.Tidyup();
        }

                #region Cancel Command

        private RelayCommand cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new RelayCommand(this.CancelExecuted, this.CancelEnabled);
                }

                return this.cancelCommand;
            }
        }


        private void CancelExecuted(object o)
        {
            this.fsm.Fire(Trigger.Cancel);
            //BackgroundWorker
            //this.Wizard.Close();
            //this.Cancel();
        }

        private bool CancelEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

                #region Finish Command

        private RelayCommand finishCommand;

        public ICommand FinishCommand
        {
            get
            {
                if (this.finishCommand == null)
                {
                    this.finishCommand = new RelayCommand(this.FinishExecuted, this.FinishEnabled);
                }

                return this.finishCommand;
            }
        }


        private void FinishExecuted(object o)
        {
            this.fsm.Fire(Trigger.Finished);
            //this.Wizard.Close();
        }

        private bool FinishEnabled(object sender)
        {
            if (CheckWizard(false))
            {
                return false;
            }

            bool canExecute =
                fsm.State == State.ReadyToAccept;
            return canExecute;
        }
        #endregion

    }
}