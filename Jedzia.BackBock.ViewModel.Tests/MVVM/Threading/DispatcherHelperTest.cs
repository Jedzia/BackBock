namespace Jedzia.BackBock.ViewModel.Tests.MVVM.Threading
{
    using System.Windows.Controls;
    using System.Threading;
    using Jedzia.BackBock.ViewModel.MVVM.Threading;
    using MbUnit.Framework;

    [TestFixture]
    public class TestDispatcherHelper
    {
        private Button _button;

        [Test]
        public void TestDispatchingToUiThread()
        {
            _button = new Button
            {
                Content = "Content1"
            };

            var manualEvent = new ManualResetEvent(false);

            var thread = new Thread(() =>
            {
                Thread.Sleep(500);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    AccessMethodOnUiThread(NewContent);
                    manualEvent.Set();
                });
            });

            DispatcherHelper.Initialize();

            thread.Start();

            manualEvent.WaitOne(1000);

#if SILVERLIGHT
            // No way to verify that the button is correctly set
            // in WPF, however the mere fact that we didn't get an exception
            // is an indication of success.
            DispatcherHelper.UIDispatcher.BeginInvoke(VerifyButtonNewContent);
#endif
        }

        private void AccessMethodOnUiThread(string newContent)
        {
            _button.Content = newContent;
        }

        [Test]
        public void TestDirectAccessToUiThread()
        {
            _button = new Button
            {
                Content = "Content1"
            };

            DispatcherHelper.Initialize();
            DispatcherHelper.CheckBeginInvokeOnUI(() => _button.Content = NewContent);

#if SILVERLIGHT
            // No way to verify that the button is correctly set
            // in WPF, however the mere fact that we didn't get an exception
            // is an indication of success.
            Assert.AreEqual(NewContent, _button.Content);
#endif
        }

        private const string NewContent = "New content";

        private void VerifyButtonNewContent()
        {
            Assert.AreEqual(NewContent, _button.Content);
        }
    }
}
