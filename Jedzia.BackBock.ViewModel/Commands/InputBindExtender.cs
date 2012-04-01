namespace Jedzia.BackBock.ViewModel.Commands
{
    using System.Windows.Input;

    /// <summary>
    /// Extension methods for Command Bindings.
    /// </summary>
    public static class InputBindExtender
    {
        /// <summary>
        /// Adds the specified command binding to an <see cref="ICanCommandBind"/> object.
        /// </summary>
        /// <param name="binder">The element to bind the command to.</param>
        /// <param name="command">The command that gets bound.</param>
        /// <param name="executed">The <see cref="ExecutedRoutedEventHandler"/> that gets executed with command invocation.</param>
        /// <param name="canExecute">The <see cref="CanExecuteRoutedEventHandler"/> that determines if
        /// the command is allowed to execute.</param>
        public static void AddCommandBinding(this ICanCommandBind binder, ICommand command,
                                             ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            // Todo: insert null check guards.
            binder.CommandBindings.Add(new CommandBinding(command, executed, canExecute));
        }

        /// <summary>
        /// Adds the specified command binding to an <see cref="ICanCommandBind"/> object.
        /// </summary>
        /// <param name="binder">The element to bind the command to.</param>
        /// <param name="command">The command that gets bound.</param>
        /// <param name="executed">The <see cref="ExecutedRoutedEventHandler"/> that gets executed with command invocation.</param>
        public static void AddCommandBinding(this ICanCommandBind binder, ICommand command,
                                             ExecutedRoutedEventHandler executed)
        {
            binder.CommandBindings.Add(new CommandBinding(command, executed));
        }

        /// <summary>
        /// Adds the specified command binding to an <see cref="ICanCommandBind"/> object.
        /// </summary>
        /// <param name="binder">The element to bind the command to.</param>
        /// <param name="command">The command that gets bound.</param>
        public static void AddCommandBinding(this ICanCommandBind binder, ICommand command)
        {
            binder.CommandBindings.Add(new CommandBinding(command));
        }
    }
}