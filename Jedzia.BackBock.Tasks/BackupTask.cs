using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Tasks
{
    public class BackupTask : ITask
    {


        #region ITask Members

        public string Name
        {
            get { return "Backup"; }
        }

        public bool Execute()
        {
            return true;
        }
        #endregion
    }
}
