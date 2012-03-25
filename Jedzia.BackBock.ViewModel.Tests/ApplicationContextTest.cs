using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Rhino.Mocks;
using Jedzia.BackBock.ViewModel.Design;
using Jedzia.BackBock.ViewModel.MainWindow;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.ViewModel.Tests
{
    [TestFixture]
    public class ApplicationContextTest
    {

        MockRepository mocks;
        IOService ioService;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            ioService = mocks.StrictMock<IOService>();
            ApplicationContext.Reset();
        }

        /// <summary>
        ///A test for MainIOService
        ///</summary>
        [Test]
        public void MainIOServiceTest()
        {
            //Assert.Throws<ApplicationException>(() =>
            //    actual = ApplicationContext.MainIOService);

            mocks.ReplayAll();
            
            // Todo mock the services.
            ApplicationContext target = new ApplicationContext(ioService, new DesignSettingsProvider(),
                new DesignMainWindow(), new DesignTaskService(), new DesignViewProvider());
            IOService actual = ioService;

            var expected = target.MainIOService;
            Assert.AreSame(expected, actual);
            //ApplicationViewModel.MainIOService.OpenFileDialog("c:\\tmp");
            mocks.VerifyAll();
        }

        [Test]
        public void MainWindowTest()
        {
            IMainWindow expected = mocks.StrictMock<IMainWindow>();
            mocks.ReplayAll();

            ApplicationContext target = new ApplicationContext(ioService, new DesignSettingsProvider(),
                expected, new DesignTaskService(), new DesignViewProvider());

            mocks.VerifyAll();
        }

        [Test]
        public void TaskServiceTest()
        {
            ITaskService expected = mocks.StrictMock<ITaskService>();
            mocks.ReplayAll();

            ApplicationContext target = new ApplicationContext(ioService, new DesignSettingsProvider(),
               new DesignMainWindow(), expected, new DesignViewProvider());

            Assert.AreEqual(expected, ApplicationContext.TaskService);
            mocks.VerifyAll();
        }

        [Test]
        public void TaskWizardProviderTest()
        {
            IViewProvider expected = mocks.StrictMock<IViewProvider>();
            mocks.ReplayAll();

            ApplicationContext target = new ApplicationContext(ioService, new DesignSettingsProvider(),
               new DesignMainWindow(),new DesignTaskService(), expected);

            Assert.AreEqual(expected, ApplicationContext.TaskWizardProvider);
            mocks.VerifyAll();
        }

        [Test]
        public void SettingsProviderTest()
        {
            ISettingsProvider expected = mocks.StrictMock<ISettingsProvider>();
            mocks.ReplayAll();

            ApplicationContext target = new ApplicationContext(ioService, expected,
               new DesignMainWindow(), new DesignTaskService(), new DesignViewProvider());

            //Assert.AreEqual(expected, ApplicationContext.);
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
            ApplicationContext target = new ApplicationContext(ioService, new DesignSettingsProvider(),
                new DesignMainWindow(), new DesignTaskService(), new DesignViewProvider());
            mocks.VerifyAll();
            Assert.Throws<ApplicationException>(() => target =
                new ApplicationContext(ioService, new DesignSettingsProvider(), new DesignMainWindow(),
                    new DesignTaskService(), new DesignViewProvider()));
        }


    }
}
