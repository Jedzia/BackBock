using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.ViewModel.Design
{
    class DesignTaskService : ITaskService
    {
        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITaskService Members

        public bool Register(ITask task)
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
