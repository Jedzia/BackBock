using System;
using Jedzia.BackBock.ViewModel.Commands;
using MbUnit.Framework;
using System.Windows.Input;

#if WIN8
using EventHandler = Windows.UI.Xaml.EventHandler;
#endif

namespace Jedzia.BackBock.ViewModel.Tests.Command
{
    [TestFixture]
    public class RelayCommandTest
    {
        [Test]
        public void TestCanExecuteChanged()
        {
            var command = new RelayCommand((e) =>
            {
            },
                                           (e) => true);

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
        public void TestCanExecute()
        {
            var canExecute = true;

            var command = new RelayCommand((e) =>
            {
            },
                                           (e) => canExecute);

            Assert.AreEqual(true, command.CanExecute(null));

            canExecute = false;

            Assert.AreEqual(false, command.CanExecute(null));
        }

        [Test]
        public void TestCanExecuteNull()
        {
            var command = new RelayCommand((e) =>
            {
            });

            Assert.AreEqual(true, command.CanExecute(null));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorInvalidExecuteNull1()
        {
            var command = new RelayCommand(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorInvalidExecuteNull2()
        {
            var command = new RelayCommand(null, null);
        }

        [Test]
        public void TestExecute()
        {
            var dummy = "Not executed";
            const string executed = "Executed";

            var command = new RelayCommand((e) =>
            {
                dummy = executed;
            });

            command.Execute(null);

            Assert.AreEqual(executed, dummy);
        }

        private bool _canExecute = true;

        [Test]
        public void TestCallingExecuteWhenCanExecuteIsFalse()
        {
            var counter = 0;

            var command = new RelayCommand(
                (e) => counter++,
                (e) => _canExecute);

            command.Execute(null);
            Assert.AreEqual(1, counter);
            _canExecute = false;
            command.Execute(null);
            Assert.AreEqual(1, counter);
            _canExecute = true;
            command.Execute(null);
            Assert.AreEqual(2, counter);
        }
    }
}