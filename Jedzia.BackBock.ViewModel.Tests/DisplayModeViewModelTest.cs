using Jedzia.BackBock.ViewModel;
using MbUnit.Framework;
using System.Collections.Generic;
namespace Jedzia.BackBock.ViewModel.Tests
{

    /// <summary>
    ///This is a test class for DisplayModeViewModelTest and is intended
    ///to contain all DisplayModeViewModelTest Unit Tests
    ///</summary>
    [TestFixture]
    public class DisplayModeViewModelTest
    {

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for IsStandardDisplayMode
        ///</summary>
        [Test]
        public void IsStandardDisplayModeTest()
        {
            int count=0;
            List<string> props = new List<string>();
            DisplayModeViewModel target = new DisplayModeViewModel();
            target.PropertyChanged += (e, o) => { count++; props.Add(o.PropertyName); };
            bool actual;
            actual = target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(0, count);

            target.DisplayMode = DisplayMode.Expert;
            actual = target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(3, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayExpert", "DisplayAll" }, props);

            props.Clear();
            target.DisplayMode = DisplayMode.All;
            actual = target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(6, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayExpert", "DisplayAll" }, props);
        }

        /// <summary>
        ///A test for IsExpertDisplayMode
        ///</summary>
        [Test]
        public void IsExpertDisplayModeTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
            bool actual;
            actual = target.IsExpertDisplayMode;
            Assert.IsFalse(actual);

            target.DisplayMode = DisplayMode.Expert;
            actual = target.IsExpertDisplayMode;
            Assert.IsTrue(actual);

            target.DisplayMode = DisplayMode.All;
            actual = target.IsExpertDisplayMode;
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for IsAllDisplayMode
        ///</summary>
        [Test]
        public void IsAllDisplayModeTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
            bool actual;
            actual = target.IsAllDisplayMode;
            Assert.IsFalse(actual);

            target.DisplayMode = DisplayMode.Expert;
            actual = target.IsAllDisplayMode;
            Assert.IsFalse(actual);

            target.DisplayMode = DisplayMode.All;
            actual = target.IsAllDisplayMode;
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for DisplayMode
        ///</summary>
        [Test]
        public void DisplayModeTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
            DisplayMode expected = DisplayMode.Standard;
            DisplayMode actual;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);
            
            expected = DisplayMode.Standard;
            target.DisplayMode = expected;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);

            expected = DisplayMode.Expert;
            target.DisplayMode = expected;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);

            expected = DisplayMode.All;
            target.DisplayMode = expected;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayExpert
        ///</summary>
        [Test]
        public void DisplayExpertTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
            string actual;
            string expected = "Collapsed";
            actual = target.DisplayExpert;
            Assert.AreEqual(expected, actual);

            target.DisplayMode = DisplayMode.Standard;
            actual = target.DisplayExpert;
            Assert.AreEqual(expected, actual);

            target.DisplayMode = DisplayMode.Expert;
            expected = "Visible";
            actual = target.DisplayExpert;
            Assert.AreEqual(expected, actual);

            target.DisplayMode = DisplayMode.All;
            expected = "Visible";
            actual = target.DisplayExpert;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayAll
        ///</summary>
        [Test]
        public void DisplayAllTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
            string actual;
            string expected = "Collapsed";
            actual = target.DisplayAll;
            Assert.AreEqual(expected, actual);

            target.DisplayMode = DisplayMode.Standard;
            actual = target.DisplayAll;
            Assert.AreEqual(expected, actual);

            target.DisplayMode = DisplayMode.Expert;
            actual = target.DisplayAll;
            Assert.AreEqual(expected, actual);

            target.DisplayMode = DisplayMode.All;
            expected = "Visible";
            actual = target.DisplayAll;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayModeViewModel Constructor
        ///</summary>
        [Test]
        public void DisplayModeViewModelConstructorTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
        }
    }
}
