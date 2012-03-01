using Jedzia.BackBock.Tasks.Utilities;
namespace Jedzia.BackBock.Tasks
{
    /// <summary>
    /// Contains properties to help extend a task.
    /// </summary>
    public abstract class TaskExtension : Task
    {
        private readonly TaskLoggingHelperExtension logExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExtension"/> class.
        /// </summary>
        internal TaskExtension()
            : base(AssemblyResources.PrimaryResources, "MSBuild.")
        {
            this.logExtension = new TaskLoggingHelperExtension(this, AssemblyResources.PrimaryResources, AssemblyResources.SharedResources, "MSBuild.");
        }

        /// <summary>
        /// Gets an instance of a <see cref="TaskLoggingHelperExtension"/> containing task logging methods.
        /// </summary>
        public new TaskLoggingHelper Log
        {
            get
            {
                return this.logExtension;
            }
        }
    }
}