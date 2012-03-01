using Jedzia.BackBock.Tasks.Utilities;
namespace Jedzia.BackBock.Tasks
{
    public abstract class TaskExtension : Task
    {
        // Fields
        private TaskLoggingHelperExtension logExtension;

        // Methods
        internal TaskExtension()
            : base(AssemblyResources.PrimaryResources, "MSBuild.")
        {
            this.logExtension = new TaskLoggingHelperExtension(this, AssemblyResources.PrimaryResources, AssemblyResources.SharedResources, "MSBuild.");
        }

        /// <summary>
        /// Gets an instance of a <see cref="TaskLoggingHelperExtension"/> containing task logging methods.
        /// </summary>
        public TaskLoggingHelper Log
        {
            get
            {
                return this.logExtension;
            }
        }
    }
}