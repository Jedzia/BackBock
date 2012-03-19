using Jedzia.BackBock.Model.Data;
using System.Windows.Data;
namespace Jedzia.BackBock.ViewModel.Data
{
    public partial class BackupDataViewModel
    {

        public ListCollectionView BackupItemsView
        //public System.Collections.ObjectModel.ObservableCollection<BackupItemViewModel> BackupItems
        {
            get
            {
                /*if (this.backupitem == null)
                {
                    this.backupitem = new System.Collections.ObjectModel.ObservableCollection<BackupItemViewModel>();
                    foreach (var item in this.data.BackupItem)
                    {
                        var colItem = new BackupItemViewModel(item);
                        colItem.PropertyChanged += OnDataPropertyChanged;
                        this.backupitem.Add(colItem);
                    }
                    this.backupitem.CollectionChanged += OnBackupItemCollectionChanged;
                }*/
                var lc = new ListCollectionView(this.backupitem);
                lc.GroupDescriptions.Add(new PropertyGroupDescription("ItemGroup"));
                return lc;
            }
        }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public BackupData MyProperty
        {
            get
            {
                return this.data;
            }

            set
            {
                if (this.data == value)
                {
                    return;
                }
                this.data = value;
                RaisePropertyChanged("MyProperty");
            }
        }

        partial void BackupItemCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (BackupItemViewModel item in e.NewItems)
                    {
                        this.data.BackupItem.Add(item.data);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (BackupItemViewModel item in e.OldItems)
                    {
                        this.data.BackupItem.Remove(item.data);
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
    }
}