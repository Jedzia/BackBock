using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.ComponentModel;

namespace Jedzia.BackBock.ViewModel.Data
{
    public class ExclusionViewModelList : List<ExclusionViewModel> { }

    [DisplayName("Path to use")]
    public partial class PathViewModel
    {
        public PathViewModel()
        {
            path = new PathDataType();
            path.Path = "Moese, hehehe";
        }
    }
}
