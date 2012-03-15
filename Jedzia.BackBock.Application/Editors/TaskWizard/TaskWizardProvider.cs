namespace Jedzia.BackBock.Application.Editors.TaskWizard
{
    using System;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Microsoft.Practices.ServiceLocation;

    public class TaskWizardProvider : ITaskWizardProvider
    {
        public IStateWizard GetWizard()
        {
            //ViewModelLocator.
            //return new TaskWizard();
            return ServiceLocator.Current.GetInstance<IStateWizard>();
        }
    }
}