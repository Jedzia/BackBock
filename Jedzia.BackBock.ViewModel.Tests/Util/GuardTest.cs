using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Jedzia.BackBock.ViewModel.Util;
using System.Linq.Expressions;

namespace Jedzia.BackBock.ViewModel.Tests.Util
{
    [TestFixture]
    public class GuardTest
    {

        [Test]
        public void NotOutOfRangeInclusiveTest()
        {
            var parameter = 55;
            Assert.DoesNotThrow(() => Guard.NotOutOfRangeInclusive(() => parameter, parameter, 22, 99));
            Assert.DoesNotThrow(() => Guard.NotOutOfRangeInclusive(() => parameter, parameter, 55, 55));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => parameter, parameter, 56, 56));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => parameter, parameter, 54, 54));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => parameter, parameter, 56, 99));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => parameter, parameter, 01, 54));
        }

        [Test]
        public void NotOutOfRangeExclusiveTest()
        {
            var parameter = 55;
            Assert.DoesNotThrow(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 22, 99));
            Assert.DoesNotThrow(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 54, 56));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 01, 55));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 55, 56));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 56, 56));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 54, 54));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 56, 99));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => parameter, parameter, 01, 54));
        }


        [Test]
        public void NotNullTest()
        {
            object parameter = null;
            Assert.Throws<ArgumentNullException>(() => Guard.NotNull(() => parameter, parameter));
            
            parameter = "Not Empty";
            Guard.NotNull(() => parameter, parameter);

        }

    }
}
