namespace Jedzia.BackBock.ViewModel
{
    using System.Windows.Input;

    public interface ICanInputBind
    {
        InputBindingCollection InputBindings { get; }
    }
}