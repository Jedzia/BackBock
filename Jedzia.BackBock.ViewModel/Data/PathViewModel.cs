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
    //public class ExclusionViewModelList : List<ExclusionViewModel> { }

    [DisplayName("Path to use")]
    public partial class PathViewModel //: ICustomTypeDescriptor
    {
        public PathViewModel()
        {
            data = new PathDataType();
            data.Path = "Moese, hehehe";
        }

        partial void ExclusionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (ExclusionViewModel item in e.NewItems)
                    {
                        this.data.Exclusion.Add(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (ExclusionViewModel item in e.OldItems)
                    {
                        this.data.Exclusion.Remove(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        partial void InclusionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (InclusionViewModel item in e.NewItems)
                    {
                        this.data.Inclusion.Add(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (InclusionViewModel item in e.OldItems)
                    {
                        this.data.Inclusion.Remove(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
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
            var path = ApplicationContext.MainIOService.OpenFileDialog(string.Empty);
            this.Path = path;
        }

        private bool OpenFileClickedEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

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
