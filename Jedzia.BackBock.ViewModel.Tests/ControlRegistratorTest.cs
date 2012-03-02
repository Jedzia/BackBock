using Jedzia.BackBock.ViewModel;
using MbUnit.Framework;
using System;

namespace Jedzia.BackBock.ViewModel.Tests
{
    using System.Collections.Generic;


    /// <summary>
    ///This is a test class for ControlRegistratorTest and is intended
    ///to contain all ControlRegistratorTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ControlRegistratorTest
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

        enum MyEnum { ValueOne, ValueTwo }

        /// <summary>
        /// Summary
        /// </summary>
        public class MyClass
        {

        }


        /// <summary>
        ///A test for RegisterControl
        ///</summary>
        [Test]
        public void RegisterControlTest()
        {
            Enum kind = MyEnum.ValueOne;
            Type type = typeof(MyClass);
            Assert.Throws<ArgumentNullException>(() => ControlRegistrator.RegisterControl(null, type));
            Assert.Throws<ArgumentNullException>(() => ControlRegistrator.RegisterControl(kind, null));
            ControlRegistrator.RegisterControl(kind, type);

            var instance = ControlRegistrator.GetInstanceOfType<MyClass>(kind);
            Assert.IsInstanceOfType(type, instance);

            Assert.Throws<KeyNotFoundException>(() => ControlRegistrator.GetInstanceOfType<MyClass>(MyEnum.ValueTwo));
        }


        /// <summary>
        ///A test for GetInstanceOfType
        ///</summary>
        public void GetInstanceOfTypeTest1Helper<T>()
            where T : class
        {
            Enum key = null; // TODO: Initialize to an appropriate value
            object[] parameters = null; // TODO: Initialize to an appropriate value
            T expected = null; // TODO: Initialize to an appropriate value
            T actual;
            actual = ControlRegistrator.GetInstanceOfType<T>(key, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test]
        public void GetInstanceOfTypeTest1()
        {
            //GetInstanceOfTypeTest1Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetInstanceOfType
        ///</summary>
        public void GetInstanceOfTypeTestHelper<T>()
            where T : class
        {
            Enum key = null; // TODO: Initialize to an appropriate value
            T expected = null; // TODO: Initialize to an appropriate value
            T actual;
            actual = ControlRegistrator.GetInstanceOfType<T>(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test]
        public void GetInstanceOfTypeTest()
        {
            //GetInstanceOfTypeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for CreateInstanceFromType
        ///</summary>
        public void CreateInstanceFromTypeTestHelper<T>()
            where T : class
        {
            // Creation of the private accessor for 'Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly' failed
            Assert.Inconclusive("Creation of the private accessor for \'Microsoft.VisualStudio.TestTools.TypesAndSy" +
                    "mbols.Assembly\' failed");
        }

        [Test]
        public void CreateInstanceFromTypeTest()
        {
            //CreateInstanceFromTypeTestHelper<GenericParameterHelper>();
        }
    }
}
