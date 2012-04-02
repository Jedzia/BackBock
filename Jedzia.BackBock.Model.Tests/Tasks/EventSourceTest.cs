using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Jedzia.BackBock.Model.Util;
using System.Globalization;
using Jedzia.BackBock.Model.Tasks;
using Microsoft.Build.Framework;
using Rhino.Mocks;
using System.Linq.Expressions;

namespace Jedzia.BackBock.Model.Tests.Tasks
{
    [TestFixture]
    public class EventSourceTest
    {
        private EventSource testObject;
        private string count;

        [SetUp]
        public void SetUp()
        {
            count = string.Empty;
            testObject = new EventSource();
        }

        [Test]
        public void ConstructorDefault()
        {
            var expected = false;
            var actual = testObject.OnlyLogCriticalEvents;
            Assert.AreEqual(expected, actual);
        }

        public void SubscribeToAllEvents()
        {
            testObject.AnyEventRaised += (sender, args) => { count += "AnyEventRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.BuildFinished += (sender, args) => { count += "BuildFinished"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.BuildStarted += (sender, args) => { count += "BuildStarted"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.CustomEventRaised += (sender, args) => { count += "CustomEventRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.ErrorRaised += (sender, args) => { count += "ErrorRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.MessageRaised += (sender, args) => { count += "MessageRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.ProjectFinished += (sender, args) => { count += "ProjectFinished"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.ProjectStarted += (sender, args) => { count += "ProjectStarted"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.StatusEventRaised += (sender, args) => { count += "StatusEventRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.TargetFinished += (sender, args) => { count += "TargetFinished"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.TargetStarted += (sender, args) => { count += "TargetStarted"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.TaskFinished += (sender, args) => { count += "TaskFinished"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.TaskStarted += (sender, args) => { count += "TaskStarted"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            testObject.WarningRaised += (sender, args) => { count += "WarningRaised"; Assert.AreEqual(this, sender); Assert.IsNull(args); };
        }

        [Test]
        public void AnyEventRaised()
        {
            //var args = MockRepository.GenerateStub<BuildEventArgs>();

            //testObject.AnyEventRaised += (sender, args) => { count++; Assert.AreEqual(this, sender); Assert.IsNull(args); };
            SubscribeToAllEvents();
            testObject.FireAnyEvent(this, null);
            Assert.AreEqual("AnyEventRaised", this.count);

            // args.VerifyAllExpectations();
        }

        [Test]
        public void FireBuildFinished()
        {
            SubscribeToAllEvents();
            testObject.FireBuildFinished(this, null);
            Assert.AreEqual("BuildFinishedAnyEventRaised", this.count);
        }

        [Test]
        public void FireBuildStarted()
        {
            SubscribeToAllEvents();
            testObject.FireBuildStarted(this, null);
            Assert.AreEqual("BuildStartedAnyEventRaised", this.count);
        }

        [Test]
        public void FireCustomEventRaised()
        {
            SubscribeToAllEvents();
            testObject.FireCustomEventRaised(this, null);
            Assert.AreEqual("CustomEventRaisedAnyEventRaised", this.count);
        }

        [Test]
        public void FireErrorRaised()
        {
            SubscribeToAllEvents();
            testObject.FireErrorRaised(this, null);
            Assert.AreEqual("ErrorRaisedAnyEventRaised", this.count);
        }

        [Test]
        public void FireMessageRaised()
        {
            SubscribeToAllEvents();
            testObject.FireMessageRaised(this, null);
            Assert.AreEqual("MessageRaisedAnyEventRaised", this.count);
        }

        [Test]
        public void FireProjectFinished()
        {
            SubscribeToAllEvents();
            testObject.FireProjectFinished(this, null);
            Assert.AreEqual("ProjectFinishedAnyEventRaised", this.count);
        }

        [Test]
        public void FireProjectStarted()
        {
            SubscribeToAllEvents();
            testObject.FireProjectStarted(this, null);
            Assert.AreEqual("ProjectStartedAnyEventRaised", this.count);
        }

        [Test]
        public void FireTargetFinished()
        {
            SubscribeToAllEvents();
            testObject.FireTargetFinished(this, null);
            Assert.AreEqual("TargetFinishedAnyEventRaised", this.count);
        }

        [Test]
        public void FireTargetStarted()
        {
            SubscribeToAllEvents();
            testObject.FireTargetStarted(this, null);
            Assert.AreEqual("TargetStartedAnyEventRaised", this.count);
        }

        [Test]
        public void FireTaskFinished()
        {
            SubscribeToAllEvents();
            testObject.FireTaskFinished(this, null);
            Assert.AreEqual("TaskFinishedAnyEventRaised", this.count);
        }

        [Test]
        public void FireTaskStarted()
        {
            SubscribeToAllEvents();
            testObject.FireTaskStarted(this, null);
            Assert.AreEqual("TaskStartedAnyEventRaised", this.count);
        }

        [Test]
        public void FireWarningRaised()
        {
            SubscribeToAllEvents();
            testObject.FireWarningRaised(this, null);
            Assert.AreEqual("WarningRaisedAnyEventRaised", this.count);
        }

        [Test]
        public void PropertyOnlyLogCriticalEvents()
        {
            var expected = false;
            var actual = testObject.OnlyLogCriticalEvents;
            Assert.AreEqual(expected, actual);

            expected = true;
            testObject.OnlyLogCriticalEvents = expected;
            actual = testObject.OnlyLogCriticalEvents;
            Assert.AreEqual(expected, actual);
        }
    }
}
