using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Jedzia.BackBock.Model.Tests
{
    [TestFixture]
    public class TaskContextTest
    {
        [SetUp]
        public void SetUp()
        {
            TaskContext.ResetToTestCondition();
        }

        [Test]
        public void DefaultContextReturnsCorrectValue()
        {
            var result = TaskContext.Default;
            Assert.IsNotNull(result);
        }

        [Test]
        public void ReplacedContextReturnsCorrectValue()
        {
            var expected = new TestingContext();
            TaskContext.Default = expected;
            var actual = TaskContext.Default;
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void DoubleAssignmentShouldThrow()
        {
            var expected = new TestingContext();
            TaskContext.Default = expected;
            Assert.Throws<InvalidOperationException>(() => TaskContext.Default = expected);
            var actual = TaskContext.Default;
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ResetToDefault()
        {
            var expected = new TestingContext();
            TaskContext.Default = expected;
            var actual = TaskContext.Default;
            Assert.AreSame(expected, actual);
            TaskContext.ResetToDefault();
            actual = TaskContext.Default;
            Assert.AreNotSame(expected, actual);
            Assert.IsInstanceOfType<DefaultTaskContext>(actual);
        }

        [Test]
        public void NullAssignmentShouldThrow()
        {
            TaskContext expected = null;
            Assert.Throws<ArgumentNullException>(() => TaskContext.Default = expected);
            var actual = TaskContext.Default;
            Assert.IsInstanceOfType<DefaultTaskContext>(actual);
        }

        [Test]
        public void GetRegisteredTasksOfDefault()
        {
            var expected = new DefaultTaskContext().TaskService.GetRegisteredTasks();
            var actual = TaskContext.Default.TaskService.GetRegisteredTasks();
            Assert.AreElementsSame(expected, actual);
        }

    }

    public class TestingContext : TaskContext
    {
        public override Jedzia.BackBock.Tasks.ITaskService TaskService
        {
            get { return null; }
        }
    }
}
