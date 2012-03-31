// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignIOService.cs" company="EvePanix">
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
    using System.IO;

    /// <summary>
    /// Design-Time Input/Output service stub.
    /// </summary>
    public class DesignIOService : IOService
    {
        #region Fields

        private const string Error = "DesignIOService does not provide any IO-Operations!";

        #endregion

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>
        /// A stream to the selected file or <see langword="null"/> if the operation wasn't successful.
        /// </returns>
        /// <exception cref="NotImplementedException"><see cref="DesignIOService"/> does not provide any IO-Operations!</exception>
        public Stream OpenFile(string path)
        {
            throw new NotImplementedException(Error);
        }

        /// <summary>
        /// Shows a Open file dialog to the user.
        /// </summary>
        /// <param name="defaultPath">The default path for the file dialog.</param>
        /// <returns>
        /// The path of the selected file or <c>null</c> if the operation was canceled.
        /// </returns>
        /// <exception cref="NotImplementedException"><see cref="DesignIOService"/> does not provide any IO-Operations!</exception>
        public string OpenFileDialog(string defaultPath)
        {
            throw new NotImplementedException(Error);
        }

        /// <summary>
        /// Shows a Save file dialog to the user.
        /// </summary>
        /// <param name="defaultPath">The default path for the file dialogs.</param>
        /// <returns>
        /// The path of the selected file or <c>null</c> if the operation was canceled.
        /// </returns>
        /// <exception cref="NotImplementedException"><see cref="DesignIOService"/> does not provide any IO-Operations!</exception>
        public string SaveFileDialog(string defaultPath)
        {
            throw new NotImplementedException(Error);
        }
    }
}