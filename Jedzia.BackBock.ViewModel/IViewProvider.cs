// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using Jedzia.BackBock.ViewModel.Wizard;

    /// <summary>
    /// Provides View's to the ViewModels.
    /// </summary>
    public interface IViewProvider
    {
        /// <summary>
        /// Gets the task editor.
        /// </summary>
        /// <returns>The task editor.</returns>
        IDialogWindow GetTaskEditor();

        /// <summary>
        /// Gets the task wizard.
        /// </summary>
        /// <returns>The task wizard.</returns>
        IStateWizard GetWizard();
    }
}