using MbUnit.Framework;
using Jedzia.BackBock.Tasks.Tests.Stubs;
namespace Jedzia.BackBock.Tasks.Tests
{


    /// <summary>
    ///This is a test class for TaskRegistryTest and is intended
    ///to contain all TaskRegistryTest Unit Tests
    ///</summary>
    [TestFixture]
    public class TaskRegistryTest
    {
        ITaskService tr;
       
        [SetUp]
        public void Setup()
        {
            tr = TaskRegistry.GetInstance();
            tr.Reset();
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
        ///A test for the standard registered types.
        ///</summary>
        [Test]
        public void RegisteredTest()
        {
            var expected = new[] { "Backup" };
            var actual = tr.GetRegisteredTasks();
            Assert.AreElementsEqualIgnoringOrder(expected, actual);
        }

        /// <summary>
        ///A test for Reset
        ///</summary>
        [Test]
        public void ResetTest()
        {
            tr.Reset();
            var initRegged = tr.GetRegisteredTasks();
            var task = new TestTask();
            tr.Register(task);
            var newRegged = tr.GetRegisteredTasks();
            Assert.AreElementsNotEqual(initRegged, newRegged);
            tr.Reset();
            var resetRegged = tr.GetRegisteredTasks();
            Assert.AreElementsNotEqual(resetRegged, newRegged);
            Assert.AreElementsEqualIgnoringOrder(initRegged, resetRegged);
        }

        /// <summary>
        ///A test for ResetAll
        ///</summary>
        [Test]
        public void ResetAllTest()
        {
            Assert.IsNotEmpty(tr.GetRegisteredTasks());
            tr.ResetAll();
            var resetRegged = tr.GetRegisteredTasks();
            Assert.IsEmpty(resetRegged);
        }

        /// <summary>
        ///A test for Register
        ///</summary>
        [Test]
        public void RegisterTest()
        {
            var task = new TestTask();
            Assert.AreEqual("TestTask", task.Name);

            bool expected = true;
            bool actual = tr.Register(task);
            Assert.AreEqual(expected, actual);
            // register again, skip.
            expected = false;
            actual = tr.Register(task);
            Assert.AreEqual(expected, actual);

            var actualTask = tr["TestTask"];
            Assert.IsInstanceOfType(typeof(TestTask), actualTask);

            var expectedRegged = new string[] { "Backup", "TestTask" };
            var actualRegged = tr.GetRegisteredTasks();
            Assert.AreElementsEqualIgnoringOrder(expectedRegged, actualRegged);
        }

        /// <summary>
        ///A test for Register
        ///</summary>
        [Test]
        public void IndexerTest()
        {
            // not registered.
            var actualTask = tr["TestTask"];
            Assert.IsNull(actualTask);

            var task = new TestTask();
            tr.Register(task);

            // now with the new type.
            actualTask = tr["TestTask"];
            Assert.IsNotNull(actualTask);
            Assert.IsInstanceOfType(typeof(TestTask), actualTask);
            Assert.AreNotEqual(task, actualTask);
            Assert.AreNotSame(task, actualTask);
        }

        /// <summary>
        ///A test for GetInstance
        ///</summary>
        [Test]
        public void GetInstanceTest()
        {
            ITaskService actual = tr;
            Assert.IsNotNull(actual);
        }
    }
}
