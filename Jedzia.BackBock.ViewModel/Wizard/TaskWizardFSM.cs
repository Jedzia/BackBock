// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskWizardFSM.cs" company="EvePanix">
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
    using System.Windows;
    using Stateless;

    /// <summary>
    /// Triggers for the <see cref="TaskWizardFsm"/> state machine.
    /// </summary>
    internal enum Trigger
    {
        /// <summary>
        /// Introduction was read.
        /// </summary>
        IntroRead,

        /// <summary>
        /// A task was choosen.
        /// </summary>
        TaskChoosen,

        /// <summary>
        /// Folders are selected.
        /// </summary>
        FoldersSelected,

        /// <summary>
        /// Finished with the wizard.
        /// </summary>
        Finished,

        /// <summary>
        /// Wizard cancelled.
        /// </summary>
        Cancel,

        /// <summary>
        /// Wizard move to previous page.
        /// </summary>
        Previous,

        /// <summary>
        /// Wizard move to next page.
        /// </summary>
        Next
    }

    /// <summary>
    /// States of the <see cref="TaskWizardFsm"/> state machine.
    /// </summary>
    internal enum State
    {
        /// <summary>
        /// Starting state.
        /// </summary>
        Initial,

        /// <summary>
        /// At step: choose the task type.
        /// </summary>
        ChooseTaskType,

        /// <summary>
        /// At step: select the pats.
        /// </summary>
        SelectFolders,

        /// <summary>
        /// At step: ready to accept the wizards result.
        /// </summary>
        ReadyToAccept,

        /// <summary>
        /// At step: user has finished the wizard.
        /// </summary>
        Finish,

        /// <summary>
        /// At step: user has canceled the wizard.
        /// </summary>
        Canceled
    }

    /// <summary>
    /// <c>TaskWizard</c> finite state machine.
    /// </summary>
    internal class TaskWizardFsm
    {
        #region Fields

        /// <summary>
        /// The event handler for the <see cref="TaskWizardFsm.Canceled"/> event.
        /// </summary>
        private EventHandler<EventArgs> canceled;

        /// <summary>
        /// The event handler for the <see cref="TaskWizardFsm.Finished"/> event.
        /// </summary>
        private EventHandler<EventArgs> finished;

        private StateMachine<State, Trigger> wizardState;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWizardFsm"/> class.
        /// </summary>
        public TaskWizardFsm()
        {
            this.Init();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs on state Canceled.
        /// </summary>
        public event EventHandler<EventArgs> Canceled
        {
            add
            {
                this.canceled += value;
            }

            remove
            {
                this.canceled -= value;
            }
        }

        /// <summary>
        /// Occurs on the finished state. 
        /// </summary>
        public event EventHandler<EventArgs> Finished
        {
            add
            {
                this.finished += value;
            }

            remove
            {
                this.finished -= value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current state of this instance.
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

        #endregion

        /// <summary>
        /// Fires the specified trigger.
        /// </summary>
        /// <param name="trigger">The trigger to fire.</param>
        public void Fire(Trigger trigger)
        {
            var oldState = this.wizardState.State;

            // Console.WriteLine(msg);
            this.wizardState.Fire(trigger);
            var msg = string.Format(
                "[Firing:] {0}, Transition from {1} to {2}.", trigger, oldState, this.wizardState.State);
            MessageBox.Show(msg);
        }

        /// <summary>
        /// Raises the <see cref="Canceled"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCanceled(EventArgs e)
        {
            EventHandler<EventArgs> handler = Interlocked.CompareExchange(ref this.canceled, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            EventHandler<EventArgs> handler = Interlocked.CompareExchange(ref this.finished, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void CanceledTransition()
        {
            var msg = string.Format("Canceled called [State:] {0}", this.wizardState.State);
            MessageBox.Show(msg);
            this.OnCanceled(EventArgs.Empty);
        }

        private void FinishedTransition()
        {
            var msg = string.Format("Finished called [State:] {0}", this.wizardState.State);
            MessageBox.Show(msg);
            this.OnFinished(EventArgs.Empty);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Init()
        {
            this.wizardState = new StateMachine<State, Trigger>(State.Initial);

            this.wizardState.Configure(State.Initial).Permit(Trigger.Next, State.ChooseTaskType);
            this.wizardState.Configure(State.Initial).Permit(Trigger.Cancel, State.Canceled);
            this.wizardState.Configure(State.ChooseTaskType).Permit(Trigger.Previous, State.Initial);

            this.wizardState.Configure(State.ChooseTaskType).Permit(Trigger.Next, State.SelectFolders);
            this.wizardState.Configure(State.ChooseTaskType).Permit(Trigger.Cancel, State.Canceled);
            this.wizardState.Configure(State.SelectFolders).Permit(Trigger.Previous, State.ChooseTaskType);

            this.wizardState.Configure(State.SelectFolders).Permit(Trigger.Next, State.ReadyToAccept);
            this.wizardState.Configure(State.SelectFolders).Permit(Trigger.Cancel, State.Canceled);
            this.wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Previous, State.SelectFolders);

            this.wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Finished, State.Finish);
            this.wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Cancel, State.Canceled);
            this.wizardState.Configure(State.Finish).Permit(Trigger.Previous, State.ReadyToAccept);

            this.wizardState.Configure(State.Canceled).OnEntry(t => this.CanceledTransition());
            this.wizardState.Configure(State.Finish).OnEntry(t => this.FinishedTransition());
        }
    }
}