using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel.Commands;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace Jedzia.BackBock.ViewModel.Data
{
    public partial class BackupItemViewModel
    {
        public enum WindowTypes { TaskEditor, ClassFieldOptPage, ClassMethodOptPage, ClassPropertyOptPage, ClassEventOptPage, SettingsPage, }

        partial void PathCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            /*if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (PathViewModel item in e.NewItems)
                {
                    backupitem.Path.Add(item.path);
                }
            }*/
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (PathViewModel item in e.NewItems)
                    {
                        backupitem.Path.Add(item.path);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (PathViewModel item in e.OldItems)
                    {
                        backupitem.Path.Remove(item.path);
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

        public Type NumerableType
        {
            get
            {
                //return typeof(PathViewModel);
                return null;
            }
        }

        #region EditCollection Command

        private RelayCommand editCollectionCommand;

        public ICommand EditCollectionCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.editCollectionCommand == null)
                {
                    this.editCollectionCommand = new RelayCommand(this.EditCollectionExecuted, this.EditCollectionEnabled);
                }

                return this.editCollectionCommand;
            }
        }


        private void EditCollectionExecuted(object o)
        {
            //this.EditCollection();
            //MessageBox.Show("Edit Collection");
            // Start collection editor this.Paths.
            /*PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
            var col = new WPG.Themes.TypeEditors.CollectionEditorControl();
            col.MyProperty = new WPG.Data.Property(this, properties["Paths"]);
            col.NumerableType = typeof(PathViewModel);
            col.NumerableValue = this.Paths;
            var pg = new WPG.TypeEditors.CollectionEditorWindow(col);
            pg.ShowDialog();*/
        }

        private bool EditCollectionEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        /*#region AddType Command

        private RelayCommand addTypeCommand;

        public ICommand AddTypeCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.addTypeCommand == null)
                {
                    this.addTypeCommand = new RelayCommand(this.AddTypeExecuted, this.AddTypeEnabled);
                }

                return this.addTypeCommand;
            }
        }


        private void AddTypeExecuted(object o)
        {
            //this.Paths.Add((PathViewModel)o);
            //this.AddType();
        }

        private bool AddTypeEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

        #region RemoveType Command

        private RelayCommand removeTypeCommand;

        public ICommand RemoveTypeCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.removeTypeCommand == null)
                {
                    this.removeTypeCommand = new RelayCommand(this.RemoveTypeExecuted, this.RemoveTypeEnabled);
                }

                return this.removeTypeCommand;
            }
        }


        private void RemoveTypeExecuted(object o)
        {
            //this.Paths.Remove((PathViewModel)o);
        }

        private bool RemoveTypeEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion*/

        #region TaskDataClicked Command

        private RelayCommand taskDataClickedCommand;

        public ICommand TaskDataClickedCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.taskDataClickedCommand == null)
                {
                    this.taskDataClickedCommand = new RelayCommand(this.TaskDataClickedExecuted, this.TaskDataClickedEnabled);
                }

                return this.taskDataClickedCommand;
            }
        }


        private void TaskDataClickedExecuted(object o)
        {
            var wnd = ApplicationViewModel.GetInstanceFromType<Window>(WindowTypes.TaskEditor);
            wnd.DataContext = this.Task;
            wnd.ShowDialog();
        }

        private bool TaskDataClickedEnabled(object sender)
        {
            bool canExecute = true;
            return canExecute;
        }
        #endregion

    }
}
