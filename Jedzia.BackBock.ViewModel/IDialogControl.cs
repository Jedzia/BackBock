// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDialogControl.cs" company="EvePanix">
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
    /// Represents a dialog control to show messages to the user.
    /// </summary>
    public interface IDialogControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text of the cancel button.
        /// </summary>
        /// <value>
        /// The text of the cancel button.
        /// </value>
        string CancelButtonText { get; set; }

        /// <summary>
        /// Gets or sets the text of the confirm button.
        /// </summary>
        /// <value>
        /// The text of the confirm button.
        /// </value>
        string ConfirmButtonText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is showing an error to the user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is showing an error; otherwise, <c>false</c>.
        /// </value>
        bool IsShowingError { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message shown by the dialog control.
        /// </value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the parent window.
        /// </summary>
        /// <value>
        /// The owner window.
        /// </value>
        Window Owner { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title shown by the dialog control.
        /// </value>
        string Title { get; set; }

        #endregion

        /// <summary>
        /// Hides the window.
        /// </summary>
        void Hide();

        /// <summary>
        /// Shows the window to the user.
        /// </summary>
        /// <param name="callback">Is called after the user confirmed or canceled the dialog.</param>
        void Show(Action callback);

        /// <summary>
        /// Shows the window to the user and gets feedback.
        /// </summary>
        /// <param name="callbackWithBool">Is called after the user confirmed or canceled the dialog.
        /// Returns the result of the interaction.</param>
        void Show(Action<bool> callbackWithBool);
    }
}