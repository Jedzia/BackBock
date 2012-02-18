namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using Jedzia.BackBock.ViewModel.Diagram.Designer;

    public interface IMainWindow : ICanInputBind, ICanCommandBind
    {
        #region Events

        event EventHandler Initialized;

        #endregion

        #region Properties

        IDesignerCanvas Designer { get; }

        #endregion
    }
}