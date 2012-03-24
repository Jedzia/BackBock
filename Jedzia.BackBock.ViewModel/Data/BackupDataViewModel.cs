using Jedzia.BackBock.Model.Data;
using System.Windows.Data;
using System.ComponentModel;
using System;
namespace Jedzia.BackBock.ViewModel.Data
{
    public partial class BackupDataViewModel : IEditableObject 
    {
        /// <summary>
        /// The <see cref="TestInt" /> property's name.
        /// </summary>
        public const string TestIntPropertyName = "TestInt";

        private string _myProperty = 55.ToString();

        /// <summary>
        /// Sets and gets the TestInt property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TestInt
        {
            get
            {
                return _myProperty;
            }

            set
            {
                if (_myProperty == value)
                {
                    return;
                }

                _myProperty = value;
                RaisePropertyChanged(TestIntPropertyName);
                //RaisePropertyChanged("Error");
            }
        }

        /// <summary>
        /// The <see cref="TestIntX" /> property's name.
        /// </summary>
        public const string TestIntXPropertyName = "TestIntX";

        private int testIntX = 123;

        /// <summary>
        /// Sets and gets the TestIntX property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TestIntX
        {
            get
            {
                return testIntX;
            }

            set
            {
                if (testIntX == value)
                {
                    return;
                }

                testIntX = value;
                RaisePropertyChanged(TestIntXPropertyName);
            }
        }

        #region IDataErrorInfo Members

        /*public string Error
        {
            get { return null; }
        }*/

        public bool ValidateBussinessRules()
        {
            //var DatasetNameRuleHasError = this["DatasetName"] != null;
            //var TestIntRuleHasError = this[TestIntPropertyName] != null;
            //return !DatasetNameRuleHasError && !TestIntRuleHasError;
            return true;
        }

        protected override void ValidateFields()
        {
            /*var res = this.data["DatasetName"];
            if (res != null)
            {
                SetFieldError("DatasetName", "from ValidateFields() " + res);
            }*/
            SetBusinessRuleError(this.data["DatasetName"]);
            SetFieldError("DatasetName", this.data["DatasetName"]);

            int testInt = -1;
            if (string.IsNullOrEmpty(TestInt))
            {
                SetFieldError("TestInt", "Age is missing.");
            }

            if (!Int32.TryParse(TestInt, out testInt))
            {
                SetFieldError("TestInt", "Age is not a whole number.");
            }
            TestIntX = testInt;
        }

        // see src\DataObjectBase\DataObjectBase.cs from "20091019 - Serializable object base part 2.zip"
        //public string this[string columnName]
        public string Nase(string columnName)
        {
            //get {
                string res = null;
                switch (columnName)
                {
                    case "DatasetName":
                        res = this.data[columnName];
                        break;
                    case TestIntPropertyName:
                        int testInt;
                        string msg = null;

                        if (string.IsNullOrEmpty(TestInt))
                        {
                            return "Age is missing.";
                        }

                        if (!Int32.TryParse(TestInt, out testInt))
                        {
                            return "Age is not a whole number.";
                        }
                        TestIntX = testInt;
                        //string msg = this.ValidateAge(out age);
                        //if (!String.IsNullOrEmpty(msg))
                        //return msg;

                        // Apply the age value now so that the 
                        // Person object can also validate it.
                        //_person.Age = age;
                        break;
                    default:
                        break;
                }

                //if (columnName == TestIntPropertyName)
                //{
                //}

                /*if (DatasetName.Contains("x"))
                    return columnName + " has an x";
                else*/
                    return res;
            //}
        }

        #endregion
        
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

        #region IEditableObject Members

        public void BeginEdit()
        {
            throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            throw new NotImplementedException();
        }

        public void EndEdit()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}