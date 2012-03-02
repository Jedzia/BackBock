// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayModeViewModelTest.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Tests
{
    using System.Collections.Generic;
    using MbUnit.Framework;

    /// <summary>
    /// This is a test class for DisplayModeViewModelTest and is intended
    ///  to contain all DisplayModeViewModelTest Unit Tests
    /// </summary>
    [TestFixture]
    public class DisplayModeViewModelTest
    {
        #region Fields

        private DisplayModeViewModel target;

        #endregion

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        // }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {
        // }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// A test for DisplayAll
        /// </summary>
        [Test]
        public void DisplayAllTest()
        {
            string actual;
            string expected = "Collapsed";
            actual = this.target.DisplayAll;
            Assert.AreEqual(expected, actual);

            this.target.DisplayMode = DisplayMode.Standard;
            actual = this.target.DisplayAll;
            Assert.AreEqual(expected, actual);

            this.target.DisplayMode = DisplayMode.Expert;
            actual = this.target.DisplayAll;
            Assert.AreEqual(expected, actual);

            this.target.DisplayMode = DisplayMode.All;
            expected = "Visible";
            actual = this.target.DisplayAll;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for DisplayAll
        /// </summary>
        [Test]
        public void DisplayAllTest1()
        {
            string actual;
            actual = this.target.DisplayAll;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for DisplayExpert
        /// </summary>
        [Test]
        public void DisplayExpertTest()
        {
            string actual;
            string expected = "Collapsed";
            actual = this.target.DisplayExpert;
            Assert.AreEqual(expected, actual);

            this.target.DisplayMode = DisplayMode.Standard;
            actual = this.target.DisplayExpert;
            Assert.AreEqual(expected, actual);

            this.target.DisplayMode = DisplayMode.Expert;
            expected = "Visible";
            actual = this.target.DisplayExpert;
            Assert.AreEqual(expected, actual);

            this.target.DisplayMode = DisplayMode.All;
            expected = "Visible";
            actual = this.target.DisplayExpert;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for DisplayMode
        /// </summary>
        [Test]
        public void DisplayModeTest()
        {
            DisplayMode expected = DisplayMode.Standard;
            DisplayMode actual;
            actual = this.target.DisplayMode;
            Assert.AreEqual(expected, actual);

            expected = DisplayMode.Standard;
            this.target.DisplayMode = expected;
            actual = this.target.DisplayMode;
            Assert.AreEqual(expected, actual);

            expected = DisplayMode.Expert;
            this.target.DisplayMode = expected;
            actual = this.target.DisplayMode;
            Assert.AreEqual(expected, actual);

            expected = DisplayMode.All;
            this.target.DisplayMode = expected;
            actual = this.target.DisplayMode;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for IsStandardDisplayMode
        /// </summary>
        [Test]
        public void DisplayModeTransitionTest()
        {
            int count = 0;
            var props = new List<string>();
            this.target.PropertyChanged += (e, o) =>
                                               {
                                                   count++;
                                                   props.Add(o.PropertyName);
                                               };
            bool actual;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(0, count);

            // Standard -> Expert : DisplayExpert
            this.target.DisplayMode = DisplayMode.Expert;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(2, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayExpert" }, props);
            
            props.Clear();
            count = 0;

            // Expert -> All      : DisplayAll
            props.Clear();
            this.target.DisplayMode = DisplayMode.All;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(2, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayAll" }, props);

            props.Clear();
            count = 0;

            // All -> Expert      : DisplayAll
            props.Clear();
            this.target.DisplayMode = DisplayMode.Expert;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(2, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayAll" }, props);

            props.Clear();
            count = 0;

            // Expert -> Standard : DisplayExpert
            props.Clear();
            this.target.DisplayMode = DisplayMode.Standard;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(2, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayExpert" }, props);

            props.Clear();
            count = 0;

            // Standard -> All    : DisplayExpert, DisplayAll
            props.Clear();
            this.target.DisplayMode = DisplayMode.All;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(3, count);
            Assert.AreElementsEqual(new[] { "DisplayMode", "DisplayExpert", "DisplayAll" }, props);

            props.Clear();
            count = 0;

            // All -> Standard    : DisplayExpert, DisplayAll
            props.Clear();
            this.target.DisplayMode = DisplayMode.Standard;
            actual = this.target.IsStandardDisplayMode;
            Assert.IsTrue(actual);
            Assert.AreEqual(3, count);
            Assert.AreElementsEqual(props, new[] { "DisplayMode", "DisplayAll", "DisplayExpert" });



        }

        /// <summary>
        /// A test for IsAllDisplayMode
        /// </summary>
        [Test]
        public void IsAllDisplayModeTest()
        {
            bool actual;
            actual = this.target.IsAllDisplayMode;
            Assert.IsFalse(actual);

            this.target.DisplayMode = DisplayMode.Expert;
            actual = this.target.IsAllDisplayMode;
            Assert.IsFalse(actual);

            this.target.DisplayMode = DisplayMode.All;
            actual = this.target.IsAllDisplayMode;
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// A test for IsExpertDisplayMode
        /// </summary>
        [Test]
        public void IsExpertDisplayModeTest()
        {
            bool actual;
            actual = this.target.IsExpertDisplayMode;
            Assert.IsFalse(actual);

            this.target.DisplayMode = DisplayMode.Expert;
            actual = this.target.IsExpertDisplayMode;
            Assert.IsTrue(actual);

            this.target.DisplayMode = DisplayMode.All;
            actual = this.target.IsExpertDisplayMode;
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Initializes the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.target = new DisplayModeViewModel();
        }
    }
}