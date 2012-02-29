using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Design
{
    class DesignDialogService : IDialogService
    {
        #region IDialogService Members

        public void ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public void ShowMessageBox(string message, string title)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDialogService Members

        public void ShowError(System.Windows.Window owner, string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public void ShowError(System.Windows.Window owner, Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(System.Windows.Window owner, string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(System.Windows.Window owner, string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
