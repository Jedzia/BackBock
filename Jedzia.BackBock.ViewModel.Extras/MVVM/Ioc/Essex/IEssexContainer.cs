namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public interface IEssexContainer : IDisposable
    {
        IEssexContainer Install(params IEssexInstaller[] installers);
        IEssexContainer Register(params IRegistration[] registrations);

        /// <summary>
        ///   Returns a component instance by the service
        /// </summary>
        /// <typeparam name = "T">Service type</typeparam>
        /// <returns>The component instance</returns>
        T Resolve<T>();
    }
}