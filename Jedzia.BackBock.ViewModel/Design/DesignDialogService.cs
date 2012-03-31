// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignDialogService.cs" company="EvePanix">
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
    using System.Windows;

    /// <summary>
    /// Dialog service for Design-Time. Empty stub.
    /// </summary>
    public class DesignDialogService : IDialogService
    {
        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowError(Window owner, string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowError(Window owner, Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowMessage(
            string message, 
            string title, 
            string buttonConfirmText, 
            string buttonCancelText, 
            Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowMessage(Window owner, string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowMessage(
            Window owner, 
            string message, 
            string title, 
            string buttonConfirmText, 
            string buttonCancelText, 
            Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ShowMessageBox(string message, string title)
        {
            throw new NotImplementedException();
        }
    }
}