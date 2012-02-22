using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;

namespace Jedzia.BackBock.ViewModel.Data
{
    public partial class PathViewModel
    {
        public PathViewModel()
        {
            path = new PathDataType();
            path.Path = "Moese, hehehe";
        }
    }
}
