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

    class BaseWizard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BaseWizard"/> class.
        /// </summary>
        public BaseWizard()
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

            wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Next, State.Finish);
            wizardState.Configure(State.ReadyToAccept).Permit(Trigger.Cancel, State.Canceled);
            wizardState.Configure(State.Finish).Permit(Trigger.Previous, State.ReadyToAccept).OnEntry(t => Finished());

            wizardState.Configure(State.Canceled).OnEntry(t => Canceled());
        }

        public void Fire(Trigger trigger)
        {
            var oldState = wizardState.State;
            //Console.WriteLine(msg);
            wizardState.Fire(trigger);
            var msg = string.Format("[Firing:] {0}, Transition from {1} to {2}.", trigger, oldState, wizardState.State);
            MessageBox.Show(msg);
        }

        void Canceled()
        {
            var msg = string.Format("Canceled called [State:] {0}", wizardState.State);
            MessageBox.Show(msg);
        }

        void Finished()
        {
            var msg = string.Format("Finished called [State:] {0}", wizardState.State);
            MessageBox.Show(msg);
        }


    }
}
