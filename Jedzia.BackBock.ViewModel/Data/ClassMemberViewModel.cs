using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Data
{
    /// <summary>
    /// Class Member base data model.
    /// </summary>
    public partial class ClassMemberViewModel
    {
        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public System.String ItemType
        {
            get
            {
                return this.GetType().Name.ToString().Replace("ViewModel", "");
            }
        }
    }
}
