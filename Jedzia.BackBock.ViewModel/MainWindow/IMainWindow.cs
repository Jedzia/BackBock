namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using Jedzia.BackBock.ViewModel.Commands;

    /// <summary>
    /// Main window of the application
    /// </summary>
    public interface IMainWindow : ICanInputBind, ICanCommandBind/*, ISelectionService*/, IDialogServiceProvider
    {
        #region Events

        event EventHandler Initialized;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the work area.
        /// </summary>
        IMainWorkArea WorkArea { get; }
        //MainWindowViewModel MainWindowViewModel { get; }
        
        /// <summary>
        /// Updates the log text.
        /// </summary>
        /// <param name="text">The text.</param>
        void UpdateLogText(string text);
        
        /// <summary>
        /// Clears the log text.
        /// </summary>
        void ClearLogText();

        #endregion

    }
}