// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDialog.cs" company="EvePanix">
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
    /// Implements the basic dialog characteristics. 
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();
        
        /// <summary>
        /// Shows the dialog to the user.
        /// </summary>
        /// <returns><c>true</c> if the user committed O.K. or <c>false</c> on cancellation.</returns>
        bool? ShowDialog();
    }
}