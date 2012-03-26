namespace Jedzia.BackBock.DataAccess
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Jedzia.BackBock.DataAccess.DTO;

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