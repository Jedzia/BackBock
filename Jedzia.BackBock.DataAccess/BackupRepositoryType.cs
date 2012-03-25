namespace Jedzia.BackBock.DataAccess
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
}