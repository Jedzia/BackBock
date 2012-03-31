namespace Jedzia.BackBock.Model
{
    using Jedzia.BackBock.Tasks;

    /// <summary>
    /// Default implementation of a <see cref="TaskContext"/>.
    /// </summary>
    internal class DefaultTaskContext : TaskContext
    {
        #region Fields

        /*/// <summary>
        /// Holds the default task service.
        /// </summary>
        private ITaskService taskService;*/

        #endregion

        #region Properties

        /// <summary>
        /// Gets the task service of this instance.
        /// </summary>
        public override ITaskService TaskService
        {
            get
            {
                // return this.taskService ?? (this.taskService = TaskRegistry.GetInstance());
                return TaskRegistry.GetInstance();
            }
        }

        #endregion
    }
}