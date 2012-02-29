﻿namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using Jedzia.BackBock.ViewModel.Commands;

    public interface IMainWindow : ICanInputBind, ICanCommandBind, ISelectionService, IDialogServiceProvider
    {
        #region Events

        event EventHandler Initialized;

        #endregion

        #region Properties

        IMainWorkArea WorkArea { get; }
        //MainWindowViewModel MainWindowViewModel { get; }
        //void ShowDetail(object val);

        #endregion
    }
}