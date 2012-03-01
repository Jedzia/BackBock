namespace Jedzia.BackBock.Tasks.Utilities
{
    using System.Resources;

    /// <summary>
    /// Enables logging of various messages. Also, enables loading and formatting of resources.
    /// </summary>
    public class TaskLoggingHelperExtension : TaskLoggingHelper
    {
        private ResourceManager taskSharedResources;

        /// <summary>
        /// Prevents a default instance of the <see cref="TaskLoggingHelperExtension"/> class from being created.
        /// </summary>
        private TaskLoggingHelperExtension()
             : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLoggingHelperExtension"/> class 
        /// with the task instance, primary resources, shared resources, and a Help keyword prefix..
        /// </summary>
        /// <param name="taskInstance">A task containing an instance of the TaskLoggingHelperExtension class.</param>
        /// <param name="primaryResources">UI and string resources.</param>
        /// <param name="sharedResources">Shared UI and string resources.</param>
        /// <param name="helpKeywordPrefix">The prefix for composing Help keywords.</param>
        public TaskLoggingHelperExtension(ITask taskInstance, ResourceManager primaryResources, ResourceManager sharedResources, string helpKeywordPrefix)
            : base(taskInstance)
        {
            TaskResources = primaryResources;
            this.TaskSharedResources = sharedResources;
            HelpKeywordPrefix = helpKeywordPrefix;
        }

        /// <summary>
        /// Loads the specified resource string and optionally formats it using the given arguments. 
        /// The current thread's culture is used for formatting.
        /// </summary>
        /// <param name="resourceName">The name of the string resource to load.</param>
        /// <param name="args">Optional arguments for formatting the loaded string, or <c>null</c>.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks>
        /// This method requires the owner task to have registered its resources either via the 
        /// <see cref="Task"/> base class constructor, or one of the following properties: TaskResources or TaskResources.
        /// </remarks>
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
        /// <summary>
        /// Gets or sets the shared UI and string resources.
        /// </summary>
        /// <value>
        /// The shared UI and string resources.
        /// </value>
        /// <remarks>
        /// This property is used to load culture-specific resources. Derived classes should register 
        /// their resources either during construction, or via this property, if they have localized strings.
        /// </remarks>
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