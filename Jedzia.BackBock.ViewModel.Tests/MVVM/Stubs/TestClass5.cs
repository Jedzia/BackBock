namespace Jedzia.BackBock.ViewModel.Tests.MVVM.Stubs
{
    using Jedzia.BackBock.ViewModel.MVVM.Ioc;

    public class TestClass5
    {
        public ITestClass MyProperty
        {
            get;
            private set;
        }

        public TestClass5()
        {
            
        }

        [PreferredConstructor]
        public TestClass5(ITestClass myProperty)
        {
            MyProperty = myProperty;
        }
    }
}
