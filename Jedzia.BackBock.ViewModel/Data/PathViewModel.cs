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
    /// <summary>
    /// DataViewModel representation of the <see cref="PathDataType"/> data.
    /// </summary>
    [DisplayName("Path to use")]
    public partial class PathViewModel //: ICustomTypeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathViewModel"/> class.
        /// </summary>
        public PathViewModel()
        {
            data = new PathDataType();
            data.Path = "Moese, hehehe";
        }

        /// <summary>
        /// Called when the Exclusion collection is changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Called when the Inclusion collection is changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Gets the open file clicked command. Opens a FileDialog chooser.
        /// </summary>
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
            // Todo: ioService
            //var path = ApplicationContext.MainIOService.OpenFileDialog(string.Empty);
            //this.Path = path;
        }

        private bool OpenFileClickedEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        #region EditorCancel Command

        private RelayCommand editorCancelCommand;

        /// <summary>
        /// Gets the editor cancel command.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICommand EditorCancelCommand
        {
            get
            {
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
