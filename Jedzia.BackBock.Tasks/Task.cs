// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    using System.Resources;
    using Jedzia.BackBock.Tasks.Utilities;
    using System.ComponentModel;

    /// <summary>
    /// When overridden in a derived form, provides functionality for tasks.
    /// </summary>
    public abstract class Task : ITask
    {
        // Fields

        #region Fields

        private readonly TaskLoggingHelper log;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        protected Task()
        {
            this.log = new TaskLoggingHelper(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with the 
        /// specified <see cref="TaskResources"/>. 
        /// </summary>
        /// <param name="taskResources">The task resources.</param>
        /// <remarks>
        /// This constructor allows derived task classes to register their resources.
        /// </remarks>
        protected Task(ResourceManager taskResources)
            : this()
        {
            this.log.TaskResources = taskResources;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with the 
        /// specified <see cref="TaskResources"/> and <see cref="HelpKeywordPrefix"/>. 
        /// </summary>
        /// <param name="taskResources">The task resources.</param>
        /// <param name="helpKeywordPrefix">The prefix to append to string resources to create Help keywords.</param>
        /// <remarks>
        /// This constructor allows derived task classes to register their resources, as well as provide 
        /// a prefix for composing help keywords from string resource names. If <paramref name="helpKeywordPrefix"/> is an
        /// empty string, string resource names will be used verbatim as help keywords. 
        /// </remarks>
        protected Task(ResourceManager taskResources, string helpKeywordPrefix)
            : this(taskResources)
        {
            this.log.HelpKeywordPrefix = helpKeywordPrefix;
        }

        #endregion

        // Properties

        #region Properties

        /// <summary>
        /// Gets or sets the instance of the <see cref="IBuildEngine"/> object used by the task. 
        /// </summary>
        /// <value>
        /// The <see cref="IBuildEngine"/> available to tasks. 
        /// </value>
        /// <remarks>The build engine automatically sets this property to allow tasks to call
        /// back into it.</remarks>
        [Browsable(false)]
        public IBuildEngine BuildEngine { get; set; }

        /// <summary>
        /// Gets or sets the instance of the <see cref="IBuildEngine2"/> object used by the task. 
        /// </summary>
        /// <value>
        /// The <see cref="IBuildEngine2"/> available to tasks. 
        /// </value>
        /// <remarks>The build engine automatically sets this property to allow tasks to call
        /// back into it.</remarks>
        [Browsable(false)]
        public IBuildEngine2 BuildEngine2
        {
            get
            {
                return (IBuildEngine2)this.BuildEngine;
            }
        }

        /// <summary>
        /// Gets or sets the host object associated with the task.
        /// </summary>
        /// <value>
        /// The host object associated with the task. This value can be <c>null</c>.
        /// </value>
        /// <remarks>If the host IDE has associated a host object with this particular task, then the build
        /// engine sets this property. The host object is provided by HostServices. Visual Studio determines
        /// the host object via a system registry key. For more information, see IVsMSBuildHostObject.
        /// </remarks>
        [Browsable(false)]
        public ITaskHost HostObject { get; set; }

        /// <summary>
        /// Gets an instance of a <see cref="TaskLoggingHelper"/> class containing task logging methods.
        /// </summary>
        [Browsable(false)]
        public TaskLoggingHelper Log
        {
            get
            {
                return this.log;
            }
        }

        /// <summary>
        /// Gets the name of the Task.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the prefix used to compose Help keywords from resource names.
        /// </summary>
        /// <value>
        /// The prefix used to compose Help keywords from resource names.
        /// </value>
        protected string HelpKeywordPrefix
        {
            get
            {
                return this.Log.HelpKeywordPrefix;
            }
            set
            {
                this.Log.HelpKeywordPrefix = value;
            }
        }

        /// <summary>
        /// Gets or sets the culture-specific resources associated with the task.
        /// </summary>
        /// <value>
        /// The culture-specific resources associated with the task. This value can be <c>null</c>.
        /// </value>
        /// <remarks>
        /// If derived classes have localized strings, then they should register their 
        /// resources either during construction, or through this property.
        /// </remarks>
        [Browsable(false)]
        protected ResourceManager TaskResources
        {
            get
            {
                return this.Log.TaskResources;
            }
            set
            {
                this.Log.TaskResources = value;
            }
        }

        #endregion

        /// <summary>
        /// Executes a task.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the task executed successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This class must be overridden in a derived class, and is used for performing task execution logic.
        /// </remarks>
        public abstract bool Execute();
    }

    /// <summary>
    /// Provides a way for task authors to use a subset of the functionality of the MSBuild engine.
    /// </summary>
    public interface IBuildEngine2
    {
    }

    /// <summary>
    /// Passes host objects from an integrated development environment (IDE) to individual tasks.
    /// </summary>
    public interface ITaskHost
    {
    }

    /// <summary>
    /// Provides a way for task authors to use a subset of the functionality of the MSBuild engine.
    /// </summary>
    public interface IBuildEngine
    {
        void LogMessageEvent(BuildMessageEventArgs e);
    }

    public class SimpleBuildEngine : IBuildEngine
    {

        #region IBuildEngine Members

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            System.Console.WriteLine(
                e.Timestamp + ":[" + e.ThreadId + "." + e.SenderName + "]" + e.Message + e.HelpKeyword);
        }

        #endregion
    }
}