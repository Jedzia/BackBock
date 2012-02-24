namespace Jedzia.BackBock.ViewModel.Tests.Util
{
    using Jedzia.BackBock.ViewModel.Util;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Controls;


    /// <summary>
    ///This is a test class for VisualHelperTest and is intended
    ///to contain all VisualHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VisualHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
        ///A test for FindVisualChild
        ///</summary>
        [TestMethod(), Ignore]
        public void FindVisualChildTest()
        {
            var cnt = new ContentControl();
            var btn = new Button();
            //btn.Tag = "MyTag";
            btn.Name = "MyTag";
            cnt.Content = btn;
            cnt.Focus();

            Visual myVisual = btn;
            string tagOrNameToSearch = "MyTag";
            FrameworkElement expected = btn; // TODO: Initialize to an appropriate value
            FrameworkElement actual;
            actual = VisualHelper.FindVisualChild(myVisual, tagOrNameToSearch);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CloneXX
        ///</summary>
        public void CloneXXTestHelper<T>()
            where T : DependencyObject
        {
            T source = default(T); // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = VisualHelper.CloneXX<T>(source);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod(), Ignore]
        public void CloneXXTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call CloneXXTestHelper<T>() with appropriate type parameters.");
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod(), Ignore]
        public void CloneTest()
        {
            FrameworkElement referenceElement = null; // TODO: Initialize to an appropriate value
            bool shareRenderingData = false; // TODO: Initialize to an appropriate value
            FrameworkElement expected = null; // TODO: Initialize to an appropriate value
            FrameworkElement actual;
            actual = VisualHelper.Clone(referenceElement, shareRenderingData);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
