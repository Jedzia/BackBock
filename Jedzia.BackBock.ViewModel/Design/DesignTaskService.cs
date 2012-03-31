// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignTaskService.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Design
{
    using System;
    using System.Collections.Generic;
    using Jedzia.BackBock.Tasks;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Design-Time implementation of an <see cref="ITaskService"/>.
    /// </summary>
    public class DesignTaskService : ITaskService
    {
        #region Fields

        private const string ERROR = "DesignTaskService does not provide executable Operations!";

        #endregion

        #region Indexers

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>DesignTaskService</c> does not provide executable Operations!</exception>
        public ITask this[string taskName]
        {
            get
            {
                throw new NotImplementedException(ERROR);
            }
        }

        #endregion

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>DesignTaskService</c> does not provide executable Operations!</exception>
        public IEnumerable<string> GetRegisteredTasks()
        {
            throw new NotImplementedException(ERROR);
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>DesignTaskService</c> does not provide executable Operations!</exception>
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException(ERROR);
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>DesignTaskService</c> does not provide executable Operations!</exception>
        public bool Register(ITask task)
        {
            throw new NotImplementedException(ERROR);
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>DesignTaskService</c> does not provide executable Operations!</exception>
        public void Reset()
        {
            throw new NotImplementedException(ERROR);
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>DesignTaskService</c> does not provide executable Operations!</exception>
        public void ResetAll()
        {
            throw new NotImplementedException(ERROR);
        }
    }
}