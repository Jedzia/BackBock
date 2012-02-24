namespace Jedzia.BackBock.ViewModel.Commands
{
    using System.Windows.Input;

    public static class InputBindExtender
    {
        public static void AddCommandBinding(this ICanCommandBind binder, ICommand pasteCommand,
                                             ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            // Todo: insert null check guards.
            // Todo: move to commands namespace.
            binder.CommandBindings.Add(new CommandBinding(pasteCommand, executed, canExecute));
        }

        public static void AddCommandBinding(this ICanCommandBind binder, ICommand pasteCommand,
                                             ExecutedRoutedEventHandler executed)
        {
            binder.CommandBindings.Add(new CommandBinding(pasteCommand, executed));
        }

        public static void AddCommandBinding(this ICanCommandBind binder, ICommand pasteCommand)
        {
            binder.CommandBindings.Add(new CommandBinding(pasteCommand));
        }
    }
}