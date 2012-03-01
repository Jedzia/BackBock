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
    }

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
        // Get instance
        public static TaskRegistry GetInstance()
        {
            if (instance == null) instance = new TaskRegistry();

            return instance;
        }

                /// <summary>
        /// Initializes a new instance of the <see cref="T:BackupTask"/> class.
        /// </summary>
        static TaskRegistry()
        {
            TaskRegistry.GetInstance().Register(new BackupTask());
        }

        public bool Register(ITask task )
        {
            Type ttask;
            bool found = taskTypes.TryGetValue(task.Name, out ttask);
            if (found)
            {
                return false;
            }

            taskTypes.Add(task.Name, task.GetType());
            return true;
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
