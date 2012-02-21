using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Tasks
{
    public class TaskRegistry
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
	
    }
}
