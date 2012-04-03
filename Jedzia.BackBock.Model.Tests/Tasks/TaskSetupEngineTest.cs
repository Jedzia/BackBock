// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskSetupEngineTest.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Tests.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Xml;
    using Jedzia.BackBock.Model.Data;
    using Jedzia.BackBock.Model.Tasks;
    using Jedzia.BackBock.Model.Tests.Tasks.Logging;
    using MbUnit.Framework;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;
    using Rhino.Mocks.Interfaces;

    [TestFixture]
    public class TaskSetupEngineTest
    {
        #region Setup/Teardown

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            // theOnlyTask = MockRepository.GenerateStrictMock<ITask>();
            this.theOnlyTask = new TestTask();
            TaskContext.Default = new TestTaskContext(this.theOnlyTask);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            TaskContext.ResetToDefault();
        }

        [SetUp]
        public void SetUp()
        {
            this.mocks = new MockRepository();

            TestTask.HasExecuted = false;
            TestTask.Instantiated = 0;
            TestTask.MyBuildEngine = null;
            TestTask.MyHostObject = null;

            // this.logger = mocks.StrictMock<ILogger>();
            // eventSource = mocks.StrictMock<IEventSource>();
            //this.resultOfLogHelper = string.Empty;
            //Action<string> logMethod = (log) => this.resultOfLogHelper += log /*+ Environment.NewLine*/;

            // this.logger = mocks.StrictMock<TestLogger>(new LogHelper(logMethod), eventSource);
            // this.logger = new TestLogger(logMethod);
            //this.logger = this.mocks.StrictMock<TestLogger>(logMethod);
            this.blogger = this.mocks.StrictMock<IBuildLogger>();

            this.paths = new[] { new PathDataType() };
        }

        [TearDown]
        public void TearDown()
        {
            //this.logger = null;
            this.paths = null;

            // if (!testObject.IsDisposed)
            // {
            // testObject.Dispose();
            // }
            this.mocks = null;
        }

        #endregion

        private static readonly DateTime TestTimeStamp = new DateTime(2001, 11, 9, 8, 46, 13);
        private static readonly string TimeStampString = TestTimeStamp.ToString(CultureInfo.InvariantCulture);
        private IBuildLogger blogger;

        // private TaskSetupEngine testObject;
        //private TestLogger logger;
        private MockRepository mocks;
        private IEnumerable<PathDataType> paths;

        // private IEventSource eventSource;
        //private string resultOfLogHelper;
        private TestTask theOnlyTask;

        [Test]
        public void AfterTask()
        {
            // Setup mocks
            // LogHelper should subscribe to the events of IEventSource.
            // Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            // Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            // Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());
            // Expect.Call(() => logger.Initialize(null));
            // logger.Initialize(null);

            // Expect.Call(delegate { logger.Initialize(null); }).Constraints(Is.TypeOf<EventSource>()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            // Expect.Call(delegate { logger.Log(null, (BuildMessageEventArgs)null); }).Constraints(Is.TypeOf<TaskSetupEngine>(), Is.NotNull() && Property.Value("Message", "Initialized")).CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            var task = this.mocks.StrictMock<ITask>();
            var taskAttributes = new List<XmlAttribute>();

            var doc = new XmlDocument();
            var attr = doc.CreateAttribute("TestAttribute");
            attr.Value = "TheValue";
            taskAttributes.Add(attr);

            var testObject = new TaskSetupEngine(this.blogger, this.paths);
            this.blogger.BackToRecord();
            this.mocks.ReplayAll();

            testObject.AfterTask(task, taskAttributes);

            this.mocks.VerifyAll();
        }

        [Test]
        public void Construction()
        {
            Expect.Call(() => this.blogger.LogBuildMessage(null, null, null))
                .Constraints(
                Is.TypeOf<TaskSetupEngine>(),
                Is.NotNull(), 
                Is.Equal("Initialized"));

            this.mocks.ReplayAll();

            var testObject = new TaskSetupEngine(this.blogger, this.paths);

            this.mocks.VerifyAll();
            Assert.IsNotNull(testObject.DefaultBuildEngine);
        }

        [Test]
        public void ConstructionWithoutLogger()
        {
            this.mocks.ReplayAll();
            var testObject = new TaskSetupEngine(this.paths);

            // Assert.IsNotNull(testObject.
            this.mocks.VerifyAll();
        }

        [Test]
        public void ConstructionXXX()
        {
            string resultOfLogHelper = string.Empty;
            Action<string> logMethod = (log) => resultOfLogHelper += log;
            var logger = new TestLogger(logMethod);

            this.mocks.ReplayAll();
            var testObject = new TaskSetupEngine(logger, this.paths);
            Assert.EndsWith(resultOfLogHelper, "Initialized");

            this.mocks.VerifyAll();
        }

        [Test]
        public void ConstructorNullArgumentShouldThrow()
        {
            string resultOfLogHelper = string.Empty;
            Action<string> logMethod = (log) => resultOfLogHelper += log;
            Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine((IBuildLogger)null, null));
            Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine(new TestLogger(logMethod), null));
            Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine((ILogger)null, null));
            Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine(null));

            // Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine(null, this.paths));
        }

        [Test]
        public void DisposeTheTestObject()
        {
            var testObject = new TaskSetupEngine(this.blogger, this.paths);
            testObject.Dispose();
            Assert.Throws<InvalidOperationException>(() => testObject.Dispose());
        }

        [Test]
        public void ExecuteTask()
        {
            // Setup mocks
            // LogHelper should subscribe to the events of IEventSource.
            // Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            // Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            // Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());
            // Expect.Call(() => logger.Initialize(null));
            // logger.Initialize(null);
            var testObject = new TaskSetupEngine(this.blogger, this.paths);
            this.blogger.BackToRecord();

            Expect.Call(delegate { this.blogger.RegisterLogger(null); })
                .Constraints(Is.NotNull())
                .Repeat.Once()
                //.CallOriginalMethod(OriginalCallOptions.CreateExpectation)
                ;


            /*Expect.Call(delegate { this.logger.Log(null, (BuildMessageEventArgs)null); })
                .Constraints(
                    Is.TypeOf<Engine>(), 
                    Is.NotNull()
                ).Repeat.Times(5)
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation)
                ;*/

            /*Expect.Call(delegate { logger.Log(null, (BuildMessageEventArgs)null); })
                .Constraints(
                Is.TypeOf<TaskSetupEngine>(),
                Is.NotNull()
                ).Repeat.Times(0)
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation)
                ;*/

            /*
            Expect.Call(delegate { logger.Log(null, (BuildErrorEventArgs)null); })
                .Constraints(
                Is.TypeOf<Microsoft.Build.BuildEngine.Engine>(),
                Is.NotNull()
                ).Repeat.Any()
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            */
            // var task = mocks.StrictMock<ITask>();
            var taskAttributes = new List<XmlAttribute>();

            // var engine = mocks.Stub<Engine>("");
            // testObject.DefaultBuildEngine = engine;

            // var project = mocks.StrictMock<Project>();
            // Expect.Call(engine.CreateNewProject()).Return(project);
            // theOnlyTask.Expect(t => t.Execute());
            this.mocks.ReplayAll();

            // var testObject = new TaskSetupEngine(logger, paths);
            // logger.BackToRecord();
            // logger.Replay();
            testObject.ExecuteTask("NameNotRelevant", taskAttributes);

            Assert.IsTrue(TestTask.HasExecuted);
            Assert.AreEqual(1, TestTask.Instantiated);
            Assert.IsNotNull(TestTask.MyBuildEngine);
            Assert.IsNull(TestTask.MyHostObject);

            this.mocks.VerifyAll();

            // Assert.EndsWith(resultOfLogHelper, "Initialized");
        }


        [Test]
        public void LogBuildMessageEvent()
        {
            /*
            // Setup mocks
            IEventRaiser messageRaised = Expect.Call(delegate { eventSource.MessageRaised += null; }).IgnoreArguments().GetEventRaiser();
            Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());

            var args = mocks.StrictMock<BuildMessageEventArgs>("msg", "helpkey", "sender", MessageImportance.High);
            Expect.Call(args.Timestamp).Return(TestTimeStamp).Repeat.Twice();
            Expect.Call(args.ThreadId).Return(33);
            mocks.ReplayAll();

            // Test
            testObject.Initialize(eventSource);
            messageRaised.Raise(42, args);

            var actualTimestamp = args.Timestamp;
            Assert.AreEqual(TestTimeStamp, actualTimestamp);

            var expected = "33:sender:11/09/2001 08:46:13 msg";
            var actual = resultOfLogHelper;
            Assert.AreEqual(expected, actual);

            //TestContext.CurrentContext.LogWriter.Default.WriteLine(actualTimestamp.ToString(CultureInfo.InvariantCulture));
            //TestContext.CurrentContext.LogWriter.Default.Write(resultOfLogHelper);

            // Verify
            mocks.VerifyAll();
            */
        }

        [Test]
        public void PropertyIsDisposed()
        {
            var testObject = new TaskSetupEngine(this.blogger, this.paths);
            var expected = false;
            var actual = testObject.IsDisposed;
            Assert.AreEqual(expected, actual);

            expected = true;
            testObject.Dispose();
            actual = testObject.IsDisposed;
            Assert.AreEqual(expected, actual);
        }
    }
}