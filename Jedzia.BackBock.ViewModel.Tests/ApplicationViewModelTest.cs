using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Rhino.Mocks;

namespace Jedzia.BackBock.ViewModel.Tests
{
    [TestFixture]
    public class ApplicationViewModelTest
    {

        enum MyEnum { ValueOne, ValueTwo }

        /// <summary>
        /// Summary
        /// </summary>
        public class MyClass
        {

        }

        MockRepository mocks;
        IOService ioService;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            ioService = mocks.StrictMock<IOService>();
            ApplicationViewModel.Reset();
        }

        /// <summary>
        ///A test for RegisterControl
        ///</summary>
        [Test]
        public void RegisterControlTest()
        {
            Enum kind = MyEnum.ValueOne; 
            Type type = typeof(MyClass);
// Todo: move the ControlRegistrator stuff to its own test class.
            Assert.Throws<ArgumentNullException>(() => ControlRegistrator.RegisterControl(null, type));
            Assert.Throws<ArgumentNullException>(() => ControlRegistrator.RegisterControl(kind, null));
            ControlRegistrator.RegisterControl(kind, type);

            var instance = ControlRegistrator.GetInstanceOfType<MyClass>(kind);
            Assert.IsInstanceOfType(type, instance);

            Assert.Throws<KeyNotFoundException>(() => ControlRegistrator.GetInstanceOfType<MyClass>(MyEnum.ValueTwo));
        }

        /// <summary>
        ///A test for MainIOService
        ///</summary>
        [Test]
        public void MainIOServiceTest()
        {
            IOService actual;
            Assert.Throws<ApplicationException>(() =>
                actual = ApplicationViewModel.MainIOService);

            mocks.ReplayAll();
            
            // Todo mock the services.
            ApplicationViewModel target = new ApplicationViewModel(ioService, null);

            actual = ApplicationViewModel.MainIOService;
            Assert.AreSame(ioService, actual);
            //ApplicationViewModel.MainIOService.OpenFileDialog("c:\\tmp");
            mocks.VerifyAll();
        }

        /*/// <summary>
        ///A test for ApplicationCommands
        ///</summary>
        [Test]
        public void ApplicationCommandsTest()
        {
            mocks.ReplayAll();
            ApplicationViewModel target = new ApplicationViewModel(ioService);
            var actual = target.ApplicationCommands;
            Assert.IsNull(actual);
            mocks.VerifyAll();
        }*/

        /*/// <summary>
        ///A test for NotifyPropertyChanged
        ///</summary>
        [Test]
        public void NotifyPropertyChangedTest()
        {
            ApplicationViewModel target = new ApplicationViewModel(ioService); // TODO: Initialize to an appropriate value
            string propertyName = string.Empty; // TODO: Initialize to an appropriate value
        }*/

        /// <summary>
        ///A test for ApplicationViewModel Constructor
        ///</summary>
        [Test]
        public void ApplicationViewModelConstructorTest()
        {
            mocks.ReplayAll();
            ApplicationViewModel target = new ApplicationViewModel(ioService, null);
            mocks.VerifyAll();
            Assert.Throws<ApplicationException>(() => target = new ApplicationViewModel(ioService, null));
        }


    }
}
