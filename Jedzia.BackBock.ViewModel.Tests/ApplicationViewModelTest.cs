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
    public class ApplicationViewModelTest
    {

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
            ApplicationViewModel target = new ApplicationViewModel(ioService,
                new DesignMainWindow(), new DesignTaskService(), new DesignViewProvider());

            actual = ApplicationViewModel.MainIOService;
            Assert.AreSame(ioService, actual);
            //ApplicationViewModel.MainIOService.OpenFileDialog("c:\\tmp");
            mocks.VerifyAll();
        }

        [Test]
        public void MainWindowTest()
        {
            IMainWindow expected = mocks.StrictMock<IMainWindow>();
            mocks.ReplayAll();

            ApplicationViewModel target = new ApplicationViewModel(ioService,
                expected, new DesignTaskService(), new DesignViewProvider());

            mocks.VerifyAll();
        }

        [Test]
        public void TaskServiceTest()
        {
            ITaskService expected = mocks.StrictMock<ITaskService>();
            mocks.ReplayAll();

            ApplicationViewModel target = new ApplicationViewModel(ioService,
               new DesignMainWindow(), expected, new DesignViewProvider());

            Assert.AreEqual(expected, ApplicationViewModel.TaskService);
            mocks.VerifyAll();
        }

        [Test]
        public void TaskWizardProviderTest()
        {
            IViewProvider expected = mocks.StrictMock<IViewProvider>();
            mocks.ReplayAll();

            ApplicationViewModel target = new ApplicationViewModel(ioService,
               new DesignMainWindow(),new DesignTaskService(), expected);

            Assert.AreEqual(expected, ApplicationViewModel.TaskWizardProvider);
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
            ApplicationViewModel target = new ApplicationViewModel(ioService,
                new DesignMainWindow(), new DesignTaskService(), new DesignViewProvider());
            mocks.VerifyAll();
            Assert.Throws<ApplicationException>(() => target =
                new ApplicationViewModel(ioService, new DesignMainWindow(), 
                    new DesignTaskService(), new DesignViewProvider()));
        }


    }
}
