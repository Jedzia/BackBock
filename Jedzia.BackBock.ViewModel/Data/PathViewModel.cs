﻿using System;
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

        partial void ExclusionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (ExclusionViewModel item in e.NewItems)
                    {
                        this.path.Exclusion.Add(item.exclusion);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (ExclusionViewModel item in e.OldItems)
                    {
                        this.path.Exclusion.Remove(item.exclusion);
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
                        this.path.Inclusion.Add(item.inclusion);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (InclusionViewModel item in e.OldItems)
                    {
                        this.path.Inclusion.Remove(item.inclusion);
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
