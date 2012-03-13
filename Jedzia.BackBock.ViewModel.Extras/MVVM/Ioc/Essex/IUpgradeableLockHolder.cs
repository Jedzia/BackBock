namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public interface IUpgradeableLockHolder : ILockHolder, IDisposable
    {
        // Methods
        ILockHolder Upgrade();
        ILockHolder Upgrade(bool waitForLock);
    }
}