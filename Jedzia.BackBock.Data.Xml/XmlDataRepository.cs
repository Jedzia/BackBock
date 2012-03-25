using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Data.Xml.XmlData;
using System.Collections.Specialized;
using Jedzia.BackBock.DataAccess;

namespace Jedzia.BackBock.Data.Xml
{
    // need for a BackupDataRepositoryFactory for requesting repositories on new file load.

    public class XmlDataRepository : BackupDataFsRepository
    {
        //private readonly string filename;
        public XmlDataRepository(/*string filename*/)
        {
            //this.context =
            //    new CommerceObjectContext(connString);
            //this.filename = filename;
        }

        /*public override BackupRepositoryType RepositoryType
        {
            get { return BackupRepositoryType.FileSystemProvider; }
        }*/

        public override Jedzia.BackBock.Model.Data.BackupData LoadBackupData(string filename, StringDictionary parameters)
        {
            //var loaded = new BackupData();
            //var result = loaded.ToHostType();
            //return result;

            //var d = new XmlBackupDataProvider();
            // here use internal context which is disposable to access the data.
            var data = new Jedzia.BackBock.Model.Data.BackupData();
            //data.toh
            //var dData = new XmlData.BackupData();
            // transform XmlData.BackupData to Model.BackupData
            data.BackupItem.Insert(0, new Jedzia.BackBock.Model.Data.BackupItemType() { ItemName = "This is from Xml" });
            return data;
        }
    }
}
