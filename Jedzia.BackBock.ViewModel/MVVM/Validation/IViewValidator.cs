using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
    /// <summary>
    /// Indicates that a view implements a validation of its values.
    /// </summary>
    public interface IViewValidator
    {
        /// <summary>
        /// Callback for full-View validation, called to determine if this blood pressure test can be saved.
        /// </summary>
        /// <returns>
        /// Returns if this View is valid.
        /// </returns>
        bool ValidateView();
    }
}
