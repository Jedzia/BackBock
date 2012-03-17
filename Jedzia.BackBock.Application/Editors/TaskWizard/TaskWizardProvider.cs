namespace Jedzia.BackBock.Application.Editors.TaskWizard
{
    using System;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Castle.Windsor;

    public class TaskWizardProvider : ITaskWizardProvider
    {
        IWindsorContainer container;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskWizardProvider"/> class.
        /// </summary>
        public TaskWizardProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        public IStateWizard GetWizard()
        {
            //ViewModelLocator.
            //return new TaskWizard();
            //return ServiceLocator.Current.GetInstance<IStateWizard>();
            return this.container.Resolve<IStateWizard>();
        }
    }
}