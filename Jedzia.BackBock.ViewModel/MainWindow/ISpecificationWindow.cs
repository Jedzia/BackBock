namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using Jedzia.BackBock.ViewModel.Commands;

    /// <summary>
    /// Provides input binding, initialization and closing capabilities.
    /// </summary>
    public interface ISpecificationWindow : ICanInputBind, ICanCommandBind
    {
        #region Events

        /// <summary>
        /// Occurs after the instance was initialized.
        /// </summary>
        event EventHandler Initialized;

        #endregion


        /// <summary>
        /// Manually closes a System.Windows.Window.
        /// </summary>
        void Close();

        #region Properties

        //IMainWorkArea Designer { get; }

        #endregion
    }
}