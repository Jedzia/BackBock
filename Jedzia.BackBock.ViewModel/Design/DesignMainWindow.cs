// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignMainWindow.cs" company="EvePanix">
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
    using System.Windows.Input;
    using Jedzia.BackBock.ViewModel.MainWindow;

    /// <summary>
    /// Design-Time main window stub.
    /// </summary>
    public class DesignMainWindow : IMainWindow
    {
        #region Events

        /// <summary>
        /// Occurs when the main window is initialized.
        /// </summary>
        public event EventHandler Initialized;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a collection of <see cref="CommandBinding"/> objects associated with this element.
        /// A <see cref="T:System.Windows.Input.CommandBinding"/> enables command handling for
        /// this element, and declares the linkage between a command, its events, and the handlers
        /// attached by this element.
        /// </summary>
        /// <returns>
        /// The collection of all <see cref="CommandBinding"/> objects.
        /// </returns>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public CommandBindingCollection CommandBindings
        {
            get
            {
                if (Initialized != null)
                {

                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the dialog service for this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public IDialogService DialogService
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the collection of input bindings associated with this element.
        /// </summary>
        /// <returns>
        /// The collection of input bindings.
        /// </returns>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public InputBindingCollection InputBindings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the work area.
        /// </summary>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public IMainWorkArea WorkArea
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        /// <summary>
        /// Clears the log text.
        /// </summary>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void ClearLogText()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the log text of the main window.
        /// </summary>
        /// <param name="text">The logging text to show.</param>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public void UpdateLogText(string text)
        {
            throw new NotImplementedException();
        }
    }
}