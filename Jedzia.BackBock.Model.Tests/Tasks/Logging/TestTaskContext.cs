// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTaskContext.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Tests.Tasks.Logging
{
    using System;
    using System.Collections.Generic;
    using Jedzia.BackBock.Tasks;
    using Microsoft.Build.Framework;

    public class TestTaskContext : TaskContext
    {
        #region Fields

        private readonly ITaskService taskService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestTaskContext"/> class.
        /// </summary>
        public TestTaskContext(ITask theOnlyOne)
        {
            this.taskService = new TestTaskService(theOnlyOne);
        }

        #endregion

        #region Properties

        public override ITaskService TaskService
        {
            get
            {
                return this.taskService;
            }
        }

        #endregion

        #region Nested type: TestTaskService

        public class TestTaskService : ITaskService
        {
            #region Fields

            private readonly ITask task;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="T:TestTaskService"/> class.
            /// </summary>
            public TestTaskService(ITask theOnlyOne)
            {
                this.task = theOnlyOne;
            }

            #endregion

            #region Indexers

            public ITask this[string taskName]
            {
                get
                {
                    return this.task;
                }
            }

            #endregion

            public IEnumerable<string> GetRegisteredTasks()
            {
                throw new NotImplementedException();
            }

            public bool Register(ITask task)
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public void ResetAll()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}