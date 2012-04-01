namespace Jedzia.BackBock.ViewModel.Commands
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Relay Command for use with <see cref="ContextMenu"/>'s.
    /// </summary>
    public class RelayContextMenu : ICommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayContextMenu"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate that determines if the command can be executed.</param>
        public RelayContextMenu(Action<object, ContextMenuEventArgs> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayContextMenu"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public RelayContextMenu(Action<object, ContextMenuEventArgs> execute)
            : this(execute, null)
        {

        }

        #endregion

        #region ICommand Implementation

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            var lcanExecute = true;
            if (this.canExecute != null)
            {
                lcanExecute = this.canExecute(parameter);
            }
            return lcanExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            object[] parameters = (object[])parameter;
            this.execute(parameters[0], (ContextMenuEventArgs)parameters[1]);
        }

        #endregion

        #region Fields

        private readonly Action<object, ContextMenuEventArgs> execute;
        private readonly Predicate<object> canExecute;

        #endregion
    }
}