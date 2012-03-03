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

        public class MyClass
        {

        }

        public class MyParameterClass
        {
            public string Parameter
            {
                get;
                private set;
            }

            public MyParameterClass(string parameter)
            {
                this.Parameter = parameter;
            }
            #region Methods
            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
            /// </returns>
            /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
            public override bool Equals(object obj)
            {
                MyParameterClass other = obj as MyParameterClass;
                if (other != null)
                    return Equals(other);
                return false;
            }
            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(MyParameterClass other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Parameter == other.Parameter;
            }
            #endregion
        }

        Enum kind;
        Type type;

        /// <summary>
        /// Initializes the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ControlRegistrator.Reset();
            kind = MyEnum.ValueOne;
            type = typeof(MyClass);
            ControlRegistrator.RegisterControl(kind, type);
        }

        /// <summary>
        ///A test for RegisterControl
        ///</summary>
        [Test]
        public void RegisterControlTest()
        {
            Assert.Throws<ArgumentNullException>(() => ControlRegistrator.RegisterControl(null, type));
            Assert.Throws<ArgumentNullException>(() => ControlRegistrator.RegisterControl(kind, null));

        }


        /// <summary>
        ///A test for GetInstanceOfType
        ///</summary>
        public T GetInstanceOfTypeTest1Helper<T>(Enum keyToTest, object[] constructorParameters)
            where T : class
        {

            Enum key = keyToTest;
            object[] parameters = constructorParameters; 

            // T expected = default(T);
            T actual;
            actual = ControlRegistrator.GetInstanceOfType<T>(key, parameters);
            if (actual != null)
            {
                Assert.IsInstanceOfType(typeof(T), actual);
            }
            return actual;
        }

        [Test]
        public void GetInstanceOfTypeTest1()
        {
            // Construct with parameter.
            var kind2 = MyEnum.ValueTwo;
            var type2 = typeof(MyParameterClass);
            var parameter = "A String Parameter";

            ControlRegistrator.RegisterControl(kind2, type2);
            var actual = GetInstanceOfTypeTest1Helper<MyParameterClass>(kind2, new[] { parameter });
            Assert.IsNotNull(actual);
            
            var expected = parameter;
            Assert.AreEqual(expected, actual.Parameter);

            // wrong parameters.
            Assert.Throws<NullReferenceException>(
                () => GetInstanceOfTypeTest1Helper<MyParameterClass>(kind2, null));
            Assert.Throws<NullReferenceException>(
                () => GetInstanceOfTypeTest1Helper<MyParameterClass>(kind2, new object[0] ));
            Assert.Throws<NullReferenceException>(
                () => GetInstanceOfTypeTest1Helper<MyParameterClass>(kind2, new object[] { 5.5d }));
        }

        /// <summary>
        ///A test for GetInstanceOfType
        ///</summary>
        public T GetInstanceOfTypeTestHelper<T>(Enum keyToTest)
            where T : class
        {
            Enum key = keyToTest; 
            T actual;
            actual = ControlRegistrator.GetInstanceOfType<T>(key);
            Assert.IsInstanceOfType(typeof(T), actual);
            return actual;
        }

        [Test]
        public void GetInstanceOfTypeTest()
        {
            GetInstanceOfTypeTestHelper<MyClass>(kind);
            Assert.Throws<KeyNotFoundException>(() => ControlRegistrator.GetInstanceOfType<MyClass>(MyEnum.ValueTwo));
        }

        [Test]
        public void GetMultipleInstancesTest()
        {
            var first = GetInstanceOfTypeTestHelper<MyClass>(kind);
            var second = GetInstanceOfTypeTestHelper<MyClass>(kind);
            Assert.AreNotEqual(first, second);
            Assert.AreNotSame(first, second);

            ControlRegistrator.Reset();
            kind = MyEnum.ValueOne;
            type = typeof(MyClass);
            ControlRegistrator.RegisterControl(kind, type, new SingletonInstance());

            first = GetInstanceOfTypeTestHelper<MyClass>(kind);
            second = GetInstanceOfTypeTestHelper<MyClass>(kind);
            Assert.AreEqual(first, second);
            Assert.AreSame(first, second);
        }

        [Test]
        public void GetMultipleInstancesTest1()
        {
            var kind2 = MyEnum.ValueTwo;
            var type2 = typeof(MyParameterClass);
            var parameter = "A String Parameter";

            ControlRegistrator.RegisterControl(kind2, type2);

            var first = GetInstanceOfTypeTest1Helper<MyParameterClass>(kind2, new[] { parameter });
            var second =GetInstanceOfTypeTest1Helper<MyParameterClass>(kind2, new[] { parameter });
            Assert.AreEqual(first, second);
            Assert.AreNotSame(first, second);
        }

    }


}
