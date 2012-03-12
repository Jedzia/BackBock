using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    public class ComponentName
    {
        public ComponentName(string name, bool setByUser)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            Name = name;
            SetByUser = setByUser;
        }

        public string Name { get; private set; }
        public bool SetByUser { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        internal void SetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            Name = value;
            SetByUser = true;
        }

        /// <summary>
        ///   Gets the default name for component implemented by <paramref name = "componentType" /> which will be used in case when user does not provide one explicitly.
        /// </summary>
        /// <param name = "componentType"></param>
        /// <returns></returns>
        public static ComponentName DefaultFor(Type componentType)
        {
            return new ComponentName(DefaultNameFor(componentType), false);
        }

        /// <summary>
        ///   Gets the default name for component implemented by <paramref name = "componentType" /> which will be used in case when user does not provide one explicitly.
        /// </summary>
        /// <param name = "componentType"></param>
        /// <returns></returns>
        public static string DefaultNameFor(Type componentType)
        {
            return componentType.FullName;
        }
    }

    public sealed class ComponentModel //: GraphNode
    {

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

    }
}
