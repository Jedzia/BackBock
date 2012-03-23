using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class BackupData 
    {
        public Jedzia.BackBock.Model.Data.BackupData ToHostType()
        {
            Jedzia.BackBock.Model.Data.BackupData h = new Jedzia.BackBock.Model.Data.BackupData();
            h.DatasetGroup = this.DatasetGroup;
            h.DatasetName = this.DatasetName;
            h.BackupItem = this.BackupItem.ConvertAll(wld => wld.ToHostType());
            return h;
        }

        public static BackupData FromHostType(Jedzia.BackBock.Model.Data.BackupData source)
        {
            var local = new BackupData();
            local.DatasetGroup = source.DatasetGroup;
            local.DatasetName = source.DatasetName;
            local.BackupItem = source.BackupItem.ConvertAll(wld => BackupItemType.FromHostType(wld));
            return local;
        }
    }
}
