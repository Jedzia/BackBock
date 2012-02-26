//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:v2.0.50727
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

/*
This code was automatically generated at 02/26/2012 02:54:05 by 
        Jedzia's ViewModel generator.
Changes to this file may be lost if regeneration occurs.
http://xxx.com
*/
// Config = Debug DefaultNamespace = Jedzia.BackBock.ViewModel

using System.Collections.Generic;
using Jedzia.BackBock.SharedTypes;
using Jedzia.BackBock.Model.Data;
using Jedzia.BackBock.ViewModel.MainWindow;


namespace Jedzia.BackBock.ViewModel.Data
{

    /// <summary>
    /// The summary of BackupItemViewModel. BaseType: 
    /// </summary>
    public partial class BackupItemViewModel : ViewModelBase
    {
        internal BackupItemType backupitem;

        public BackupItemViewModel(BackupItemType backupItem)
        {
            this.backupitem = backupItem;
        }

        // Path. HasFacets: False AttrQName: 
        //                   propertyType: System.Collections.ObjectModel.ObservableCollection<Path>, IsChoiceRoot: False, BaseType: 
        //                   ListType: None, HasCommonBaseType: False, xxxx: 
        /// <summary>
        /// The summary. 
        /// </summary>
        private System.Collections.ObjectModel.ObservableCollection<PathViewModel> path;

        public System.Collections.ObjectModel.ObservableCollection<PathViewModel> Paths
        {
            get
            {
                if (this.path == null)
                {
                    this.path = new System.Collections.ObjectModel.ObservableCollection<PathViewModel>();
                    foreach (var item in this.backupitem.Path)
                    {
                        var colItem = new PathViewModel(item);
                        colItem.PropertyChanged += OnDataPropertyChanged;
                        this.path.Add(colItem);
                    }
                    this.path.CollectionChanged += OnPathCollectionChanged;
                }
                return this.path;
            }
        }

        protected virtual void OnPathCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PathCollectionChanged(sender, e);
        }

        partial void PathCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

        // Task. HasFacets: False AttrQName: 
        //                   propertyType: TaskType, IsChoiceRoot: False, BaseType: 
        //                   ListType: None, HasCommonBaseType: False, xxxx: 
        /// <summary>
        /// The summary. 
        /// </summary>
        private TaskViewModel task;

        public TaskViewModel Task
        {
            get
            {
                if (this.task == null)
                {
                    this.task = new TaskViewModel(this.backupitem.Task);
                    this.task.PropertyChanged += OnDataPropertyChanged;
                }
                return this.task;
            }
        }

        protected virtual void OnTaskCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TaskCollectionChanged(sender, e);
        }

        partial void TaskCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);

        // ItemName. HasFacets: False AttrQName: 
        //                   propertyType: System.String, IsChoiceRoot: False, BaseType: 
        //                   ListType: None, HasCommonBaseType: False, xxxx: 
        /// <summary>
        /// Gets or sets the ItemName. HasFacets: False AttrQName: 
        /// </summary> // Attribute
        /// <value>The ItemName.</value>
        public System.String ItemName
        {
            get
            {
                return this.backupitem.ItemName;
            }

            set
            {
                if (this.backupitem.ItemName == value)
                {
                    return;
                }
                this.backupitem.ItemName = value;
                RaisePropertyChanged("ItemName");
            }
        }

        // IsEnabled. HasFacets: False AttrQName: 
        //                   propertyType: System.Boolean, IsChoiceRoot: False, BaseType: 
        //                   ListType: None, HasCommonBaseType: False, xxxx: 
        /// <summary>
        /// Gets or sets the IsEnabled. HasFacets: False AttrQName: 
        /// </summary> // Attribute
        /// <value>The IsEnabled.</value>
        public System.Boolean IsEnabled
        {
            get
            {
                return this.backupitem.IsEnabled;
            }

            set
            {
                if (this.backupitem.IsEnabled == value)
                {
                    return;
                }
                this.backupitem.IsEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        protected virtual void OnDataPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DataPropertyChanged(sender, e);
        }

        partial void DataPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e);

        public BackupItemViewModel Clone()
        {
            return (BackupItemViewModel)this.MemberwiseClone();
        }
    } 
}
