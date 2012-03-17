using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.Security.Principal;

namespace Jedzia.BackBock.ViewModel.Data
{
    public interface IBackupDataService
    {
        BackupData GetBackupData(IPrincipal user);
    }
}
