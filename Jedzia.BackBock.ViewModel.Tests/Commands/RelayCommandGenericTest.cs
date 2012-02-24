using System;
using Jedzia.BackBock.ViewModel.Commands;
using MbUnit.Framework;

#if WIN8
using EventHandler = Windows.UI.Xaml.EventHandler;
#endif

namespace Jedzia.BackBock.ViewModel.Tests.Command
{
    [TestFixture]
    public class RelayCommandGenericTest
    {
        [Test]
        public void CanExecuteChangedTest()
        {
            var command = new RelayCommand<string>(p =>
            {
            },
                                                   p => true);

            var canExecuteChangedCalled = 0;

            var canExecuteChangedEventHandler = new EventHandler((s, e) => canExecuteChangedCalled++);

            command.CanExecuteChanged += canExecuteChangedEventHandler;

            command.RaiseCanExecuteChanged();

            Assert.AreEqual(1, canExecuteChangedCalled);

            command.CanExecuteChanged -= canExecuteChangedEventHandler;

            command.RaiseCanExecuteChanged();

            Assert.AreEqual(1, canExecuteChangedCalled);
        }

        [Test]
        public void CanExecuteTest()
        {
            var command = new RelayCommand<string>(p =>
            {
            },
                                                   p => p == "CanExecute");

            Assert.AreEqual(true, command.CanExecute("CanExecute"));
            Assert.AreEqual(false, command.CanExecute("Hello"));
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void CanExecuteTestInvalidParameterType()
        {
            var command = new RelayCommand<string>(p =>
            {
            },
                                                   p => p == "CanExecute");

            command.CanExecute(DateTime.Now);
        }

        [Test]
        public void CanExecuteTestNull()
        {
            var command = new RelayCommand<string>(p =>
            {
            });

            Assert.AreEqual(true, command.CanExecute("Hello"));
        }

        [Test]
        public void CanExecuteTestNullParameter()
        {
            var command = new RelayCommand<string>(p =>
            {
            },
                                                   p => false);

            Assert.AreEqual(false, command.CanExecute(null));

            var command2 = new RelayCommand<string>(p =>
            {
            },
                                                   p => true);

            Assert.AreEqual(true, command2.CanExecute(null));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTestInvalidExecuteNull1()
        {
            var command = new RelayCommand<string>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTestInvalidExecuteNull2()
        {
            var command = new RelayCommand<string>(null, null);
        }

        [Test]
        public void ExecuteTest()
        {
            var dummy = "Not executed";
            const string executed = "Executed";
            const string parameter = "Parameter";

            var command = new RelayCommand<string>(p =>
            {
                dummy = executed + p;
            });

            command.Execute(parameter);

            Assert.AreEqual(executed + parameter, dummy);
        }

        private bool _canExecute = true;

        [Test]
        public void TestCallingExecuteWhenCanExecuteIsFalse()
        {
            var result = string.Empty;
            const string value1 = "Hello";
            const string value2 = "World";

            var command = new RelayCommand<string>(
                s => result = s,
                s => _canExecute);

            command.Execute(value1);
            Assert.AreEqual(value1, result);
            _canExecute = false;
            command.Execute(value2);
            Assert.AreEqual(value1, result);
            _canExecute = true;
            command.Execute(value2);
            Assert.AreEqual(value2, result);
        }
    }
}