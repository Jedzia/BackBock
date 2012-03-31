// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogServiceBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System;
    using System.Windows;

    /// <summary>
    /// Base class for Windows with integrated dialog display capability.
    /// </summary>
    public class DialogServiceBase : Window, IDialogService
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogServiceBase"/> class.
        /// </summary>
        public DialogServiceBase()
        {
            Loaded += this.PhonePageBaseLoaded;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dialog control.
        /// </summary>
        /// <value>
        /// The dialog control.
        /// </value>
        public IDialogControl DialogControl { get; protected set; }

        #endregion

        /// <summary>
        /// Shows the specified error to the user and waits for feedback.
        /// </summary>
        /// <param name="message">The message of the error.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="buttonText">The text of the commit button.</param>
        /// <param name="hideCallback">A callback that gets executed after the dialog is hidden.</param>
        public virtual void ShowError(string message, string title, string buttonText, Action hideCallback)
        {
            if (this.DialogControl != null)
            {
                this.DialogControl.IsShowingError = true;
                this.DialogControl.Message = message;
                this.DialogControl.Title = title;
                this.DialogControl.ConfirmButtonText = buttonText;
                this.DialogControl.CancelButtonText = null;
                this.DialogControl.Show(hideCallback);
            }

#if DEBUG
            else
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }

#endif
        }

        /// <inheritdoc/>
        public virtual void ShowError(Exception error, string title, string buttonText, Action hideCallback)
        {
            if (this.DialogControl != null)
            {
                this.DialogControl.IsShowingError = true;
                this.DialogControl.Message = error.Message;
                this.DialogControl.Title = title;
                this.DialogControl.ConfirmButtonText = buttonText;
                this.DialogControl.CancelButtonText = null;
                this.DialogControl.Show(hideCallback);
            }

#if DEBUG
            else
            {
                MessageBox.Show(error.Message, title, MessageBoxButton.OK);
            }

#endif
        }

        /// <inheritdoc/>
        public virtual void ShowMessage(string message, string title, string buttonText, Action hideCallback)
        {
            if (this.DialogControl != null)
            {
                this.DialogControl.IsShowingError = false;
                this.DialogControl.Message = message;
                this.DialogControl.Title = title;
                this.DialogControl.ConfirmButtonText = buttonText;
                this.DialogControl.CancelButtonText = null;
                this.DialogControl.Show(hideCallback);
            }

#if DEBUG
            else
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }

#endif
        }

        /// <inheritdoc/>
        public virtual void ShowMessage(
            string message, string title, string confirmButtonText, string cancelButtonText, Action<bool> callback)
        {
            if (this.DialogControl != null)
            {
                this.DialogControl.IsShowingError = false;
                this.DialogControl.Message = message;
                this.DialogControl.Title = title;
                this.DialogControl.ConfirmButtonText = confirmButtonText;
                this.DialogControl.CancelButtonText = cancelButtonText;
                this.DialogControl.Show(callback);
            }

#if DEBUG
            else
            {
                callback(MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK);
            }

#endif
        }

        /// <inheritdoc/>
        public virtual void ShowMessageBox(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        private void PhonePageBaseLoaded(object sender, RoutedEventArgs e)
        {
            this.DialogControl = FindName("DialogControl") as IDialogControl;
            Loaded -= this.PhonePageBaseLoaded;
        }
    }
}