using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.Data;
using Jedzia.BackBock.Model.Data;
using Jedzia.BackBock.DataAccess;

namespace Jedzia.BackBock.ViewModel.Design.Data
{
    /// <summary>
    /// Skips the usually steps of BackupDataRepository injection for Design Time.
    /// </summary>
    public class DesignBackupDataService : IBackupDataService
    {
        #region IBackupDataService Members

        public BackupData GetBackupData(BackupRepositoryType repotype, System.Security.Principal.IPrincipal user, System.Collections.Specialized.StringDictionary parameters)
        {
            var dbr = new DesignBackupDataRepository();
            return dbr.GetBackupData();
        }

        #endregion

        #region IBackupDataService Members


        public BackupData Load(string connection, System.Security.Principal.IPrincipal user, System.Collections.Specialized.StringDictionary parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> LoadedServices
        {
            get { return new[] {  "Static: " + typeof(DesignBackupDataRepository).FullName }; }
        }

        #endregion
    }
}
