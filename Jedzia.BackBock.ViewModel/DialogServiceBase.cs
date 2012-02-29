using System;
using System.Windows;

namespace Jedzia.BackBock.ViewModel
{
    public class DialogServiceBase :  Window, IDialogService
    {
        public IDialogControl DialogControl
        {
            get;
            protected set;
        }

        public DialogServiceBase()
        {
            Loaded += PhonePageBaseLoaded;
        }

        void PhonePageBaseLoaded(object sender, RoutedEventArgs e)
        {
            DialogControl = FindName("DialogControl") as IDialogControl;
            Loaded -= PhonePageBaseLoaded;
        }

        #region Implementation of IDialogService

        public virtual void ShowError(string message, string title, string buttonText, Action hideCallback)
        {
            if (DialogControl != null)
            {
                DialogControl.IsShowingError = true;
                DialogControl.Message = message;
                DialogControl.Title = title;
                DialogControl.ConfirmButtonText = buttonText;
                DialogControl.CancelButtonText = null;
                DialogControl.Show(hideCallback);
            }
#if DEBUG
            else
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }
#endif
        }

        public virtual void ShowError(Exception error, string title, string buttonText, Action hideCallback)
        {
            if (DialogControl != null)
            {
                DialogControl.IsShowingError = true;
                DialogControl.Message = error.Message;
                DialogControl.Title = title;
                DialogControl.ConfirmButtonText = buttonText;
                DialogControl.CancelButtonText = null;
                DialogControl.Show(hideCallback);
            }
#if DEBUG
            else
            {
                MessageBox.Show(error.Message, title, MessageBoxButton.OK);
            }
#endif
        }

        public virtual void ShowMessageBox(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        public virtual void ShowMessage(string message, string title, string buttonText, Action hideCallback)
        {
            if (DialogControl != null)
            {
                DialogControl.IsShowingError = false;
                DialogControl.Message = message;
                DialogControl.Title = title;
                DialogControl.ConfirmButtonText = buttonText;
                DialogControl.CancelButtonText = null;
                DialogControl.Show(hideCallback);
            }
#if DEBUG
            else
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }
#endif
        }

        public virtual void ShowMessage(string message, string title, string confirmButtonText, string cancelButtonText, Action<bool> callback)
        {
            if (DialogControl != null)
            {
                DialogControl.IsShowingError = false;
                DialogControl.Message = message;
                DialogControl.Title = title;
                DialogControl.ConfirmButtonText = confirmButtonText;
                DialogControl.CancelButtonText = cancelButtonText;
                DialogControl.Show(callback);
            }
#if DEBUG
            else
            {
                callback(MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK);
            }
#endif
        }
        #endregion
    }
}
