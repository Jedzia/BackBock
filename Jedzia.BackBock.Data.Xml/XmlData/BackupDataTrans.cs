using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class BackupData 
    {
        public Jedzia.BackBock.DataAccess.DTO.BackupData ToHostType()
        {
            Jedzia.BackBock.DataAccess.DTO.BackupData h = new Jedzia.BackBock.DataAccess.DTO.BackupData();
            h.DatasetGroup = this.DatasetGroup;
            h.DatasetName = this.DatasetName;
            if (this.BackupItem != null)
                h.BackupItem = this.BackupItem.Select(wld => wld.ToHostType()).ToArray();
            return h;
        }

        public static BackupData FromHostType(Jedzia.BackBock.DataAccess.DTO.BackupData source)
        {
            var local = new BackupData();
            local.DatasetGroup = source.DatasetGroup;
            local.DatasetName = source.DatasetName;
            if (source.BackupItem != null)
                local.BackupItem = source.BackupItem.Select(wld => BackupItemType.FromHostType(wld)).ToArray();
            return local;
        }
    }
}
