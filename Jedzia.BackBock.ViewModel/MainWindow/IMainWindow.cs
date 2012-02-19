namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;

    public interface IMainWindow : ICanInputBind, ICanCommandBind
    {
        #region Events

        event EventHandler Initialized;

        #endregion

        #region Properties

        IMainWorkArea Designer { get; }

        #endregion
    }
}