// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    using System.Collections.Generic;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Provides access to a <see cref="ITask"/> exposing service.
    /// </summary>
    public interface ITaskService //: IServiceProvider
    {
        #region Indexers

        /// <summary>
        /// Gets or sets the <see cref="ITask"/> at the specified index.
        /// </summary>
        /// <param name="taskName">The <paramref name="taskName"/> of the element to get.</param>
        /// <value>The <see cref="ITask"/> with the specified <paramref name="taskName"/>.</value>
        ITask this[string taskName] { get; /*set;*/ }

        #endregion

        /// <summary>
        /// Gets a list of the registered tasks.
        /// </summary>
        /// <returns>The list of the registered tasks.</returns>
        IEnumerable<string> GetRegisteredTasks();

        /// <summary>
        /// Registers the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns><c>true</c> if the task was successfully registered.</returns>
        bool Register(ITask task);

        /// <summary>
        /// Resets this instance to the default setup.
        /// </summary>
        void Reset();

        /// <summary>
        /// Resets this instance and clears all registered tasks.
        /// </summary>
        void ResetAll();
    }
}