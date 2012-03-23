namespace Jedzia.BackBock.ViewModel.Data
{
    using Jedzia.BackBock.Model.Data;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public enum BackupRepositoryType
    {
        Unknown,
        FileSystemProvider,
        Database,
        Static
    }

    public abstract class BackupDataRepository
    {
            public abstract BackupData GetBackupData();

            public abstract BackupRepositoryType RepositoryType { get; }

            /*public abstract void DeleteBackup(int id);

            public abstract void InsertBackup(BackupData backup);

            public abstract BackupData SelectBackup(int id);

            public abstract IEnumerable<BackupData> SelectAllBackups();

            public abstract void UpdateBackup(BackupData backup);*/
    }

    public abstract class BackupDataFsRepository : BackupDataRepository
    {

        public abstract BackupData LoadBackupData(string filename, StringDictionary parameters);
        public override BackupRepositoryType RepositoryType
        {
            get { return BackupRepositoryType.FileSystemProvider; }
        }
        public override BackupData GetBackupData()
        {
            throw new System.NotImplementedException();
        }
    }
}