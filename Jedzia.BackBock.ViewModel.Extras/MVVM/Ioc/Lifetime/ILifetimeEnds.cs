namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    /// <summary>
    /// Provides a release method for object destruction.
    /// </summary>
    public interface ILifetimeEnds
    {
        void Release();
    }
}