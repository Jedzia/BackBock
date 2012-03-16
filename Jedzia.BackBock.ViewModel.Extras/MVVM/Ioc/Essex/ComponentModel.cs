namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Collections;
    using System.Threading;

    public sealed class ComponentModel //: GraphNode
    {

        /// <summary>
        ///   Gets or sets a value indicating whether the component requires generic arguments.
        /// </summary>
        /// <value>
        ///   <c>true</c> if generic arguments are required; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresGenericArguments { get; set; }
        
        public const string GenericImplementationMatchingStrategy = "generic.matching";

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private readonly ConstructorCandidateCollection constructors = new ConstructorCandidateCollection();

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private readonly LifecycleConcernsCollection lifecycle = new LifecycleConcernsCollection();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<Type> services = new List<Type>(4);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ComponentName componentName;

        //[NonSerialized]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private IDictionary customDependencies;

        /// <summary>
        ///   Dependencies the kernel must resolve
        /// </summary>
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private DependencyModelCollection dependencies;

        [NonSerialized]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary extendedProperties;

        /// <summary>
        ///   Interceptors associated
        /// </summary>
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private InterceptorReferenceCollection interceptors;

        /// <summary>
        ///   External parameters
        /// </summary>
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private ParameterModelCollection parameters;

        /// <summary>
        ///   All potential properties that can be setted by the kernel
        /// </summary>
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private PropertySetCollection properties;

        /// <summary>
        ///   Gets or sets the extended properties.
        /// </summary>
        /// <value>The extended properties.</value>
        [DebuggerDisplay("Count = {extendedProperties.Count}")]
        public IDictionary ExtendedProperties
        {
            get
            {
                var value = extendedProperties;
                if (value != null)
                {
                    return value;
                }
                value = new Arguments();
                var originalValue = Interlocked.CompareExchange(ref extendedProperties, value, null);
                return originalValue ?? value;
            }
        }

        /// <summary>
        ///   Add service to be exposed by this <see cref = "ComponentModel" />
        /// </summary>
        /// <param name = "type"></param>
        public void AddService(Type type)
        {
            if (type == null)
            {
                return;
            }
            if (type.IsValueType)
            {
                throw new ArgumentException("Type {0} is a value type and can not be used as a service.");
            }

            ComponentServicesUtil.AddService(services, type);
        }

        /// <summary>
        ///   Gets or sets the component implementation.
        /// </summary>
        /// <value>The implementation.</value>
        public Type Implementation { get; set; }

        public ComponentName ComponentName
        {
            get { return componentName; }
            internal set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                componentName = value; 
            }
        }

        /// <summary>
        ///   Gets or sets the lifestyle type.
        /// </summary>
        /// <value>The type of the lifestyle.</value>
        public LifestyleType LifestyleType { get; set; }

        /// <summary>
        ///   Gets or sets the custom lifestyle.
        /// </summary>
        /// <value>The custom lifestyle.</value>
        public Type CustomLifestyle { get; set; }
        
        /// <summary>
        ///   Sets or returns the component key
        /// </summary>
        public string Name
        {
            get { return componentName.Name; }
            set { componentName.SetName(value); }
        }

        [DebuggerDisplay("Count = {services.Count}")]
        public IEnumerable<Type> Services
        {
            get { return services; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool HasInterceptors
        {
            get
            {
                //var value = interceptors;
                return false;
                //return value != null && value.HasInterceptors;
            }
        }

        public IConfiguration Configuration { get; set; }
    }
}
