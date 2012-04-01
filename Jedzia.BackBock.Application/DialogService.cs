using System;
using System.Windows;
using Jedzia.BackBock.ViewModel;

namespace Jedzia.BackBock.Application
{
    public class DialogService : /*Window,*/ IDialogService
    {
        public IDialogControl DialogControl
        {
            get;
            protected set;
        }

        public DialogService()
        {
            //Loaded += PhonePageBaseLoaded;
            //PhonePageBaseLoaded(null, null);
        }

        void PhonePageBaseLoaded(object sender, RoutedEventArgs e)
        {
            //DialogControl = FindName("DialogControl") as IDialogControl;
            //DialogControl = RequestDialogControl();
            //Loaded -= PhonePageBaseLoaded;
        }
        
        private IDialogControl RequestDialogControl()
        {
            //DialogControl = FindName("DialogControl") as IDialogControl;
            return new CustomMessageBox();
            //Loaded -= PhonePageBaseLoaded;
        }

        #region Implementation of IDialogService

        public virtual void ShowError(string message, string title, string buttonText, Action hideCallback)
        {
            DialogControl = RequestDialogControl();
            if (DialogControl != null)
            {
                DialogControl.Owner = (Window)ViewModelLocator.MainStatic.ApplicationContext.MainWindow;
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
            DialogControl = RequestDialogControl();
            if (DialogControl != null)
            {
                DialogControl.Owner = (Window)ViewModelLocator.MainStatic.ApplicationContext.MainWindow;
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
            DialogControl = RequestDialogControl();
            if (DialogControl != null)
            {
                DialogControl.Owner = (Window)ViewModelLocator.MainStatic.ApplicationContext.MainWindow;
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
            DialogControl = RequestDialogControl();
            if (DialogControl != null)
            {
                DialogControl.Owner = (Window)ViewModelLocator.MainStatic.ApplicationContext.MainWindow;
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
