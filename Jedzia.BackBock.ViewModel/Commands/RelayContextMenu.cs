namespace Jedzia.BackBock.ViewModel.Commands
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class RelayContextMenu : ICommand
    {
        #region Constructors

        public RelayContextMenu(Action<object, ContextMenuEventArgs> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayContextMenu(Action<object, ContextMenuEventArgs> execute)
            : this(execute, null)
        {

        }

        #endregion

        #region ICommand Implementation

        public bool CanExecute(object parameter)
        {
            var lcanExecute = true;
            if (this.canExecute != null)
            {
                lcanExecute = this.canExecute(parameter);
            }
            return lcanExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

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