// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDataRepository.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.DataAccess
{
    using Jedzia.BackBock.DataAccess.DTO;

    /// <summary>
    /// A repository that provides backup data.
    /// </summary>
    public abstract class BackupDataRepository
    {
        #region Properties

        /// <summary>
        /// Gets the type of the repository.
        /// </summary>
        /// <value>
        /// The type of the repository.
        /// </value>
        public abstract BackupRepositoryType RepositoryType { get; }

        #endregion

        /// <summary>
        /// Gets the backup data.
        /// </summary>
        /// <returns>The backup data.</returns>
        public abstract BackupData GetBackupData();

        /*public abstract void DeleteBackup(int id);

            public abstract void InsertBackup(BackupData backup);

            public abstract BackupData SelectBackup(int id);

            public abstract IEnumerable<BackupData> SelectAllBackups();

            public abstract void UpdateBackup(BackupData backup);*/
    }
}