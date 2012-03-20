namespace Jedzia.BackBock.ViewModel.Data
{
    using Jedzia.BackBock.Model.Data;
    using System.Collections.Generic;

    public abstract class BackupDataRepository
    {
            public abstract BackupData GetBackupData();

            /*public abstract void DeleteBackup(int id);

            public abstract void InsertBackup(BackupData backup);

            public abstract BackupData SelectBackup(int id);

            public abstract IEnumerable<BackupData> SelectAllBackups();

            public abstract void UpdateBackup(BackupData backup);*/
    }
}