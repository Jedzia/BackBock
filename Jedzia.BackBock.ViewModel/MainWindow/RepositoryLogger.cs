namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using Jedzia.BackBock.DataAccess;
    using Jedzia.BackBock.ViewModel.Util;

    /// <summary>
    /// An decorator/wrapper for BackupDataFsRepository types.
    /// </summary>
    public class RepositoryLogger : BackupDataFsRepository
    {
        private readonly BackupDataRepository innerRepository;
        private readonly ILogger auditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryLogger"/> class.
        /// </summary>
        /// <param name="repository">The repository to wrap.</param>
        /// <param name="auditor">The logging auditor.</param>
        public RepositoryLogger(BackupDataRepository repository, ILogger auditor)
        {
            Guard.NotNull(() => repository, repository);
            Guard.NotNull(() => auditor, auditor);
            this.innerRepository = repository;
            this.auditor = auditor;
        }

        /// <summary>
        /// Is not used in a <see cref="BackupDataFsRepository"/> type.
        /// </summary>
        /// <returns>
        /// Nothing, not allowed.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Cannot call the GetBackupData method on a
        ///   <see cref="BackupDataFsRepository"/> type.</exception>
        public override Jedzia.BackBock.DataAccess.DTO.BackupData GetBackupData()
        {
            auditor.LogMessageEvent("ERROR: GetBackupData() in a BackupDataFsRepository type.");
            return innerRepository.GetBackupData();
        }

        /// <summary>
        /// Gets the type of the repository.
        /// </summary>
        /// <value>
        /// The type of the repository.
        /// </value>
        public override BackupRepositoryType RepositoryType
        {
            get { return innerRepository.RepositoryType; }
        }

        /// <summary>
        /// Loads the backup data from a specified file.
        /// </summary>
        /// <param name="filename">The full path to the file with <see cref="Jedzia.BackBock.DataAccess.DTO.BackupData"/>.</param>
        /// <param name="parameters">Additional parameters used by the repository.</param>
        /// <returns>
        /// The backup data from the specified file.
        /// </returns>
        public override Jedzia.BackBock.DataAccess.DTO.BackupData LoadBackupData(string filename, System.Collections.Specialized.StringDictionary parameters)
        {
            auditor.LogMessageEvent("LoadBackupData(" + filename+ ")");
            return ((BackupDataFsRepository)innerRepository).LoadBackupData(filename,  parameters);
        }
    }
}