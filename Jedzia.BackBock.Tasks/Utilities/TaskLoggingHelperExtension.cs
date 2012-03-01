namespace Jedzia.BackBock.Tasks.Utilities
{
    using System.Resources;

    public class TaskLoggingHelperExtension : TaskLoggingHelper
    {
        // Fields
        private ResourceManager taskSharedResources;

        // Methods
        private TaskLoggingHelperExtension()
             : base(null)
        {
        }

        public TaskLoggingHelperExtension(ITask taskInstance, ResourceManager primaryResources, ResourceManager sharedResources, string helpKeywordPrefix)
            : base(taskInstance)
        {
            TaskResources = primaryResources;
            this.TaskSharedResources = sharedResources;
            HelpKeywordPrefix = helpKeywordPrefix;
        }

        public override string FormatResourceString(string resourceName, params object[] args)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(resourceName, "resourceName");
            //ErrorUtilities.VerifyThrowInvalidOperation(base.TaskResources != null, "Shared.TaskResourcesNotRegistered", base.TaskName);
            //ErrorUtilities.VerifyThrowInvalidOperation(this.TaskSharedResources != null, "Shared.TaskResourcesNotRegistered", base.TaskName);
            //string unformatted = base.TaskResources.GetString(resourceName, CultureInfo.CurrentUICulture);
            //if (unformatted == null)
            //{
            //    unformatted = this.TaskSharedResources.GetString(resourceName, CultureInfo.CurrentUICulture);
            // }
            //ErrorUtilities.VerifyThrowArgument(unformatted != null, "Shared.TaskResourceNotFound", resourceName, base.TaskName);
            //return this.FormatString(unformatted, args);
            return resourceName;
        }

        // Properties
        public ResourceManager TaskSharedResources
        {
            get
            {
                return this.taskSharedResources;
            }
            set
            {
                this.taskSharedResources = value;
            }
        }
    }
}