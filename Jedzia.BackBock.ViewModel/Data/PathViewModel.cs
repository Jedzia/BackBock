using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.ComponentModel;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows.Input;

namespace Jedzia.BackBock.ViewModel.Data
{
    public class ExclusionViewModelList : List<ExclusionViewModel> { }

    [DisplayName("Path to use")]
    public partial class PathViewModel //: ICustomTypeDescriptor
    {
        public PathViewModel()
        {
            path = new PathDataType();
            path.Path = "Moese, hehehe";
        }

        #region OpenFileClicked Command

        private RelayCommand openFileClickedCommand;

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICommand OpenFileClickedCommand
        {
            get
            {
                if (this.openFileClickedCommand == null)
                {
                    this.openFileClickedCommand = new RelayCommand(this.OpenFileClickedExecuted, this.OpenFileClickedEnabled);
                }

                return this.openFileClickedCommand;
            }
        }


        private void OpenFileClickedExecuted(object o)
        {
            var path = ApplicationViewModel.MainIOService.OpenFileDialog(string.Empty);
            this.Path = path;
        }

        private bool OpenFileClickedEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

    }
}
