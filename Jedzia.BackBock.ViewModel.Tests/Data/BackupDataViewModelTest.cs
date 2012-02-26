using Jedzia.BackBock.ViewModel.Data;
using Jedzia.BackBock.Model.Data;
using System.ComponentModel;
using System.Collections.Generic;
using MbUnit.Framework;

namespace Jedzia.BackBock.ViewModel.Tests.Data
{
    using System.Collections.ObjectModel;


    /// <summary>
    ///This is a test class for BackupDataViewModelTest and is intended
    ///to contain all BackupDataViewModelTest Unit Tests
    ///</summary>
    [TestFixture]
    public class BackupDataViewModelTest
    {
        private BackupDataViewModel target;
        
        [SetUp]
        public void SetUp()
        {
            BackupData backupData = new BackupData();
            backupData.DatasetName = "My Data Set";
            target = new BackupDataViewModel(backupData); 
        }
        
         /// <summary>
        ///A test for DatasetName
        ///</summary>
        [Test]
        public void DatasetNameTest()
        {
            Assert.AreEqual("My Data Set", target.DatasetName);
            string expected = "A new name."; 
            string actual;
            target.DatasetName = expected;
            actual = target.DatasetName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BackupItems
        ///</summary>
        [Test]
        public void BackupItemsTest()
        {
            ObservableCollection<BackupItemViewModel> actual;
            actual = target.BackupItems;
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);

            BackupItemType bit;
            BackupItemViewModel bivm;
            target.BackupItems.Add(bivm = new BackupItemViewModel(bit = new BackupItemType()));
            actual = target.BackupItems;
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreSame(bivm, actual[0]);
        }

        /// <summary>
        ///A test for OnDataPropertyChanged
        ///</summary>
        [Test]
        public void OnDataPropertyChangedTest()
        {
            /*PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            BackupDataViewModel_Accessor target = new BackupDataViewModel_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            PropertyChangedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnDataPropertyChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");*/
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [Test]
        public void CloneTest()
        {
            BackupDataViewModel expected = target; 
            BackupDataViewModel actual;
            actual = target.Clone();
            Assert.AreNotEqual(expected, actual);
            Assert.AreEqual(expected.DatasetName, actual.DatasetName);
            Assert.AreEqual(0, actual.BackupItems.Count);
            Assert.AreEqual(expected.BackupItems, actual.BackupItems);
            Assert.AreElementsEqual(expected.BackupItems, actual.BackupItems);

            BackupItemType bit;
            BackupItemViewModel bivm;
            target.BackupItems.Add(bivm = new BackupItemViewModel(bit = new BackupItemType()));
            actual = target.Clone();
            Assert.AreNotEqual(expected, actual);
            Assert.AreEqual(expected.DatasetName, actual.DatasetName);
            Assert.AreEqual(1, actual.BackupItems.Count);
            Assert.AreEqual(expected.BackupItems, actual.BackupItems);
            Assert.AreElementsEqual(expected.BackupItems, actual.BackupItems);
        }

        /// <summary>
        ///A test for BackupDataViewModel Constructor
        ///</summary>
        [Test]
        public void BackupDataViewModelConstructorTest()
        {
            BackupData backupData = null; // TODO: Initialize to an appropriate value
            BackupDataViewModel target = new BackupDataViewModel(backupData);
            // Todo: Guard checks for initialization arguments.
            /*AssertEx.IsNull(target.DatasetName);
            string expected = "A new name.";
            string actual;
            target.DatasetName = expected;
            actual = target.DatasetName;
            Assert.AreEqual(expected, actual);*/

        }
    }
}
