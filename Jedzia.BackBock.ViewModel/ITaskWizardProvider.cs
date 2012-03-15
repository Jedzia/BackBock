namespace Jedzia.BackBock.ViewModel
{
    using Jedzia.BackBock.ViewModel.Wizard;

    public interface ITaskWizardProvider
    {
        IStateWizard GetWizard();
    }
}