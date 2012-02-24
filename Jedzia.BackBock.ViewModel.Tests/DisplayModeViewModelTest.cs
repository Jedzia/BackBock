using Jedzia.BackBock.ViewModel;
using MbUnit.Framework;
namespace Jedzia.BackBock.ViewModel.Tests
{
    using Gallio.Framework;


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
            DisplayModeViewModel target = new DisplayModeViewModel(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsStandardDisplayMode;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsExpertDisplayMode
        ///</summary>
        [Test]
        public void IsExpertDisplayModeTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsExpertDisplayMode;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsAllDisplayMode
        ///</summary>
        [Test]
        public void IsAllDisplayModeTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsAllDisplayMode;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DisplayMode
        ///</summary>
        [Test]
        public void DisplayModeTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel(); // TODO: Initialize to an appropriate value
            DisplayMode expected = new DisplayMode(); // TODO: Initialize to an appropriate value
            DisplayMode actual;
            target.DisplayMode = expected;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DisplayExpert
        ///</summary>
        [Test]
        public void DisplayExpertTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DisplayExpert;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DisplayAll
        ///</summary>
        [Test]
        public void DisplayAllTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DisplayAll;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DisplayModeViewModel Constructor
        ///</summary>
        [Test]
        public void DisplayModeViewModelConstructorTest()
        {
            DisplayModeViewModel target = new DisplayModeViewModel();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
