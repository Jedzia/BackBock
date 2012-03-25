namespace Jedzia.BackBock.DataAccess
{
    using Jedzia.BackBock.Model.Data;
    using System.Collections.Generic;
    using System.Collections.Specialized;

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