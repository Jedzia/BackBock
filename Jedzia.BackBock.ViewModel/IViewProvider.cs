namespace Jedzia.BackBock.ViewModel
{
    using Jedzia.BackBock.ViewModel.Wizard;

    public interface IViewProvider
    {
        IStateWizard GetWizard();

        IDialogWindow GetTaskEditor();
    }
}