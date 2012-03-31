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

        /// <summary>
        /// Gets the backup data for a given repository type.
        /// </summary>
        /// <param name="repotype">The repository type.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be null.</param>
        /// <returns>
        /// A set of Backup data.
        /// </returns>
        public BackupData GetBackupData(BackupRepositoryType repotype, System.Security.Principal.IPrincipal user, System.Collections.Specialized.StringDictionary parameters)
        {
            var dbr = new DesignBackupDataRepository();
            return dbr.GetBackupData();
        }

        /// <summary>
        /// Not implemented in Design Mode.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="user">The user.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Nothing, not implemented.
        /// </returns>
        public BackupData Load(string connection, System.Security.Principal.IPrincipal user, System.Collections.Specialized.StringDictionary parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the available endpoints that provide BackupData.
        /// </summary>
        public IEnumerable<string> LoadedServices
        {
            get { return new[] {  "Static: " + typeof(DesignBackupDataRepository).FullName }; }
        }

        #endregion
    }
}
