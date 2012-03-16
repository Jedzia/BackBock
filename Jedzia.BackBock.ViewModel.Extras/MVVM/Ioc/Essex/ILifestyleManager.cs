namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   The <c>ILifestyleManager</c> implements 
    ///   a strategy for a given lifestyle, like singleton, per-thread
    ///   and transient.
    /// </summary>
    /// <remarks>
    ///   The responsibility of <c>ILifestyleManager</c>
    ///   is only the management of lifestyle. It should rely on
    ///   <see cref = "IComponentActivator" /> to obtain a new component instance
    /// </remarks>
    public interface ILifestyleManager : IDisposable
    {
        /// <summary>
        ///   Initializes the <c>ILifestyleManager</c> with the 
        ///   <see cref = "IComponentActivator" />
        /// </summary>
        /// <param name = "componentActivator"></param>
        /// <param name = "kernel"></param>
        /// <param name = "model"></param>
        //void Init(IComponentActivator componentActivator, IKernel kernel, ComponentModel model);

        /// <summary>
        ///   Implementors should release the component instance based
        ///   on the lifestyle semantic, for example, singleton components
        ///   should not be released on a call for release, instead they should
        ///   release them when disposed is invoked.
        /// </summary>
        /// <param name = "instance"></param>
        bool Release(object instance);

        /// <summary>
        ///   Implementors should return the component instance based on the lifestyle semantic.
        ///   Also the instance should be set to <see cref = "Burden.SetRootInstance" />, <see
        ///    cref = "Burden.RequiresPolicyRelease" /> should be also set if needed
        ///   and if a new instance was created it should be passed on to <see cref = "Track" /> of <paramref
        ///    name = "releasePolicy" />.
        /// </summary>
        /// <param name = "context" />
        /// <param name = "releasePolicy" />
        /// <returns></returns>
        //object Resolve(CreationContext context, IReleasePolicy releasePolicy);
    }
}