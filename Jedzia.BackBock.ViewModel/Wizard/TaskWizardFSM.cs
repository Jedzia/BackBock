using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;
using System.Windows;

namespace Jedzia.BackBock.ViewModel.Wizard
{
    enum Trigger
    {
        IntroRead,
        TaskChoosen,
        FoldersSelected,
        Finished,
        Cancel,
        Previous,
        Next
    }

    enum State
    {
        Initial,
        ChooseTaskType,
        SelectFolders,
        ReadyToAccept,
        Finish,
        Canceled
    }

    class TaskWizardFSM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BaseWizard"/> class.
        /// </summary>
        public TaskWizardFSM()
        {
            Init();
        }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public State State
        {
            get
            {
                return this.wizardState.State;
            }
            
            /*internal set
            {
                this.wizardState.State = value;
            }*/
        }

        StateMachine<State, Trigger> wizardState;
        private void Init()
        {
            wizardState = new StateMachine<State, Trigger>(State.Initial);

            wizardState.Configure(State.Initial).Permit(Trigger.Next, State.ChooseTaskType);
            wizardState.Configure(State.Initial).Permit(Trigger.Cancel, State.Canceled);
            wizardState.Configure(State.ChooseTaskType).Permit(Trigger.Previous, State.Initial);

            wizardState.Configure(State.ChooseTaskType).Permit(Trigger.Next, State.SelectFolders);
            wizardState.Configure(State.ChooseTaskType).Permit(Trigger.Cancel, State.Canceled);
            wizardState.Configure(State.SelectFolders).Permit(Trigger.Previous, State.ChooseTaskType);

            wizardState.Configure(State.SelectFolders).Permit(Trigger.Next, State.ReadyToAccept);
            wizardState.Configure(State.SelectFolders).Permit(Trigger.Cancel, State.Canceled);
            wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Previous, State.SelectFolders);

            wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Finished, State.Finish);
            wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Cancel, State.Canceled);
            wizardState.Configure(State.Finish).Permit(Trigger.Previous, State.ReadyToAccept);

            wizardState.Configure(State.Canceled).OnEntry(t => CanceledTransition());
            wizardState.Configure(State.Finish).OnEntry(t => FinishedTransition());
        }

        public void Fire(Trigger trigger)
        {
            var oldState = wizardState.State;
            //Console.WriteLine(msg);
            wizardState.Fire(trigger);
            var msg = string.Format("[Firing:] {0}, Transition from {1} to {2}.", trigger, oldState, wizardState.State);
            MessageBox.Show(msg);
        }

        void CanceledTransition()
        {
            var msg = string.Format("Canceled called [State:] {0}", wizardState.State);
            MessageBox.Show(msg);
            OnCanceled(EventArgs.Empty);
        }

        void FinishedTransition()
        {
            var msg = string.Format("Finished called [State:] {0}", wizardState.State);
            MessageBox.Show(msg);
            OnFinished(EventArgs.Empty);
        }

        /// <summary>
        /// The event handler for the <see cref="E:TaskWizardFSM.Canceled"/> event.
        /// </summary>
        private EventHandler<EventArgs> canceled;

        /// <summary>
        /// Occurs when 
        /// </summary>
        public event EventHandler<EventArgs> Canceled
        {
            add
            {
                // TODO: write your implementation of the add accessor here
                this.canceled += value;
            }

            remove
            {
                // TODO: write your implementation of the remove accessor here
                this.canceled -= value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCanceled(EventArgs e)
        {
            EventHandler<EventArgs> handler = System.Threading.Interlocked.CompareExchange(ref this.canceled, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }


        /// <summary>
        /// The event handler for the <see cref="E:TaskWizardFSM.Finished"/> event.
        /// </summary>
        private EventHandler<EventArgs> finished;

        /// <summary>
        /// Occurs when 
        /// </summary>
        public event EventHandler<EventArgs> Finished
        {
            add
            {
                // TODO: write your implementation of the add accessor here
                this.finished += value;
            }

            remove
            {
                // TODO: write your implementation of the remove accessor here
                this.finished -= value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            EventHandler<EventArgs> handler = System.Threading.Interlocked.CompareExchange(ref this.finished, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
