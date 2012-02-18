using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Data.Hierarchical
{
    public partial class ClassMemberViewModel
    {
        public System.String ItemType
        {
            get
            {
                return this.GetType().Name.ToString().Replace("ViewModel", "");
            }
        }
    }
}
