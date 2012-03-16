namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Indicates that the target components wants instance lifetime and reuse scope to be bound to another component further up the object graph.
    ///   Good scenario for this would be unit of work bound to a presenter in a two tier MVP application.
    ///   The <see cref = "ScopeRootBinderType" /> attribute must point to a type
    ///   having default accessible constructor and public method matching signature of <code>Func&lt;IHandler[], IHandler&gt;</code> delegate.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class BoundToAttribute : LifestyleAttribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BoundToAttribute" /> class.
        /// </summary>
        /// <param name = "scopeRootBinderType">type having default accessible constructor and public method matching signature of <code>Func&lt;IHandler[], IHandler&gt;</code> delegate. The method will be used to pick <see
        ///    cref = "IHandler" /> of the component current instance should be bound to.</param>
        public BoundToAttribute(Type scopeRootBinderType)
            : base(LifestyleType.Bound)
        {
            ScopeRootBinderType = scopeRootBinderType;
        }

        /// <summary>
        ///   type having default accessible constructor and public method matching signature of <code>Func&lt;IHandler[], IHandler&gt;</code> delegate. The method will be used to pick <see
        ///    cref = "IHandler" /> of the component current instance should be bound to.
        /// </summary>
        public Type ScopeRootBinderType { get; private set; }
    }
}