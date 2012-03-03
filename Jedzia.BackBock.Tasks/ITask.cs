// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    /// <summary>
    /// A unit of work.
    /// </summary>
    public interface ITask
    {
        #region Properties

        /// <summary>
        /// Gets the name of the Task.
        /// </summary>
        string Name { get; }

        IBuildEngine BuildEngine { get; set; }

        #endregion

        /// <summary>
        /// Executes a task.
        /// </summary>
        /// <returns><c>true</c> if the task executed successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method is called by the build engine to execute a task. Upon termination of this method,
        /// a task should indicate if the execution was successful. If a task throws an exception from
        /// this method, the engine assumes that the task has failed.
        /// </remarks>
        bool Execute();
    }
}