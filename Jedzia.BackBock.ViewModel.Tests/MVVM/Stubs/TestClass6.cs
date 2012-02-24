namespace Jedzia.BackBock.ViewModel.Tests.MVVM.Stubs
{
    public class TestClass6
    {
        public ITestClass MyProperty
        {
            get;
            set;
        }

        public TestClass6()
        {
            
        }

        public TestClass6(ITestClass myProperty)
        {
            MyProperty = myProperty;
        }
    }
}
