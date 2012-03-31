// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOService.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System.IO;

    /// <summary>
    /// Implements File-IO capabilities. 
    /// </summary>
    public interface IOService
    {
        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>
        /// A stream to the selected file or <see langword="null"/> if the operation wasn't successful.
        /// </returns>
        Stream OpenFile(string path);

        /// <summary>
        /// Shows a Open file dialog to the user.
        /// </summary>
        /// <param name="defaultPath">The default path for the file dialog.</param>
        /// <returns>The path of the selected file or <c>null</c> if the operation was canceled.</returns>
        string OpenFileDialog(string defaultPath);

        /// <summary>
        /// Shows a Save file dialog to the user.
        /// </summary>
        /// <param name="defaultPath">The default path for the file dialogs.</param>
        /// <returns>
        /// The path of the selected file or <c>null</c> if the operation was canceled.
        /// </returns>
        string SaveFileDialog(string defaultPath);

        // Other similar untestable IO operations
    }
}