namespace Jedzia.BackBock.ViewModel
{
    using System.Windows.Input;

    public interface ICanCommandBind
    {
        CommandBindingCollection CommandBindings { get; }
    }
}