// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMainWindow.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using Jedzia.BackBock.ViewModel.Commands;

    /// <summary>
    /// Main window of the application
    /// </summary>
    public interface IMainWindow : ICanInputBind, ICanCommandBind /*, ISelectionService*/, IDialogServiceProvider
    {
        #region Events

        /// <summary>
        /// Occurs when the main window is initialized.
        /// </summary>
        event EventHandler Initialized;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the work area.
        /// </summary>
        IMainWorkArea WorkArea { get; }

        #endregion

        // MainWindowViewModel MainWindowViewModel { get; }

        /// <summary>
        /// Clears the log text.
        /// </summary>
        void ClearLogText();

        /// <summary>
        /// Updates the log text of the main window.
        /// </summary>
        /// <param name="text">The logging text to show.</param>
        void UpdateLogText(string text);
    }
}