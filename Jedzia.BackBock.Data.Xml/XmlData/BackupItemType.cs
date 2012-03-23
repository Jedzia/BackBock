using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class BackupItemType 
    {
        public Jedzia.BackBock.Model.Data.BackupItemType ToHostType()
        {
            Jedzia.BackBock.Model.Data.BackupItemType h = new Jedzia.BackBock.Model.Data.BackupItemType();
            h.IsEnabled = this.IsEnabled;
            h.ItemGroup = this.ItemGroup;
            h.ItemName = this.ItemName;
            h.Task = this.Task.ToHostType();
            h.Path = this.Path.ConvertAll(wld => wld.ToHostType());
            return h;
        }

        public static BackupItemType FromHostType(Jedzia.BackBock.Model.Data.BackupItemType source)
        {
            var local = new BackupItemType();
            local.IsEnabled = source.IsEnabled;
            local.ItemGroup = source.ItemGroup;
            local.ItemName = source.ItemName;
            local.Task = TaskType.FromHostType(source.Task);
            local.Path = source.Path.ConvertAll(wld => PathDataType.FromHostType(wld));
            return local;
        }
    }
}
