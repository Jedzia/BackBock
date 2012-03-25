using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.Security.Principal;
using System.Collections.Specialized;
using Jedzia.BackBock.DataAccess;

namespace Jedzia.BackBock.DataAccess
{
    public interface IBackupDataService
    {

        //BackupRepositoryType ServiceType { get; }
        BackupData GetBackupData(BackupRepositoryType repotype, IPrincipal user, StringDictionary parameters);
        BackupData Load(string filename, IPrincipal user, StringDictionary parameters);
        IEnumerable<string> LoadedServices { get; }
    }
}
