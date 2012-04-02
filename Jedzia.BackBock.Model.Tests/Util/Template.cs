using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Jedzia.BackBock.Model.Util;

namespace Jedzia.BackBock.Model.Tests.Util
{
    [TestFixture]
    public class Template
    {
        // private LogHelper testObject;
        // private string resultOfLogHelper;

        [SetUp]
        public void SetUp()
        {
            //resultOfLogHelper = string.Empty;
            //testObject = new LogHelper(log => resultOfLogHelper += log);
        }

        [TearDown]
        public void TearDown()
        {
            // resultOfLogHelper = string.Empty;
            // LogHelper.Dispose();
            //testObject.Shutdown();
            //testObject = null;
        }

        [Test]
        public void NullAssignmentShouldThrow()
        {
            //Assert.Throws<ArgumentNullException>(() => new LogHelper(null));
        }

        [Test]
        public void DefaultContextShouldDoSomething()
        {
            //
            // TODO: Add test logic here
            //
            // var to = new LogHelperTest( ... );
            //var expected = "BlaBlaBla";
            //testObject.Initialize(null);
            //var actual = testObject.ToString();
            //Assert.AreEqual(expected, actual);
        }
    }
}
