using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Data.Xml.XmlData
{
    public partial class PathDataType 
    {
        public Jedzia.BackBock.Model.Data.PathDataType ToHostType()
        {
            Jedzia.BackBock.Model.Data.PathDataType h = new Jedzia.BackBock.Model.Data.PathDataType();
            h.Path = this.Path;
            h.UserData = this.UserData;
            h.Exclusion = this.Exclusion.ConvertAll(wld => wld.ToHostType());
            h.Inclusion = this.Inclusion.ConvertAll(wld => wld.ToHostType());
            return h;
        }

        public static PathDataType FromHostType(Jedzia.BackBock.Model.Data.PathDataType source)
        {
            var local = new PathDataType();
            local.Path = source.Path;
            local.UserData = source.UserData;
            local.Exclusion = source.Exclusion.ConvertAll(wld => Wildcard.FromHostType(wld));
            local.Inclusion = source.Inclusion.ConvertAll(wld => Wildcard.FromHostType(wld));
            return local;
        }
    }
}
