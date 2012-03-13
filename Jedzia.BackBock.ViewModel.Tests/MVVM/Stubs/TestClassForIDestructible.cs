using Jedzia.BackBock.ViewModel.MVVM.Ioc;
namespace Jedzia.BackBock.ViewModel.Tests.MVVM.Stubs
{
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;

    public class TestClassForIDestructible : ITestClass, IDestructible
    {
        public static int InstancesCreated
        {
            get;
            private set;
        }

        public TestClassForIDestructible()
        {
            InstancesCreated++;
        }

        public static void Reset()
        {
            InstancesCreated = 0;
        }

        #region IDestructible Members

        private ILifetimeEnds candidate;
        public ILifetimeEnds Candidate
        {
            get
            {
                return this.candidate;
            }

            set
            {
                this.candidate = value;
            }
        }

        #endregion
    }
}
