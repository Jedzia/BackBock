namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    /// <summary>
    ///   Implementors should use a strategy to obtain 
    ///   valid references to properties and/or services 
    ///   requested in the dependency model.
    /// </summary>
    public interface ISubDependencyResolver
    {
        /// <summary>
        ///   Returns true if the resolver is able to satisfy this dependency.
        /// </summary>
        /// <param name = "context">Creation context, which is a resolver itself</param>
        /// <param name = "contextHandlerResolver">Parent resolver - normally the IHandler implementation</param>
        /// <param name = "model">Model of the component that is requesting the dependency</param>
        /// <param name = "dependency">The dependency model</param>
        /// <returns><c>true</c> if the dependency can be satisfied</returns>
        bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency);

        /// <summary>
        ///   Should return an instance of a service or property values as
        ///   specified by the dependency model instance. 
        ///   It is also the responsibility of <see cref = "IDependencyResolver" />
        ///   to throw an exception in the case a non-optional dependency 
        ///   could not be resolved.
        /// </summary>
        /// <param name = "context">Creation context, which is a resolver itself</param>
        /// <param name = "contextHandlerResolver">Parent resolver - normally the IHandler implementation</param>
        /// <param name = "model">Model of the component that is requesting the dependency</param>
        /// <param name = "dependency">The dependency model</param>
        /// <returns>The dependency resolved value or null</returns>
        object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency);
    }
}