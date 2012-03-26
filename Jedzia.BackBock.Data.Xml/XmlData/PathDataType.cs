using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class PathDataType 
    {
        public Jedzia.BackBock.DataAccess.DTO.PathDataType ToHostType()
        {
            Jedzia.BackBock.DataAccess.DTO.PathDataType h = new Jedzia.BackBock.DataAccess.DTO.PathDataType();
            h.Path = this.Path;
            h.UserData = this.UserData;
            if (this.Exclusion != null)
                h.Exclusion = this.Exclusion.Select(wld => wld.ToHostType()).ToArray();
            if (this.Inclusion != null)
                h.Inclusion = this.Inclusion.Select(wld => wld.ToHostType()).ToArray();
            return h;
        }

        public static PathDataType FromHostType(Jedzia.BackBock.DataAccess.DTO.PathDataType source)
        {
            var local = new PathDataType();
            local.Path = source.Path;
            local.UserData = source.UserData;
            if (source.Exclusion != null)
                local.Exclusion = source.Exclusion.Select(wld => Wildcard.FromHostType(wld)).ToArray();
            if (source.Inclusion != null)
                local.Inclusion = source.Inclusion.Select(wld => Wildcard.FromHostType(wld)).ToArray();
            return local;
        }
    }
}
