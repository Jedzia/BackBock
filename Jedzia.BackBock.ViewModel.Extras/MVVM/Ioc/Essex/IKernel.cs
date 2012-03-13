namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections;

    public interface IKernel : IKernelEvents, IDisposable
    {

        
        /// <summary>
        ///   Returns the component instance by the component key
        /// </summary>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        ///   Returns the component instance by the service type
        ///   using dynamic arguments
        /// </summary>
        /// <param name = "service"></param>
        /// <param name = "arguments"></param>
        /// <returns></returns>
        object Resolve(Type service, IDictionary arguments);

        /// <summary>
        ///   Returns <c>true</c> if a component with given <paramref name = "name" /> was registered, otherwise <c>false</c>.
        /// </summary>
        /// <param name = "name"></param>
        /// <returns></returns>
        bool HasComponent(String name);
        
        /// <summary>
        ///   Returns the implementation of <see cref = "IComponentModelBuilder" />
        /// </summary>
        IComponentModelBuilder ComponentModelBuilder { get; }

        /// <summary>
        ///   Registers the components with the <see cref = "IKernel" />. The instances of <see cref = "IRegistration" /> are produced by fluent registration API.
        ///   Most common entry points are <see cref = "Component.For{TService}" /> method to register a single type or (recommended in most cases) 
        ///   <see cref = "AllTypes.FromThisAssembly" />.
        ///   Let the Intellisense drive you through the fluent API past those entry points. For details see the documentation at http://j.mp/WindsorApi
        /// </summary>
        /// <example>
        ///   <code>
        ///     kernel.Register(Component.For&lt;IService&gt;().ImplementedBy&lt;DefaultService&gt;().LifestyleTransient());
        ///   </code>
        /// </example>
        /// <example>
        ///   <code>
        ///     kernel.Register(Classes.FromThisAssembly().BasedOn&lt;IService&gt;().WithServiceDefaultInterfaces().Configure(c => c.LifestyleTransient()));
        ///   </code>
        /// </example>
        /// <param name = "registrations">The component registrations created by <see cref = "Component.For{TService}" />, <see
        ///    cref = "AllTypes.FromThisAssembly" /> or different entry method to the fluent API.</param>
        /// <returns>The kernel.</returns>
        IKernel Register(params IRegistration[] registrations);

        /// <summary>
        ///   Returns true if the specified service was registered
        /// </summary>
        /// <param name = "service"></param>
        /// <returns></returns>
        bool HasComponent(Type service);
    }
}