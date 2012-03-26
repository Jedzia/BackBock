using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class Wildcard 
    {
        public Jedzia.BackBock.DataAccess.DTO.Wildcard ToHostType()
        {
            Jedzia.BackBock.DataAccess.DTO.Wildcard h = new Jedzia.BackBock.DataAccess.DTO.Wildcard();
            h.Enabled = this.Enabled;
            h.Pattern = this.Pattern;
            return h;
        }

        public static Wildcard FromHostType(Jedzia.BackBock.DataAccess.DTO.Wildcard source)
        {
            var local = new Wildcard();
            local.Enabled = source.Enabled;
            local.Pattern = source.Pattern;
            return local;
        }

    }
}
