using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Rhino.Mocks;
using Microsoft.Build.Framework;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Interfaces;
using System.Globalization;
using Jedzia.BackBock.Model.Tasks;
using Jedzia.BackBock.Model.Data;
using System.Xml;
using Jedzia.BackBock.Model.Util;
using Jedzia.BackBock.Tasks;
using Microsoft.Build.BuildEngine;

namespace Jedzia.BackBock.Model.Tests.Tasks
{
    public class TestTask : ITask
    {
        public static bool HasExecuted;
        public IBuildEngine MyBuildEngine;
        public ITaskHost MyHostObject;

        #region ITask Members

        public static bool SExecute()
        {
            HasExecuted = true;
            return true;
        }

        public bool Execute()
        {
            return SExecute();
        }

        public IBuildEngine BuildEngine
        {
            get
            {
                return MyBuildEngine;
            }
            set
            {
                MyBuildEngine = value;
            }
        }

        public ITaskHost HostObject
        {
            get
            {
                return MyHostObject;
            }
            set
            {
                MyHostObject = value;
            }
        }

        #endregion
    }

    public class TestTaskContext : TaskContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestTaskContext"/> class.
        /// </summary>
        public TestTaskContext(ITask theOnlyOne)
        {
            this.taskService = new TestTaskService(theOnlyOne);
        }

        public class TestTaskService : ITaskService
        {
            ITask task;
            /// <summary>
            /// Initializes a new instance of the <see cref="T:TestTaskService"/> class.
            /// </summary>
            public TestTaskService(ITask theOnlyOne)
            {
                this.task = theOnlyOne;
            }

            #region ITaskService Members

            public ITask this[string taskName]
            {
                get { return this.task; }
            }

            public IEnumerable<string> GetRegisteredTasks()
            {
                throw new NotImplementedException();
            }

            public bool Register(ITask task)
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public void ResetAll()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private readonly ITaskService taskService;
        public override Jedzia.BackBock.Tasks.ITaskService TaskService
        {
            get { return taskService; }
        }
    }

    /// <summary>
    /// Helper-Wrapper for Logging Microsoft.Build.Framework <see cref="BuildEventArgs"/> events.
    /// </summary>
    public class TestLogger : ILogger
    {
        #region Fields

        /// <summary>
        /// For testing purposes. Switch logging of with <c>false</c>.
        /// </summary>
        private bool ENABLELOGGING = true;

        /// <summary>
        /// Holds a reference to the logging method.
        /// </summary>
        private readonly Action<string> log;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogHelper"/> class.
        /// </summary>
        /// <param name="log">The logging method to forward the messages, to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="log" /> is <c>null</c>.</exception>
        public TestLogger(Action<string> log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            this.log = log;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public string Parameters
        {
            get
            {
                return string.Empty;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>
        /// The verbosity.
        /// </value>
        public LoggerVerbosity Verbosity
        {
            get
            {
                return LoggerVerbosity.Detailed;
            }

            set
            {
            }
        }

        #endregion

        /// <summary>
        /// Initializes the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        public virtual void Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += Log;
            eventSource.WarningRaised += Log;
            eventSource.ErrorRaised += Log;
        }

        /// <summary>
        /// Shut down this instance.
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="e">The <see cref="Microsoft.Build.Framework.BuildErrorEventArgs"/> instance containing the event data.</param>
        public virtual void Log(object sender, BuildErrorEventArgs e)
        {
            if (ENABLELOGGING)
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}:{3}:{0} [{7}]   Error: {4}({5}{6}):{2}",
                    e.Timestamp,
                    e.ThreadId,
                    e.Message,
                    e.SenderName,
                    e.File,
                    e.LineNumber,
                    e.ColumnNumber,
                    e.Code);
                this.log(msg);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="e">The <see cref="Microsoft.Build.Framework.BuildWarningEventArgs"/> instance containing the event data.</param>
        public virtual void Log(object sender, BuildWarningEventArgs e)
        {
            if (ENABLELOGGING)
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}:{3}:{0} [{7}] Warning: {4}({5}{6}):{2}",
                    e.Timestamp,
                    e.ThreadId,
                    e.Message,
                    e.SenderName,
                    e.File,
                    e.LineNumber,
                    e.ColumnNumber,
                    e.Code);
                this.log(msg);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="e">The <see cref="Microsoft.Build.Framework.BuildMessageEventArgs"/> instance containing the event data.</param>
        public virtual void Log(object sender, BuildMessageEventArgs e)
        {
            if (ENABLELOGGING)
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}:{3}:{0} {2}",
                    e.Timestamp,
                    e.ThreadId,
                    e.Message,
                    e.SenderName);
                this.log(msg);
            }
        }
    }

    [TestFixture]
    public class TaskSetupEngineTest
    {
        private static readonly DateTime TestTimeStamp = new DateTime(2001, 11, 9, 8, 46, 13);
        private static readonly string TimeStampString = TestTimeStamp.ToString(CultureInfo.InvariantCulture);
        //private TaskSetupEngine testObject;
        private MockRepository mocks;
        private TestLogger logger;
        private IEnumerable<PathDataType> paths;
        //private IEventSource eventSource;
        private string resultOfLogHelper;
        private TestTask theOnlyTask;
        //private Engine engine;

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            //theOnlyTask = MockRepository.GenerateStrictMock<ITask>();
            theOnlyTask = new TestTask();
            TaskContext.Default = new TestTaskContext(theOnlyTask);
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
            //this.logger = mocks.StrictMock<ILogger>();
            //eventSource = mocks.StrictMock<IEventSource>();
            resultOfLogHelper = string.Empty;
            Action<string> logMethod = (log) => resultOfLogHelper += log + Environment.NewLine;
            //this.logger = mocks.StrictMock<TestLogger>(new LogHelper(logMethod), eventSource);
            //this.logger = new TestLogger(logMethod);
            this.logger = mocks.StrictMock<TestLogger>(logMethod);

            this.paths = new[] { new PathDataType() };

        }

        [TearDown]
        public void TearDown()
        {
            this.logger = null;
            this.paths = null;
            //if (!testObject.IsDisposed)
            //{
            //    testObject.Dispose();
            //}
            mocks = null;
        }

        [Test]
        public void ConstructorNullArgumentShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine(null, null));
            Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine(this.logger, null));
            //Assert.Throws<ArgumentNullException>(() => new TaskSetupEngine(null, this.paths));
        }

        [Test]
        public void Construction()
        {
            //logger.BackToRecord();
            //logger.Expect((e) => e.Initialize(null)).Constraints(Is.NotNull());
            var evs = new EventSource();
            //logger.Initialize(evs);

            //Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());

            Expect.Call(delegate { logger.Initialize(null); }).Constraints(Is.TypeOf<EventSource>())
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation)
               ;

            // new BuildMessageEventArgs("a", "b", "c", MessageImportance.High)

            Expect.Call(delegate
            {
                logger.Log(null, (BuildMessageEventArgs)null);
            }).Constraints(Is.TypeOf<TaskSetupEngine>(), Is.NotNull() && Property.Value("Message", "Initialized"))
            .CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            //LastCall.Constraints(Is.TypeOf<EventSource>());

            //var resultOfLogHelper = string.Empty;
            //Action<string> logMethod = (log) => resultOfLogHelper += log;
            //logger = new LogHelper(logMethod);

            //LastCall.Return(null);
            //Expect.Call(delegate { logger.Initialize(null); }); 
            //logger.Initialize(null);
            //logger.Expect((e) => e.Initialize(null));
            string count;
            //logger.MessageRaised += (sender, args) => { count += "MessageRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };

            mocks.ReplayAll();
            var testObject = new TaskSetupEngine(logger, paths);
            //eventSource.BackToRecord();
            Assert.EndsWith(resultOfLogHelper, "Initialized");

            //logger.VerifyAllExpectations();
            mocks.VerifyAll();
        }

        [Test]
        public void ConstructionXXX()
        {
            Action<string> logMethod = (log) => resultOfLogHelper += log;
            this.logger = new TestLogger(logMethod);

            mocks.ReplayAll();
            var testObject = new TaskSetupEngine(logger, paths);
            Assert.EndsWith(resultOfLogHelper, "Initialized");

            mocks.VerifyAll();
        }

        [Test]
        public void ConstructionWithoutLogger()
        {
            mocks.ReplayAll();
            var testObject = new TaskSetupEngine(null, paths);

            //Assert.IsNotNull(testObject.
            mocks.VerifyAll();
        }

        [Test]
        public void AfterTask()
        {

            // Setup mocks
            // LogHelper should subscribe to the events of IEventSource.
            //Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(() => logger.Initialize(null));
            //logger.Initialize(null);

            //Expect.Call(delegate { logger.Initialize(null); }).Constraints(Is.TypeOf<EventSource>()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            //Expect.Call(delegate { logger.Log(null, (BuildMessageEventArgs)null); }).Constraints(Is.TypeOf<TaskSetupEngine>(), Is.NotNull() && Property.Value("Message", "Initialized")).CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            var task = mocks.StrictMock<ITask>();
            var taskAttributes = new List<XmlAttribute>();
            var testObject = new TaskSetupEngine(logger, paths);
            mocks.ReplayAll();

            logger.BackToRecord();
            logger.Replay();
            testObject.AfterTask(task, taskAttributes);

            mocks.VerifyAll();

        }

        [Test]
        public void ExecuteTask()
        {

            // Setup mocks
            // LogHelper should subscribe to the events of IEventSource.
            //Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());
            //Expect.Call(() => logger.Initialize(null));
            //logger.Initialize(null);

            var testObject = new TaskSetupEngine(logger, paths);
            logger.BackToRecord();

            Expect.Call(delegate { logger.Initialize(null); })
                .Constraints(Is.NotNull() && !Is.TypeOf<EventSource>())
                .Repeat.Once()
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation);


            Expect.Call(delegate { logger.Log(null, (BuildMessageEventArgs)null); })
                .Constraints(
                Is.TypeOf<Microsoft.Build.BuildEngine.Engine>(),
                Is.NotNull()
                ).Repeat.Times(5)
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation)
                ;

            Expect.Call(delegate { logger.Log(null, (BuildMessageEventArgs)null); })
                .Constraints(
                Is.TypeOf<TaskSetupEngine>(),
                Is.NotNull()
                ).Repeat.Any()
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation)
                ;

            /*
            Expect.Call(delegate { logger.Log(null, (BuildErrorEventArgs)null); })
                .Constraints(
                Is.TypeOf<Microsoft.Build.BuildEngine.Engine>(),
                Is.NotNull()
                ).Repeat.Any()
                .CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            */
            //var task = mocks.StrictMock<ITask>();
            var taskAttributes = new List<XmlAttribute>();

            //var engine = mocks.Stub<Engine>("");
            //testObject.DefaultBuildEngine = engine;

            //var project = mocks.StrictMock<Project>();
            //Expect.Call(engine.CreateNewProject()).Return(project);
            //theOnlyTask.Expect(t => t.Execute());

            mocks.ReplayAll();

            //var testObject = new TaskSetupEngine(logger, paths);
            //logger.BackToRecord();
            //logger.Replay();
            testObject.ExecuteTask("NameNotRelevant", taskAttributes);

            Assert.IsTrue(TestTask.HasExecuted);

            mocks.VerifyAll();
            //Assert.EndsWith(resultOfLogHelper, "Initialized");

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
            var testObject = new TaskSetupEngine(logger, paths);
            var expected = false;
            var actual = testObject.IsDisposed;
            Assert.AreEqual(expected, actual);

            expected = true;
            testObject.Dispose();
            actual = testObject.IsDisposed;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DisposeTheTestObject()
        {
            var testObject = new TaskSetupEngine(logger, paths);
            testObject.Dispose();
            Assert.Throws<InvalidOperationException>(() => testObject.Dispose());
        }
    }
}
