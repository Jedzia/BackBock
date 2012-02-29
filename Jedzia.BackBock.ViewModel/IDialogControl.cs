using System;
using System.Windows;

namespace Jedzia.BackBock.ViewModel
{
    public interface IDialogControl
    {
        string Message
        {
            get;
            set;
        }

        string Title
        {
            get;
            set;
        }

        string ConfirmButtonText
        {
            get;
            set;
        }

        string CancelButtonText
        {
            get;
            set;
        }

        bool IsShowingError
        {
            get;
            set;
        }

        Window Owner { get; set; }

        void Show(Action callback);
        void Show(Action<bool> callbackWithBool);
        void Hide();
    }
}
