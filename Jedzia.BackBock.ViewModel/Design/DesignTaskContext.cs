namespace Jedzia.BackBock.ViewModel.Design
{
    using Jedzia.BackBock.Model;
    using Jedzia.BackBock.Tasks;

    public class DesignTaskContext : TaskContext
    {
        #region Fields

        private ITaskService taskService;

        public DesignTaskContext(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        #endregion

        #region Properties

        public override ITaskService TaskService
        {
            get
            {
                return this.taskService;
            }
        }

        #endregion
    }
}