// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDialogWindow.cs" company="EvePanix">
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
    /// A dialog window with a data binding <c>DataContext</c>.
    /// </summary>
    public interface IDialogWindow : IDialog
    {
        #region Properties

        /// <summary>
        /// Gets or sets the data context for an element when it participates in data binding. 
        /// This is a dependency property.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        /// <returns>
        /// The object to use as data context.
        /// </returns>
        object DataContext { get; set; }

        #endregion
    }
}