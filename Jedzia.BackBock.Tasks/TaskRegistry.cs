using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Jedzia.BackBock.Tasks
{
    public interface ITaskService //: IServiceProvider
    {
        bool Register(ITask task);
        ITask this[string taskName] { get; /*set;*/ }
        void Reset();
        void ResetAll();
        IEnumerable<string> GetRegisteredTasks();
}

    /// <summary>
    /// A simple container for ITask items.
    /// </summary>
    public class TaskRegistry : ITaskService
    {
        private readonly Dictionary<string, Type> taskTypes;

        // declare singleton field
        private static TaskRegistry instance = null;
        // Protected constructor.
        protected TaskRegistry()
        {
            taskTypes = new Dictionary<string, Type>();
        }

        /// <summary>
        /// Gets the instance for this singleton.
        /// </summary>
        /// <returns>the only TaskRegistry instance.</returns>
        public static ITaskService GetInstance()
        {
            if (instance == null) instance = new TaskRegistry();

            return instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskRegistry"/> class.
        /// </summary>
        static TaskRegistry()
        {
            TaskRegistry.GetInstance().Reset();
        }

        /// <summary>
        /// Registers the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns><c>true</c> if the task was sucessfully registered.</returns>
        public bool Register(ITask task)
        {
            var taskName = task.Name;
            var taskType = task.GetType();

            Type ttask;
            bool found = taskTypes.TryGetValue(taskName, out ttask);
            if (found)
            {
                return false;
            }

            RegisterInternal(taskName, taskType);
            return true;
        }

        private void RegisterInternal(string taskName, Type taskType)
        {
            taskTypes.Add(taskName, taskType);
        }

        /// <summary>
        /// Gets a list of the registered tasks.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetRegisteredTasks()
        {
            return taskTypes.Keys.ToArray();
        }

        /// <summary>
        /// Resets this instance to the default setup.
        /// </summary>
        public void Reset()
        {
            ResetAll();
            //Register(new BackupTask());
            RegisterInternal("Backup", typeof(BackupTask));
        }

        /// <summary>
        /// Resets this instance and clears all registered tasks.
        /// </summary>
        public void ResetAll()
        {
            taskTypes.Clear();
        }


        /// <summary>
        /// Gets or sets the <see cref="T:ITask"/> at the specified index.
        /// </summary>
        /// <param name="taskName">The taskName of the element to get.</param>
        /// <value>The <see cref="T:ITask"/> with the specified taskName.</value>
        public ITask this[string taskName]
        {
            get
            {
                Type ttask;
                bool found = taskTypes.TryGetValue(taskName, out ttask);
                if (found)
                {
                    return (ITask)Activator.CreateInstance(ttask);
                }
                return null;
            }
        }

        /*#region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (serviceType is ITaskService) 
            {
                return this;
            }
            return null;
        }

        #endregion*/
    }
}
