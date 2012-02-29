using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.MainWindow;

namespace Jedzia.BackBock.ViewModel.Design
{
    class DesignMainWindow : IMainWindow
    {
        #region IMainWindow Members

        public event EventHandler Initialized;

        public IMainWorkArea WorkArea
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ICanInputBind Members

        public System.Windows.Input.InputBindingCollection InputBindings
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ICanCommandBind Members

        public System.Windows.Input.CommandBindingCollection CommandBindings
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ISelectionService Members

        public object SelectedItem
        {
            get { return null; }
        }

        #endregion

        #region IDialogServiceProvider Members

        public IDialogService DialogService
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
