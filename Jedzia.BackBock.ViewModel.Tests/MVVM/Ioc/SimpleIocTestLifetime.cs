using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Jedzia.BackBock.ViewModel.MVVM.Ioc;
using Jedzia.BackBock.ViewModel.Tests.MVVM.Stubs;

namespace Jedzia.BackBock.ViewModel.Tests.MVVM.Ioc
{
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;

    [TestFixture]
    public class SimpleIocTestLifetime
    {
        [Test]
        public void TestCreationTimeForTransitionLifetime()
        {
            SimpleIoc.Default.Reset();
            TestClassForCreationTime.Reset();
            Assert.AreEqual(0, TestClassForCreationTime.InstancesCreated);
            SimpleIoc.Default.RegisterWithLifetime<TestClassForCreationTime>(new TransitionLifetime());
            Assert.AreEqual(0, TestClassForCreationTime.InstancesCreated);
            var inst1 = SimpleIoc.Default.GetInstance<TestClassForCreationTime>();
            Assert.AreEqual(1, TestClassForCreationTime.InstancesCreated);
            var inst2 = SimpleIoc.Default.GetInstance<TestClassForCreationTime>();
            Assert.AreEqual(2, TestClassForCreationTime.InstancesCreated);

            Assert.AreEqual(2, SimpleIoc.Default.GetAllInstances(typeof(TestClassForCreationTime)).Count());
            SimpleIoc.Default.Unregister(inst1);
            Assert.AreEqual(1, SimpleIoc.Default.GetAllInstances(typeof(TestClassForCreationTime)).Count());
            SimpleIoc.Default.Unregister(inst1);
            Assert.AreEqual(1, SimpleIoc.Default.GetAllInstances(typeof(TestClassForCreationTime)).Count());
            SimpleIoc.Default.Unregister(inst2);
            Assert.AreEqual(0, SimpleIoc.Default.GetAllInstances(typeof(TestClassForCreationTime)).Count());
        }

        [Test]
        public void TestCreationTimeForIDestructible()
        {
            SimpleIoc.Default.Reset();
            TestClassForIDestructible.Reset();
            Assert.AreEqual(0, TestClassForIDestructible.InstancesCreated);
            SimpleIoc.Default.RegisterWithLifetime<TestClassForIDestructible>(new TransitionLifetime());
            Assert.AreEqual(0, TestClassForIDestructible.InstancesCreated);

            var inst1 = SimpleIoc.Default.GetInstance<TestClassForIDestructible>();
            Assert.AreEqual(1, TestClassForIDestructible.InstancesCreated);
            Assert.IsNotNull(inst1);
            Assert.IsNotNull(inst1.Candidate);
            Assert.IsInstanceOfType<TransitionLifetime>(inst1.Candidate);
            Assert.AreEqual(1, SimpleIoc.Default.GetAllInstances(typeof(TestClassForIDestructible)).Count());
            
            inst1.Candidate.Release();
            Assert.AreEqual(0, SimpleIoc.Default.GetAllInstances(typeof(TestClassForIDestructible)).Count());
        }

        [Test]
        public void TestReleaseIDestructible()
        {
            SimpleIoc.Default.Reset();
            TestClassForIDestructible.Reset();
            TestClassForIDestructible releaseInstance = null;

            SimpleIoc.Default.RegisterWithLifetime<TestClassForIDestructible>(new TransitionLifetime())
                .OnDestroy((obj) => { releaseInstance = obj; });

            var inst1 = SimpleIoc.Default.GetInstance<TestClassForIDestructible>();

            Assert.AreEqual(1, SimpleIoc.Default.GetAllInstances(typeof(TestClassForIDestructible)).Count());
            inst1.Candidate.Release();
            Assert.AreEqual(0, SimpleIoc.Default.GetAllInstances(typeof(TestClassForIDestructible)).Count());
            Assert.AreEqual(inst1, releaseInstance);
        }

        [Test, Ignore]
        public void TestEventRegisterIDestructible()
        {
            SimpleIoc.Default.Reset();
            TestClassForIDestructible.Reset();
            TestClassForIDestructible releaseInstance = null;
            System.Windows.Window w = new System.Windows.Window();
            //SimpleIoc.Default.RegisterWithLifetime<TestClassForIDestructible>(new TransitionLifetime())
               // .DestroyOnEvent( (o) => w.Closed += o.DoRelease );
            
            var inst1 = SimpleIoc.Default.GetInstance<TestClassForIDestructible>();

            Assert.AreEqual(1, SimpleIoc.Default.GetAllInstances(typeof(TestClassForIDestructible)).Count());
            inst1.Candidate.Release();
            Assert.AreEqual(0, SimpleIoc.Default.GetAllInstances(typeof(TestClassForIDestructible)).Count());
            Assert.AreEqual(inst1, releaseInstance);
        }

    }
}
