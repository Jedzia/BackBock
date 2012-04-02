using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Rhino.Mocks;
using Jedzia.BackBock.Model.Util;

namespace Jedzia.BackBock.Model.Tests.Util
{
    [TestFixture]
    public class TimeProviderTest
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            TimeProvider.ResetToDefault();
        }

        [Test]
        public void DefaultTimeProviderIsCorrect()
        {
            TimeProvider result = TimeProvider.Current;
            Assert.IsAssignableFrom<DefaultTimeProvider>(result.GetType());
        }

        [Test]
        public void CurrentIsWellBehavedWritableProperty()
        {
            TimeProvider expectedTimeProvider = mocks.StrictMock<TimeProvider>();
            TimeProvider.Current = expectedTimeProvider;
            TimeProvider result = TimeProvider.Current;
            Assert.AreEqual<TimeProvider>(expectedTimeProvider, result);
        }

        [Test]
        public void SettingCurrentToNullWillThrow()
        {
            Assert.Throws<ArgumentNullException>(() =>
                TimeProvider.Current = null);
        }
    }
}
