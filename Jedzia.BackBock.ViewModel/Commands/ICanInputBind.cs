namespace Jedzia.BackBock.ViewModel.Commands
{
    using System.Windows.Input;

    public interface ICanInputBind
    {
        InputBindingCollection InputBindings { get; }
    }
}