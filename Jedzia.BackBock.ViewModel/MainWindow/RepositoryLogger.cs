namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using Jedzia.BackBock.DataAccess;
    using Jedzia.BackBock.ViewModel.Util;

    public class RepositoryLogger : BackupDataFsRepository
    {
        private readonly BackupDataRepository innerRepository;
        private readonly ILogger auditor;
        public RepositoryLogger(BackupDataRepository repository, ILogger auditor)
        {
            Guard.NotNull(() => repository, repository);
            Guard.NotNull(() => auditor, auditor);
            this.innerRepository = repository;
            this.auditor = auditor;
        }

        public override Jedzia.BackBock.DataAccess.DTO.BackupData GetBackupData()
        {
            auditor.LogMessageEvent("GetBackupData()");
            return innerRepository.GetBackupData();
        }

        public override BackupRepositoryType RepositoryType
        {
            get { return innerRepository.RepositoryType; }
        }

        public override Jedzia.BackBock.DataAccess.DTO.BackupData LoadBackupData(string filename, System.Collections.Specialized.StringDictionary parameters)
        {
            auditor.LogMessageEvent("LoadBackupData(" + filename+ ")");
            return ((BackupDataFsRepository)innerRepository).LoadBackupData(filename,  parameters);
        }
    }
}