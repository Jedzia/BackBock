using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Design
{
    /// <summary>
    /// Dialog service for Design-Time. Empty stub.
    /// </summary>
    public class DesignDialogService : IDialogService
    {
        #region IDialogService Members

        /// <inheritdoc/>
        public void ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowMessageBox(string message, string title)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowError(System.Windows.Window owner, string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowError(System.Windows.Window owner, Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowMessage(System.Windows.Window owner, string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ShowMessage(System.Windows.Window owner, string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
