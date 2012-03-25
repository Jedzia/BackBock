using Jedzia.BackBock.Model.Data;
using System.Windows.Data;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
namespace Jedzia.BackBock.ViewModel.Data
{
    public partial class BackupDataViewModel
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
        [Range(18, 90/*, ErrorMessageResourceName = "AgeOutOrRange", ErrorMessageResourceType = typeof(ValidationMessageResources)*/)]
        [Required(/*ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationMessageResources)*/)]
        public string TestInt
        {
            get
            {
                return _myProperty;
            }

            set
            {
                bool isValid = ValidateProperty(TestIntPropertyName, value);
                //bool isValid2 = ValidateProperty("DatasetName", DatasetName);
                if (_myProperty == value)
                {
                    return;
                }

                //if (isValid)
                {
                    _myProperty = value;
                }
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
                bool isValid = ValidateProperty(TestIntXPropertyName, value);
                if (testIntX == value)
                {
                    return;
                }

                //if (isValid)
                {
                    testIntX = value;
                }
                RaisePropertyChanged(TestIntXPropertyName);
            }
        }

        #region IDataErrorInfo Members

        /*public string Error
        {
            get { return null; }
        }*/

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            //Validate("OnPropertyChanged " + propertyName);
            if (propertyName != "Error")
            {
                OnValidateViewModel();

                //this.EndEdit();
            }
        }


        public bool ValidateBussinessRules()
        {
            //var DatasetNameRuleHasError = this["DatasetName"] != null;
            //var TestIntRuleHasError = this[TestIntPropertyName] != null;
            //return !DatasetNameRuleHasError && !TestIntRuleHasError;
            return true;
        }

        protected override void ValidateViewModel()
        {
            /*int testInt = -1;
            if (!Int32.TryParse(this.TestInt, out testInt))
            {
                AddError("TestInt", "Age is not a whole number.");
            }
            TestIntX = testInt;*/
            if (this.TestInt == "55" && this.DatasetName == "Daily")
            {
                SetBusinessRuleError("The combination of 55 as TestInt and DatasetName with 'Daily' is a deadly combination :P");
            }
            /*if (this.TestInt == "42")
            {
                SetBusinessRuleError("The origin of the universe...");
            }
            if (this.DatasetName.Contains("x"))
            {
                SetBusinessRuleError("DatasetName cannot contain an 'x'.");
            }*/

        }

        /*protected override void ValidateFields()
        {
            //var res = this.data["DatasetName"];
            //if (res != null)
            //{
            //    SetFieldError("DatasetName", "from ValidateFields() " + res);
            //}
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
        }*/

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
    }
}