namespace Jedzia.BackBock.Application
{
    using System;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.Wizard;
    using Castle.Windsor;
    using Jedzia.BackBock.Application.Editors.TaskWizard;
    using Jedzia.BackBock.Application.Editors;

    public class ViewProvider : IViewProvider
    {
        //IWindsorContainer container;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskWizardProvider"/> class.
        /// </summary>
        public ViewProvider(/*IWindsorContainer container*/)
        {
            //this.container = container;
        }

        public IStateWizard GetWizard()
        {
            //ViewModelLocator.
            //return new TaskWizard();
            //return ServiceLocator.Current.GetInstance<IStateWizard>();
            return new TaskWizard();
            //return this.container.Resolve<IStateWizard>();
        }

        public IDialogWindow GetTaskEditor()
        {
            return new TaskEditorWindow();
        }
    }
}