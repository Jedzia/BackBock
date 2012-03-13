namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    /// <summary>
    /// Provides lifetime destruction abilities to the <see cref="SimpleIoc"/> container for the implementor.
    /// </summary>
    public interface IDestructible
    {
        ILifetimeEnds Candidate { get; set; }
    }
}