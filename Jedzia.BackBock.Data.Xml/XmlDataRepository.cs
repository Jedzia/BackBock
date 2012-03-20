using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.Data;
using Jedzia.BackBock.Model.Data;

namespace Jedzia.BackBock.Data.Xml
{
    public class XmlDataRepository : BackupDataRepository
    {
        // ctor with filename

        public override BackupData GetBackupData()
        {
            //var d = new XmlBackupDataDataProvider();
            // here use internal context which is disposable to access the data.
            var data = new BackupData();
            //var dData = new XmlData.BackupData();
            // transform XmlData.BackupData to Model.BackupData
            data.BackupItem.Insert(0, new BackupItemType() { ItemName = "This is from Xml" });
            return data;
        }
    }
}
