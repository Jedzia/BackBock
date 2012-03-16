namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   No tracking of component instances are made.
    /// </summary>
    [Serializable]
    public class NoTrackingReleasePolicy : IReleasePolicy
    {
        public void Dispose()
        {
        }

        public IReleasePolicy CreateSubPolicy()
        {
            return this;
        }

        public bool HasTrack(object instance)
        {
            return false;
        }

        public void Release(object instance)
        {
        }

        public void Track(object instance, Burden burden)
        {
        }
    }
}