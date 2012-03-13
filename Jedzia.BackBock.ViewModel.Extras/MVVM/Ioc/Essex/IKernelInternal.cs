namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections;

    public interface IKernelInternal : IKernel, IKernelEvents, IDisposable
    {
        IHandler LoadHandlerByType(string key, Type service, IDictionary arguments);
        /// <summary>
        ///   Returns a component instance by the key
        /// </summary>
        /// <param name = "key"></param>
        /// <param name = "service"></param>
        /// <param name = "arguments"></param>
        /// <param name = "policy"></param>
        /// <returns></returns>
        object Resolve(String key, Type service, IDictionary arguments, IReleasePolicy policy);
        object Resolve(Type service, IDictionary arguments, IReleasePolicy policy);
        
        IDisposable OptimizeDependencyResolution();

        /// <summary>
        ///   Adds a custom made <see cref = "ComponentModel" />.
        ///   Used by facilities.
        /// </summary>
        /// <param name = "model"></param>
        IHandler AddCustomComponent(ComponentModel model);
    }
}