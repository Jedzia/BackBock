namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public interface ILockHolder : IDisposable
    {
        // Properties
        bool LockAcquired { get; }
    }
}