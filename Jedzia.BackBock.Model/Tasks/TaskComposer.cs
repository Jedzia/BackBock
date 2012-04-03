namespace Jedzia.BackBock.Model.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Microsoft.Build.BuildEngine;

    /// <summary>
    /// Provides creation of <see cref="ITaskComposer"/>'s and <see cref="ItemGroupComposer"/>
    /// to help in <see cref="ITask"/> setup.
    /// </summary>
    public interface ITaskComposerFactory
    {
        /// <summary>
        /// Creates a new instance of an <see cref="ITaskComposer"/> class
        /// from the specified parameters.
        /// </summary>
        /// <param name="project">The build project the task should use.</param>
        /// <param name="taskType">Type of the task to compose.</param>
        /// <param name="buildLogger">The logger to use. Can be null.</param>
        /// <returns>A new <see cref="ITaskComposer"/> with the specified parameters.</returns>
        ITaskComposer CreateTaskComposer(Project project, Type taskType, IBuildLogger buildLogger);

        /// <summary>
        /// Creates a new instance of an <see cref="ItemGroupComposer"/> class
        /// from the specified parameters.
        /// </summary>
        /// <returns>A new <see cref="ItemGroupComposer"/> with the specified parameters.</returns>
        ItemGroupComposer CreateItemGroupComposer();
    }

    /// <summary>
    /// Defines methods to help in <see cref="ITask"/> creation for the <see cref="TaskSetupEngine"/>.
    /// </summary>
    public interface ITaskComposer
    {
        /// <summary>
        /// Creates and Adds a new task on a build target.
        /// </summary>
        /// <param name="target">The build target.</param>
        /// <param name="sourceParameterIdentifier">The source parameter identifier used to name the 
        /// ItemGroup with source files.</param>
        /// <param name="sourceParameter">The source parameter.</param>
        /// <returns>
        /// A new task ready to use.
        /// </returns>
        BuildTask CreateNewTaskOnTarget(Target target, string sourceParameterIdentifier, string sourceParameter);

        /// <summary>
        /// Sets the specified parameters on the task, that was created by 
        /// the <see cref="CreateNewTaskOnTarget"/> method.
        /// </summary>
        /// <param name="taskAttributes">The list of Xml attributes with the parameters of the task.</param>
        void SetParametersOnCreatedTask(IEnumerable<XmlAttribute> taskAttributes);
    }

    /// <summary>
    /// A factory to help in the creation of <see cref="ITaskComposer"/>'s and <see cref="ItemGroupComposer"/>'s.
    /// </summary>
    internal class TaskComposerFactory : ITaskComposerFactory
    {
        /// <summary>
        /// Creates a new instance of an <see cref="ITaskComposer"/> class
        /// from the specified parameters.
        /// </summary>
        /// <param name="project">The build project the task should use.</param>
        /// <param name="taskType">Type of the task to compose.</param>
        /// <param name="buildLogger">The logger to use. Can be null.</param>
        /// <returns>
        /// A new <see cref="ITaskComposer"/> with the specified parameters.
        /// </returns>
        public ITaskComposer CreateTaskComposer(Project project, Type taskType, IBuildLogger buildLogger)
        {
            return new TaskComposer(project, taskType, buildLogger);
        }

        /// <summary>
        /// Creates a new instance of an <see cref="ItemGroupComposer"/> class
        /// from the specified parameters.
        /// </summary>
        /// <returns>
        /// A new <see cref="ItemGroupComposer"/> with the specified parameters.
        /// </returns>
        public ItemGroupComposer CreateItemGroupComposer()
        {
            return new FileItemGroupComposer();
        }
    }

    /// <summary>
    /// Implementation of a simple <see cref="ITaskComposer"/> task creation helper.
    /// </summary>
    internal class TaskComposer : ITaskComposer
    {
        private readonly Type taskType;
        private readonly Project project;
        private readonly IBuildLogger buildLogger;

        /// <summary>
        /// The default source parameter.
        /// </summary>
        //private string defaultSourceParameterIdentifier = @"@(FilesToZip)";
        private BuildTask composedTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskComposer"/> class.
        /// </summary>
        /// <param name="project">The build project the task should use.</param>
        /// <param name="taskType">Type of the task to compose.</param>
        public TaskComposer(Project project, Type taskType)
            : this(project, taskType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskComposer"/> class
        /// with a logger.
        /// </summary>
        /// <param name="project">The build project the task should use.</param>
        /// <param name="taskType">Type of the task to compose.</param>
        /// <param name="buildLogger">The logger to use. Can be null.</param>
        public TaskComposer(Project project, Type taskType, IBuildLogger buildLogger)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            this.project = project;

            if (taskType == null)
            {
                throw new ArgumentNullException("taskType");
            }

            this.taskType = taskType;
            this.buildLogger = buildLogger;

            project.AddNewUsingTaskFromAssemblyName(this.taskType.FullName, this.taskType.Assembly.FullName);
        }

        /*/// <summary>
        /// Gets or sets the default source parameter identifier.
        /// </summary>
        /// <value>The default source parameter identifier.</value>
        public string DefaultSourceParameterIdentifier
        {
            get
            {
                return this.defaultSourceParameterIdentifier;
            }

            set
            {
                this.defaultSourceParameterIdentifier = value;
            }
        }*/

        /// <summary>
        /// Creates and Adds a new task on a build target.
        /// </summary>
        /// <param name="target">The build target.</param>
        /// <param name="sourceParameterIdentifier">The source parameter identifier used to name the
        /// ItemGroup with source files.</param>
        /// <param name="sourceParameter">The source parameter.</param>
        /// <returns>
        /// A new task ready to use.
        /// </returns>
        public BuildTask CreateNewTaskOnTarget(
            Target target, 
            string sourceParameterIdentifier,  
            string sourceParameter)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            // Compose the new task.
            var batask = target.AddNewTask(this.taskType.FullName);

            // Set the main source parameter on the build task.
            //batask.SetParameterValue(sourceParameter, this.DefaultSourceParameterIdentifier);
            var spi = @"@(" + sourceParameterIdentifier + ")";
            batask.SetParameterValue(sourceParameter, spi);

            // Store for later use with SetParametersOnCreatedTask( ... ).
            this.composedTask = batask;
            return batask;
        }

        /// <summary>
        /// Sets the specified parameters on the task, that was created by 
        /// the <see cref="CreateNewTaskOnTarget"/> method.
        /// </summary>
        /// <param name="taskAttributes">The list of Xml attributes with the parameters of the task.</param>
        public void SetParametersOnCreatedTask(IEnumerable<XmlAttribute> taskAttributes)
        {
            if (taskAttributes == null)
            {
                throw new ArgumentNullException("taskAttributes");
            }

            if (this.composedTask == null)
            {
                throw new InvalidOperationException("You'll have to create a task first. Use " +
                    "TaskComposer.CreateNewTaskOnTarget(...) before calling TaskComposer.SetParametersOnCreatedTask(...)");
            }

            var batask = this.composedTask;

            // Set parameter values on the BuildTask.
            foreach (var item in taskAttributes)
            {
                batask.SetParameterValue(item.Name, item.Value);
                var message = string.Format("Setting parameter {0} to {1}", item.Name, item.Value);
                LogBuildMessage(message);
            }
        }

        private void LogBuildMessage(string message)
        {
            if (this.buildLogger != null)
            {
                this.buildLogger.LogBuildMessage(this, this.taskType.Name, message);
            }
        }
    }
}
