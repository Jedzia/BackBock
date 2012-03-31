// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationTaskContext.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Application.Installers
{
    using Jedzia.BackBock.Model;
    using Jedzia.BackBock.Tasks;

    internal class ApplicationTaskContext : TaskContext
    {
        #region Fields

        private readonly ITaskService taskService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTaskContext"/> class.
        /// </summary>
        /// <param name="taskService">The task service.</param>
        public ApplicationTaskContext(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the task service of this instance.
        /// </summary>
        public override ITaskService TaskService
        {
            get
            {
                return this.taskService;
            }
        }

        #endregion
    }
}