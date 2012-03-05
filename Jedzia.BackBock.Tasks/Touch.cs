namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.ComponentModel;
    using Microsoft.Build.Tasks;

    /// <summary>
    /// A simple backup task.
    /// </summary>
    [DisplayName("Touch Task")]
    public class Touch : TaskExtension
    {
        /// <summary>
        /// Gets the name of the Task.
        /// </summary>
        /*public override string Name
        {
            get
            {
                return "Touch";
            }
        }*/

        /// <summary>
        /// Executes a task.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the task executed successfully; otherwise, <c>false</c>.
        /// </returns>
        public override bool Execute()
        {
            throw new NotImplementedException();
            //return false;
        }
    }
}