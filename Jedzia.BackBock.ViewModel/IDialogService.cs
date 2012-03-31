using System;

namespace Jedzia.BackBock.ViewModel
{
    /// <summary>
    /// Provides a way to present dialogues to the user.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Shows the specified error to the user and waits for feedback.
        /// </summary>
        /// <param name="message">The message of the error.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="buttonText">The text of the commit button.</param>
        /// <param name="afterHideCallback">A callback that gets executed after the dialog is hidden.</param>
        void ShowError(string message, string title, string buttonText, Action afterHideCallback);

        /// <summary>
        /// Shows the specified error to the userand waits for feedback.
        /// </summary>
        /// <param name="error">The text of the error message.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="buttonText">The text of the commit button.</param>
        /// <param name="afterHideCallback">A callback that gets executed after the dialog is hidden.</param>
        void ShowError(Exception error, string title, string buttonText, Action afterHideCallback);

        /// <summary>
        /// Explicitely shows a message box to the user and waits for feedback.
        /// </summary>
        /// <param name="message">The message shown to the user.</param>
        /// <param name="title">The title of the dialog.</param>
        void ShowMessageBox(string message, string title);

        /// <summary>
        /// Shows the specified message to the user and waits for feedback.
        /// </summary>
        /// <param name="message">The message shown to the user.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="buttonText">The text of the commit button.</param>
        /// <param name="afterHideCallback">A callback that gets executed after the dialog is hidden.</param>
        void ShowMessage(string message, string title, string buttonText, Action afterHideCallback);

        /// <summary>
        /// Shows the specified message to the user and waits for feedback.
        /// </summary>
        /// <param name="message">The message shown to the user.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="buttonConfirmText">The text of the commit button.</param>
        /// <param name="buttonCancelText">The text of the cancel button.</param>
        /// <param name="afterHideCallback">A callback that gets executed after the dialog is hidden.</param>
        void ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback);
    }
}
