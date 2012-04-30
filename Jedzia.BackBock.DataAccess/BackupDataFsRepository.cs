namespace Jedzia.BackBock.DataAccess
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Jedzia.BackBock.DataAccess.DTO;

    /// <summary>
    /// A specialized repository that provides backup data from files on a filesystem.
    /// </summary>
    public abstract class BackupDataFsRepository : BackupDataRepository
    {

        /// <summary>
        /// Loads the backup data from a specified file.
        /// </summary>
        /// <param name="filename">The full path to the file with <see cref="BackupData"/>.</param>
        /// <param name="parameters">Additional parameters used by the repository.</param>
        /// <returns>
        /// The backup data from the specified file.
        /// </returns>
        public abstract BackupData LoadBackupData(string filename, StringDictionary parameters);

        /// <summary>
        /// Saves the backup data to a specified file.
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="filename">The full path to the file with <see cref="BackupData"/>.</param>
        /// <param name="parameters">Additional parameters used by the repository.</param>
        public abstract void SaveBackupData(BackupData data, string filename, StringDictionary parameters);

        /// <summary>
        /// Gets the type of the repository.
        /// </summary>
        /// <value>
        /// The type of the repository.
        /// </value>
        public override BackupRepositoryType RepositoryType
        {
            get { return BackupRepositoryType.FileSystemProvider; }
        }

        /// <summary>
        /// Is not used in a <see cref="BackupDataFsRepository"/> type.
        /// </summary>
        /// <returns>
        /// Nothing, not allowed.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Cannot call the GetBackupData method on a
        /// <see cref="BackupDataFsRepository"/> type.</exception>
        public override BackupData GetBackupData()
        {
            throw new System.NotSupportedException("Cannot call the GetBackupData method on a BackupDataFsRepository type.");
        }
    }
}