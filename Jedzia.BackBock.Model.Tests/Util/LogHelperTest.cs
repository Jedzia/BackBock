using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Rhino.Mocks;
using Jedzia.BackBock.Model.Util;
using Microsoft.Build.Framework;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Interfaces;
using System.Globalization;

namespace Jedzia.BackBock.Model.Tests.Util
{
    [TestFixture]
    public class LogHelperTest
    {
        static LogHelperTest()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private static readonly DateTime TestTimeStamp = new DateTime(2001, 11, 9, 8, 46, 13);
        private static readonly string TimeStampString = TestTimeStamp.ToString(CultureInfo.InvariantCulture);
        private LogHelper testObject;
        private string resultOfLogHelper;
        private MockRepository mocks;
        private IEventSource eventSource;
        Action<string> logMethod;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            eventSource = mocks.StrictMock<IEventSource>();
            // LogHelperTest.Reset();

            resultOfLogHelper = string.Empty;
            logMethod = (log) => resultOfLogHelper += log;
            testObject = new LogHelper(logMethod);
        }

        [TearDown]
        public void TearDown()
        {
            // ioService.Close();
            // LogHelperTest.Dispose();

            testObject.Shutdown();
            mocks = null;
        }

        [Test]
        public void ConstructorNullArgumentShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new LogHelper(null));
        }

        [Test]
        public void InitializeTheLogHelper()
        {
            // Setup mocks
            // LogHelper should subscribe to the events of IEventSource.
            Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());
            mocks.ReplayAll();

            testObject.Initialize(eventSource);

            mocks.VerifyAll();
        }

        [Test]
        public void LogBuildMessageEvent()
        {
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
        }

        [Test]
        public void LogBuildWarning()
        {
            // Setup mocks
            Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            IEventRaiser messageRaised = Expect.Call(delegate { eventSource.WarningRaised += null; }).IgnoreArguments().GetEventRaiser();
            Expect.Call(delegate { eventSource.ErrorRaised += null; }).Constraints(Is.NotNull());

            //new BuildWarningEventArgs("subcategory", "code", "file", 1, 2, 3, 4, "message", "helpKeyword", "senderName");
            var args = mocks.StrictMock<BuildWarningEventArgs>("subcategory", "code", "directory\\file.txt", 1, 2, 3, 4, "message", "helpKeyword", "senderName");
            Expect.Call(args.Timestamp).Return(TestTimeStamp).Repeat.Twice();
            Expect.Call(args.ThreadId).Return(33);
            mocks.ReplayAll();

            // Test
            testObject.Initialize(eventSource);
            messageRaised.Raise(42, args);

            var actualTimestamp = args.Timestamp;
            Assert.AreEqual(TestTimeStamp, actualTimestamp);

            var expected = "33:senderName:11/09/2001 08:46:13 [code] Warning: directory\\file.txt(12):message";
            var actual = resultOfLogHelper;
            Assert.AreEqual(expected, actual);

            // Verify
            mocks.VerifyAll();
        }

        [Test]
        public void LogBuildError()
        {
            // Setup mocks
            Expect.Call(delegate { eventSource.MessageRaised += null; }).Constraints(Is.NotNull());
            Expect.Call(delegate { eventSource.WarningRaised += null; }).Constraints(Is.NotNull());
            IEventRaiser messageRaised = Expect.Call(delegate { eventSource.ErrorRaised += null; }).IgnoreArguments().GetEventRaiser();

            //new BuildErrorEventArgs("subcategory", "code", "file", 1, 2, 3, 4, "message", "helpKeyword", "senderName");
            var args = mocks.StrictMock<BuildErrorEventArgs>("subcategory", "code", "directory\\file.txt", 1, 2, 3, 4, "message", "helpKeyword", "senderName");
            Expect.Call(args.Timestamp).Return(TestTimeStamp).Repeat.Twice();
            Expect.Call(args.ThreadId).Return(33);
            mocks.ReplayAll();

            // Test
            testObject.Initialize(eventSource);
            messageRaised.Raise(42, args);

            var actualTimestamp = args.Timestamp;
            Assert.AreEqual(TestTimeStamp, actualTimestamp);

            var expected = "33:senderName:11/09/2001 08:46:13 [code]   Error: directory\\file.txt(12):message";
            var actual = resultOfLogHelper;
            Assert.AreEqual(expected, actual);

            // Verify
            mocks.VerifyAll();
        }

        [Test]
        public void PropertyParameters()
        {
            var expected = string.Empty;
            var actual = testObject.Parameters;
            Assert.AreEqual(expected, actual);

            testObject.Parameters = "Does Nothing";
            actual = testObject.Parameters;
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void PropertyVerbosity()
        {
            var expected = LoggerVerbosity.Detailed;
            var actual = testObject.Verbosity;
            Assert.AreEqual(expected, actual);

            testObject.Verbosity = LoggerVerbosity.Quiet;
            actual = testObject.Verbosity;
            Assert.AreEqual(expected, actual);
        }
    }
}
