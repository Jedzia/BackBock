using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.ComponentModel;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows.Input;
using System.Windows;

namespace Jedzia.BackBock.ViewModel.Data
{
    [DisplayName("Wildcard's to exclude")]
    public partial class ExclusionViewModel : ViewModelBase
    {
        public ExclusionViewModel()
        {
            this.data = new Wildcard();
        }
    
        #region EditorCancel Command

        private RelayCommand editorCancelCommand;

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICommand EditorCancelCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.editorCancelCommand == null)
                {
                    this.editorCancelCommand = new RelayCommand(this.EditorCancelExecuted, this.EditorCancelEnabled);
                }

                return this.editorCancelCommand;
            }
        }


        private void EditorCancelExecuted(object o)
        {
            MessageBox.Show(this.GetType().FullName + ".EditorCancelExecuted() is not implemented.");
            //this.EditorCancel();
        }

        private bool EditorCancelEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

    }
}
