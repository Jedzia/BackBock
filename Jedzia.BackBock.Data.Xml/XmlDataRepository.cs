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

        public override BackupData GetBackupData()
        {
            //var d = new DesignDataProvider();
            var data = new BackupData();
            data.BackupItem.Insert(0, new BackupItemType() { ItemName = "This is from Xml" });
            return data;
        }
    }
}
