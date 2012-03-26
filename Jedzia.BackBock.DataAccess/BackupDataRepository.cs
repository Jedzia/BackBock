namespace Jedzia.BackBock.DataAccess
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Jedzia.BackBock.DataAccess.DTO;

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
}