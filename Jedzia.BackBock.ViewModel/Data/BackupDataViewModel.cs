// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDataViewModel.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Data
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Data;
    using Jedzia.BackBock.Model.Data;

    /// <summary>
    /// <c>ViewModel</c> representation of <see cref="BackupData"/>. 
    /// </summary>
    public partial class BackupDataViewModel
    {
        #region Fields

        /// <summary>
        /// The <see cref="TestInt" /> property's name.
        /// </summary>
        public const string TestIntPropertyName = "TestInt";

        /// <summary>
        /// The <see cref="TestIntX" /> property's name.
        /// </summary>
        public const string TestIntXPropertyName = "TestIntX";

        private string myProperty = 55.ToString();

        private int testIntX = 123;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the backup items view sorted by ItemGroup's.
        /// </summary>
        public ListCollectionView BackupItemsView
        {
            // public System.Collections.ObjectModel.ObservableCollection<BackupItemViewModel> BackupItems
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
                return data;
            }

            set
            {
                if (data == value)
                {
                    return;
                }

                data = value;
                RaisePropertyChanged("MyProperty");
            }
        }

        /// <summary>
        /// Gets or sets the TestInt property. DeleteMe after testing.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        [Range(18, 90/*, ErrorMessageResourceName = "AgeOutOrRange", ErrorMessageResourceType = typeof(ValidationMessageResources)*/)]
        [Required]
        public string TestInt
        {
            get
            {
                return this.myProperty;
            }

            set
            {
                // bool isValid = ValidateProperty(TestIntPropertyName, value);
                // bool isValid2 = ValidateProperty("DatasetName", DatasetName);
                if (this.myProperty == value)
                {
                    return;
                }

                {
                    // if (isValid)
                    this.myProperty = value;
                }

                bool isValid2 = ValidateProperty(TestIntPropertyName);
                RaisePropertyChanged(TestIntPropertyName);

                // RaisePropertyChanged("Error");
            }
        }

        /// <summary>
        /// Gets or sets the TestIntX property. DeleteMe after testing.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        [RegularExpression("1..", ErrorMessageResourceType = typeof(ValidationMessageResources),
            ErrorMessageResourceName = "WrongTestIntXFormat")]
        public int TestIntX
        {
            get
            {
                // [System.ComponentModel.DataAnnotations.DataType("bka")]
                return this.testIntX;
            }

            set
            {
                // bool isValid = ValidateProperty(TestIntXPropertyName, value);
                if (this.testIntX == value)
                {
                    return;
                }

                // if (isValid)
                // {
                    this.testIntX = value;
                // }

                bool isValid = ValidateProperty(TestIntXPropertyName);
                RaisePropertyChanged(TestIntXPropertyName);
            }
        }

        #endregion

        /// <summary>
        /// Test method for checking <c>ViewModel</c> data errors. DeleteMe.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>An error string or <c>null</c> if all is O.K.</returns>
        public string Nase(string columnName)
        {
            // get {
            string res = null;
            switch (columnName)
            {
                case "DatasetName":
                    res = data[columnName];
                    break;
                case TestIntPropertyName:
                    int testInt;
                    string msg = null;

                    if (string.IsNullOrEmpty(this.TestInt))
                    {
                        return "Age is missing.";
                    }

                    if (!Int32.TryParse(this.TestInt, out testInt))
                    {
                        return "Age is not a whole number.";
                    }

                    this.TestIntX = testInt;

                    // string msg = this.ValidateAge(out age);
                    // if (!String.IsNullOrEmpty(msg))
                    // return msg;

                    // Apply the age value now so that the 
                    // Person object can also validate it.
                    // _person.Age = age;
                    break;
                default:
                    break;
            }

            // if (columnName == TestIntPropertyName)
            // {
            // }

            /*if (DatasetName.Contains("x"))
                return columnName + " has an x";
            else*/
            return res;

            // }
        }

        /*public string Error
        {
            get { return null; }
        }*/

        /// <summary>
        /// Validates the business rules.
        /// </summary>
        /// <returns><c>true</c> on success.</returns>
        public bool ValidateBusinessRules()
        {
            // var DatasetNameRuleHasError = this["DatasetName"] != null;
            // var TestIntRuleHasError = this[TestIntPropertyName] != null;
            // return !DatasetNameRuleHasError && !TestIntRuleHasError;
            return true;
        }

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            // Validate("OnPropertyChanged " + propertyName);
            if (propertyName != "Error")
            {
                // ValidateProperty(propertyName);
                OnValidateViewModel();

                // this.EndEdit();
            }
        }

        /// <summary>
        /// Validates the view model data.
        /// </summary>
        protected override void ValidateViewModel()
        {
            SetBusinessRuleError(DataObject.Error);

            /*int testInt = -1;
                        if (!Int32.TryParse(this.TestInt, out testInt))
                        {
                            AddError("TestInt", "Age is not a whole number.");
                        }
                        TestIntX = testInt;*/
            if (this.TestInt == "55" && this.DatasetName == "Daily")
            {
                SetBusinessRuleError(
                    "The combination of 55 as TestInt and DatasetName with 'Daily' is a deadly combination :P");
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
        // public string this[string columnName]

        /// <summary>
        /// Called when the BackupItem collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        partial void BackupItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Reflect the changes to the underlying data.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (BackupItemViewModel item in e.NewItems)
                    {
                        data.BackupItem.Add(item.data);
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (BackupItemViewModel item in e.OldItems)
                    {
                        data.BackupItem.Remove(item.data);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
    }
}