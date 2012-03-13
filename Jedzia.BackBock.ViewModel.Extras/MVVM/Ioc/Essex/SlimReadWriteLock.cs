namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public class SlimReadWriteLock : Lock
    {
        public override ILockHolder ForReading()
        {
            throw new NotImplementedException();
        }

        public override ILockHolder ForReading(bool waitForLock)
        {
            throw new NotImplementedException();
        }

        public override IUpgradeableLockHolder ForReadingUpgradeable()
        {
            throw new NotImplementedException();
        }

        public override IUpgradeableLockHolder ForReadingUpgradeable(bool waitForLock)
        {
            throw new NotImplementedException();
        }

        public override ILockHolder ForWriting()
        {
            // Todo: impl.
            throw new NotImplementedException();
        }

        public override ILockHolder ForWriting(bool waitForLock)
        {
            throw new NotImplementedException();
        }
    }
}