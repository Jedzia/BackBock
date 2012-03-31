// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBackupDataService.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.DataAccess
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using Jedzia.BackBock.Model.Data;

    /// <summary>
    /// Data provider for backup sets.
    /// </summary>
    public interface IBackupDataService
    {
        // BackupRepositoryType ServiceType { get; }
        #region Properties

        /// <summary>
        /// Gets the available endpoints that provide <see cref="BackupData"/>.
        /// </summary>
        IEnumerable<string> LoadedServices { get; }

        #endregion

        /// <summary>
        /// Gets the backup data for a given repository type.
        /// </summary>
        /// <param name="repotype">The repository type.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be <c>null</c>.</param>
        /// <returns>A set of Backup data.</returns>
        BackupData GetBackupData(BackupRepositoryType repotype, IPrincipal user, StringDictionary parameters);

        /// <summary>
        /// Loads the specified <see cref="BackupData"/> by filename.
        /// </summary>
        /// <param name="filename">The path to the stored <see cref="BackupData"/> on disk.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be <c>null</c>.</param>
        /// <returns>A set of Backup data.</returns>
        BackupData Load(string filename, IPrincipal user, StringDictionary parameters);
    }
}