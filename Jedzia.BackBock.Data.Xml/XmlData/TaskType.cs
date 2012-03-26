using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Jedzia.BackBock.Model;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class TaskType
    {
        public Jedzia.BackBock.DataAccess.DTO.TaskType ToHostType()
        {
            Jedzia.BackBock.DataAccess.DTO.TaskType h = new Jedzia.BackBock.DataAccess.DTO.TaskType();
            h.TypeName = this.TypeName;
            if (this.AnyAttr != null)
            {
                h.AnyAttr = new System.Xml.XmlAttribute[this.AnyAttr.Length];
                this.AnyAttr.CopyTo(h.AnyAttr, 0);
            }
            return h;
        }

        public static TaskType FromHostType(Jedzia.BackBock.DataAccess.DTO.TaskType source)
        {
            var local = new TaskType();
            local.TypeName = source.TypeName;
            local.AnyAttr = source.AnyAttr;
            if (source.AnyAttr != null)
            {
                local.AnyAttr = new System.Xml.XmlAttribute[source.AnyAttr.Length];
                source.AnyAttr.CopyTo(local.AnyAttr, 0);
            }
            return local;
        }
    }
}
