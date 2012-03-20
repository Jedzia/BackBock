using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Jedzia.BackBock.Model;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class TaskType
    {
        public Jedzia.BackBock.Model.Data.TaskType ToHostType()
        {
            Jedzia.BackBock.Model.Data.TaskType h = new Jedzia.BackBock.Model.Data.TaskType();
            h.TypeName = this.TypeName;
            h.AnyAttr = this.AnyAttr;
            return h;
        }

        public static TaskType FromHostType(Jedzia.BackBock.Model.Data.TaskType source)
        {
            var local = new TaskType();
            local.TypeName = source.TypeName;
            local.AnyAttr = source.AnyAttr;
            return local;
        }
    }
}
