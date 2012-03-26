using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class BackupItemType 
    {
        public Jedzia.BackBock.DataAccess.DTO.BackupItemType ToHostType()
        {
            Jedzia.BackBock.DataAccess.DTO.BackupItemType h = new Jedzia.BackBock.DataAccess.DTO.BackupItemType();
            h.IsEnabled = this.IsEnabled;
            h.ItemGroup = this.ItemGroup;
            h.ItemName = this.ItemName;
            if (this.Task != null)
                h.Task = this.Task.ToHostType();
            if (this.Path != null)
                h.Path = this.Path.Select(wld => wld.ToHostType()).ToArray();
            return h;
        }

        public static BackupItemType FromHostType(Jedzia.BackBock.DataAccess.DTO.BackupItemType source)
        {
            var local = new BackupItemType();
            local.IsEnabled = source.IsEnabled;
            local.ItemGroup = source.ItemGroup;
            local.ItemName = source.ItemName;
            if (source.Task != null)
                local.Task = TaskType.FromHostType(source.Task);
            if (source.Path != null)
                local.Path = source.Path.Select(wld => PathDataType.FromHostType(wld)).ToArray();
            return local;
        }
    }
}
