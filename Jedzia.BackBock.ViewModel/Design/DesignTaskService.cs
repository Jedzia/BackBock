﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.ViewModel.Design
{
    using Microsoft.Build.Framework;

    class DesignTaskService : ITaskService
    {
        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITaskService Members

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

        public ITask this[string taskName]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
