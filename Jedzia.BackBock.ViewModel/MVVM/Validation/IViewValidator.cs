// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewValidator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
    /// <summary>
    /// Indicates that a view implements a validation of its values.
    /// </summary>
    public interface IViewValidator
    {
        /// <summary>
        /// Callback for full-View validation, called to determine if the model can be persisted.
        /// </summary>
        /// <returns>
        /// Returns if this View is valid.
        /// </returns>
        bool ValidateView();
    }
}