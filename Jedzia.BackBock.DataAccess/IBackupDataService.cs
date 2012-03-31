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
    /// <summary>
    /// Data provider for backup sets.
    /// </summary>
    public interface IBackupDataService
    {

        //BackupRepositoryType ServiceType { get; }
        
        /// <summary>
        /// Gets the backup data for a given repository type.
        /// </summary>
        /// <param name="repotype">The repository type.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be null.</param>
        /// <returns>A set of Backup data.</returns>
        BackupData GetBackupData(BackupRepositoryType repotype, IPrincipal user, StringDictionary parameters);

        /// <summary>
        /// Loads the specified <see cref="BackupData"/> by filename.
        /// </summary>
        /// <param name="filename">The path to the stored BackupData on disk.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be null.</param>
        /// <returns>A set of Backup data.</returns>
        BackupData Load(string filename, IPrincipal user, StringDictionary parameters);

        /// <summary>
        /// Gets the available endpoints that provide BackupData.
        /// </summary>
        IEnumerable<string> LoadedServices { get; }
    }
}
