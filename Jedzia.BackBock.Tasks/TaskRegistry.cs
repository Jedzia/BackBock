// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Build.Framework;

    /// <summary>
    /// A simple container for <see cref="ITask"/> items.
    /// </summary>
    public class TaskRegistry : ITaskService
    {
        // declare singleton field

        #region Fields

        private static TaskRegistry instance;
        private readonly Dictionary<string, Type> taskTypes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRegistry"/> class.
        /// </summary>
        static TaskRegistry()
        {
            GetInstance().Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRegistry"/> class.
        /// </summary>
        protected TaskRegistry()
        {
            this.taskTypes = new Dictionary<string, Type>();
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the <see cref="ITask"/> at the specified index.
        /// </summary>
        /// <param name="taskName">The <paramref name="taskName"/> of the element to get.</param>
        /// <value>The <see cref="ITask"/> with the specified <paramref name="taskName"/>.</value>
        public ITask this[string taskName]
        {
            get
            {
                Type ttask;
                if (taskName == null)
                {
                    return null;
                }
                bool found = this.taskTypes.TryGetValue(taskName, out ttask);
                if (found)
                {
                    return (ITask)Activator.CreateInstance(ttask);
                }
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Gets the instance for this singleton.
        /// </summary>
        /// <returns>the only <see cref="TaskRegistry"/> instance.</returns>
        public static ITaskService GetInstance()
        {
            return instance ?? (instance = new TaskRegistry());
        }

        /// <summary>
        /// Gets a list of the registered tasks.
        /// </summary>
        /// <returns>The list of the registered tasks.</returns>
        public IEnumerable<string> GetRegisteredTasks()
        {
            return this.taskTypes.Keys.ToArray();
        }

        /// <summary>
        /// Registers the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns><c>true</c> if the task was successfully registered.</returns>
        public bool Register(ITask task)
        {
            //var taskName = task.Name;
            var taskName = task.GetType().Name;
            var taskType = task.GetType();

            Type ttask;
            bool found = this.taskTypes.TryGetValue(taskName, out ttask);
            if (found)
            {
                return false;
            }

            this.RegisterInternal(taskName, taskType);
            return true;
        }

        /// <summary>
        /// Resets this instance to the default setup.
        /// </summary>
        public void Reset()
        {
            this.ResetAll();
            //Register(new Backup());
            this.RegisterInternal("Backup", typeof(Backup));
            this.RegisterInternal("Touch", typeof(Touch));
            this.RegisterInternal("Zip", typeof(Zip));
        }

        /// <summary>
        /// Resets this instance and clears all registered tasks.
        /// </summary>
        public void ResetAll()
        {
            this.taskTypes.Clear();
        }

        private void RegisterInternal(string taskName, Type taskType)
        {
            this.taskTypes.Add(taskName, taskType);
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