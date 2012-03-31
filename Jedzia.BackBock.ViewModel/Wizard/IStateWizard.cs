// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStateWizard.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Wizard
{
    /// <summary>
    /// Represents a wizard window with multiple pages.
    /// </summary>
    public interface IStateWizard : IDialog
    {
        #region Properties

        /// <summary>
        /// Gets the number of wizard pages.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Gets or sets the selected page of the wizard.
        /// </summary>
        /// <value>
        /// The selected page.
        /// </value>
        int SelectedPage { get; set; }

        #endregion
    }
}