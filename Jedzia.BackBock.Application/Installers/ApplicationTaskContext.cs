namespace Jedzia.BackBock.Application.Installers
{
    using Jedzia.BackBock.Model;
    using Jedzia.BackBock.Tasks;

    internal class ApplicationTaskContext : TaskContext
    {
        #region Fields

        private ITaskService taskService;

        public ApplicationTaskContext(ITaskService taskService)
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