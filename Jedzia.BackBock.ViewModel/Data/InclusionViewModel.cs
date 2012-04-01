using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;

namespace Jedzia.BackBock.ViewModel.Data
{
    /// <summary>
    /// DataViewModel representation of the <see cref="Wildcard"/> Inclusion data.
    /// </summary>
    public partial class InclusionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InclusionViewModel"/> class.
        /// </summary>
        public InclusionViewModel()
        {
            this.data = new Wildcard();
        }
    }
}
