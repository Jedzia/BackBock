namespace Jedzia.BackBock.ViewModel.Tests.MVVM.Stubs
{
    public class TestClass3
    {
        public ITestClass SavedProperty
        {
            get;
            set;
        }

        public TestClass3(ITestClass parameter)
        {
            SavedProperty = parameter;
        }
    }
}
