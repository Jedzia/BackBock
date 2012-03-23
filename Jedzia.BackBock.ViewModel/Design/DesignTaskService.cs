using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.ViewModel.Design
{
    using Microsoft.Build.Framework;

    public class DesignTaskService : ITaskService
    {
        private const string error = "DesignTaskService does not provide executable Operations!";
        
        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException(error);
        }

        #endregion

        #region ITaskService Members

        public IEnumerable<string> GetRegisteredTasks()
        {
            throw new NotImplementedException(error);
        }

        public bool Register(ITask task)
        {
            throw new NotImplementedException(error);
        }

        public void Reset()
        {
            throw new NotImplementedException(error);
        }

        public void ResetAll()
        {
            throw new NotImplementedException(error);
        }

        public ITask this[string taskName]
        {
            get { throw new NotImplementedException(error); }
        }

        #endregion
    }
}
