// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignViewProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Design
{
    using System;
    using Jedzia.BackBock.ViewModel.Wizard;

    /// <summary>
    /// Design-Time view provider stub.
    /// </summary>
    public class DesignViewProvider : IViewProvider
    {
        /// <summary>
        /// Gets the task editor.
        /// </summary>
        /// <returns>
        /// The task editor.
        /// </returns>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public IDialogWindow GetTaskEditor()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the task wizard.
        /// </summary>
        /// <returns>
        /// The task wizard.
        /// </returns>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public IStateWizard GetWizard()
        {
            throw new NotImplementedException();
        }
    }
}