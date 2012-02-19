namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;

    public interface ISpecificationWindow : ICanInputBind, ICanCommandBind
    {
        #region Events

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