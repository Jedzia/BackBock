using Jedzia.BackBock.ViewModel.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Jedzia.BackBock.ViewModel.Tests.Util
{
    
    
    /// <summary>
    ///This is a test class for GuardTest and is intended
    ///to contain all GuardTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GuardTestX
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
        ///A test for NotOutOfRangeExclusive
        ///</summary>
        public void NotOutOfRangeExclusiveTestHelper<T>()
            where T : IComparable
        {
            Expression<Func<T>> reference = null; // TODO: Initialize to an appropriate value
            T value = default(T); // TODO: Initialize to an appropriate value
            T from = default(T); // TODO: Initialize to an appropriate value
            T to = default(T); // TODO: Initialize to an appropriate value
            Guard.NotOutOfRangeExclusive<T>(reference, value, from, to);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void NotOutOfRangeExclusiveTest()
        {
            NotOutOfRangeExclusiveTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for NotNullOrEmpty
        ///</summary>
        [TestMethod()]
        public void NotNullOrEmptyTest()
        {
            Expression<Func<string>> reference = null; // TODO: Initialize to an appropriate value
            string value = string.Empty; // TODO: Initialize to an appropriate value
            Guard.NotNullOrEmpty(reference, value);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void NotNullTest()
        {
            var parameter = "Not Empty";
            Guard.NotNull(() => parameter, parameter);
        }

        /// <summary>
        ///A test for NotDefault
        ///</summary>
        public void NotDefaultTestHelper<T>()
        {
            Expression<Func<T>> reference = null; // TODO: Initialize to an appropriate value
            T value = default(T); // TODO: Initialize to an appropriate value
            Guard.NotDefault<T>(reference, value);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void NotDefaultTest()
        {
            NotDefaultTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetParameterName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Jedzia.BackBock.ViewModel.dll")]
        public void GetParameterNameTest()
        {
            LambdaExpression reference = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Guard_Accessor.GetParameterName(reference);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanBeAssigned
        ///</summary>
        [TestMethod()]
        public void CanBeAssignedTest()
        {
            Expression<Func<object>> reference = null; // TODO: Initialize to an appropriate value
            Type typeToAssign = null; // TODO: Initialize to an appropriate value
            Type targetType = null; // TODO: Initialize to an appropriate value
            Guard.CanBeAssigned(reference, typeToAssign, targetType);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
