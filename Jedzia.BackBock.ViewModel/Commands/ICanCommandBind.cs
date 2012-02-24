namespace Jedzia.BackBock.ViewModel.Commands
{
    using System.Windows.Input;

    public interface ICanCommandBind
    {
        CommandBindingCollection CommandBindings { get; }
    }
}