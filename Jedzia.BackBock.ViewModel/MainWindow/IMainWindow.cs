namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using Jedzia.BackBock.ViewModel.Commands;

    public interface IMainWindow : ICanInputBind, ICanCommandBind
    {
        #region Events

        event EventHandler Initialized;

        #endregion

        #region Properties

        IMainWorkArea Designer { get; }
        //void ShowDetail(object val);

        #endregion
    }
}