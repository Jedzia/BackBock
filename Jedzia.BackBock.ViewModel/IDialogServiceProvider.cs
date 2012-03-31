// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDialogServiceProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    /// <summary>
    /// Provides a <see cref="DialogService"/>.
    /// </summary>
    public interface IDialogServiceProvider
    {
        #region Properties

        /// <summary>
        /// Gets the dialog service for this instance.
        /// </summary>
        IDialogService DialogService { get; }

        #endregion
    }
}