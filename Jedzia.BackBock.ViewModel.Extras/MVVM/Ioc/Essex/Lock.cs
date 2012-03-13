namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public abstract class Lock
    {
        // Methods
        protected Lock()
        {
        }

        public static Lock Create()
        {
            return new SlimReadWriteLock();
        }

        public abstract ILockHolder ForReading();
        public abstract ILockHolder ForReading(bool waitForLock);
        public abstract IUpgradeableLockHolder ForReadingUpgradeable();
        public abstract IUpgradeableLockHolder ForReadingUpgradeable(bool waitForLock);
        public abstract ILockHolder ForWriting();
        public abstract ILockHolder ForWriting(bool waitForLock);
    }
}