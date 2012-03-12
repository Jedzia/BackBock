using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    /// <summary>
    ///   Registration for a single type as a component with the kernel.
    ///   <para />
    ///   You can create a new registration with the <see cref = "Component" /> factory.
    /// </summary>
    /// <typeparam name = "TService">The service type</typeparam>
    public class ComponentRegistration<TService> : IRegistration
        where TService : class
    {
        private readonly List<IComponentModelDescriptor> descriptors = new List<IComponentModelDescriptor>();
        private readonly List<Type> potentialServices = new List<Type>();

        private bool ifComponentRegisteredIgnore;
        private Type implementation;
        private ComponentName name;
        private bool overwrite;
        private bool registerNewServicesOnly;
        private bool registered;


        /// <summary>
        ///   Initializes a new instance of the <see cref = "ComponentRegistration{TService}" /> class.
        /// </summary>
        public ComponentRegistration()
            : this(typeof(TService))
        {
        }
        internal void RegisterOptionally()
        {
            ifComponentRegisteredIgnore = true;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ComponentRegistration{TService}" /> class.
        /// </summary>
        public ComponentRegistration(params Type[] services)
        {
            Forward(services);
        }

        /// <summary>
        ///   Adds <paramref name = "types" /> as additional services to be exposed by this component.
        /// </summary>
        /// <param name = "types">The types to forward.</param>
        /// <returns></returns>
        public ComponentRegistration<TService> Forward(params Type[] types)
        {
            return Forward((IEnumerable<Type>)types);
        }

        /// <summary>
        ///   Adds <typeparamref name = "TService2" /> as additional service to be exposed by this component.
        /// </summary>
        /// <typeparam name = "TService2">The forwarded type.</typeparam>
        /// <returns>The component registration.</returns>
        public ComponentRegistration<TService> Forward<TService2>()
        {
            return Forward(new[] { typeof(TService2) });
        }

        /// <summary>
        ///   Adds <typeparamref name = "TService2" /> and <typeparamref name = "TService3" /> as additional services to be exposed by this component.
        /// </summary>
        /// <typeparam name = "TService2">The first forwarded type.</typeparam>
        /// <typeparam name = "TService3">The second forwarded type.</typeparam>
        /// <returns>The component registration.</returns>
        public ComponentRegistration<TService> Forward<TService2, TService3>()
        {
            return Forward(new[] { typeof(TService2), typeof(TService3) });
        }

        /// <summary>
        ///   Adds <typeparamref name = "TService2" />, <typeparamref name = "TService3" /> and  <typeparamref name = "TService4" /> as additional services to be exposed by this component.
        /// </summary>
        /// <typeparam name = "TService2">The first forwarded type.</typeparam>
        /// <typeparam name = "TService3">The second forwarded type.</typeparam>
        /// <typeparam name = "TService4">The third forwarded type.</typeparam>
        /// <returns>The component registration.</returns>
        public ComponentRegistration<TService> Forward<TService2, TService3, TService4>()
        {
            return Forward(new[] { typeof(TService2), typeof(TService3), typeof(TService4) });
        }

        /// <summary>
        ///   Adds <typeparamref name = "TService2" />, <typeparamref name = "TService3" />,<typeparamref name = "TService4" /> and  <typeparamref
        ///     name = "TService5" /> as additional services to be exposed by this component.
        /// </summary>
        /// <typeparam name = "TService2">The first forwarded type.</typeparam>
        /// <typeparam name = "TService3">The second forwarded type.</typeparam>
        /// <typeparam name = "TService4">The third forwarded type.</typeparam>
        /// <typeparam name = "TService5">The fourth forwarded type.</typeparam>
        /// <returns>The component registration.</returns>
        public ComponentRegistration<TService> Forward<TService2, TService3, TService4, TService5>()
        {
            return Forward(new[] { typeof(TService2), typeof(TService3), typeof(TService4), typeof(TService5) });
        }

        /// <summary>
        ///   Adds <paramref name = "types" /> as additional services to be exposed by this component.
        /// </summary>
        /// <param name = "types">The types to forward.</param>
        /// <returns></returns>
        public ComponentRegistration<TService> Forward(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                ComponentServicesUtil.AddService(potentialServices, type);
            }
            return this;
        }

        /// <summary>
        ///   The concrete type that implements the service.
        ///   <para />
        ///   To set the implementation, use <see cref = "ImplementedBy(System.Type)" />.
        /// </summary>
        /// <value>The implementation of the service.</value>
        public Type Implementation
        {
            get { return implementation; }
        }

        /// <summary>
        ///   Registers this component with the <see cref = "IKernel" />.
        /// </summary>
        /// <param name = "kernel">The kernel.</param>
        void IRegistration.Register(IKernelInternal kernel)
        {
            if (registered)
            {
                return;
            }
            registered = true;
            var services = FilterServices(kernel);
            if (services.Length == 0)
            {
                return;
            }

            var componentModel = kernel.ComponentModelBuilder.BuildModel(GetContributors(services));
            if (SkipRegistration(kernel, componentModel))
            {
                return;
            }
            kernel.AddCustomComponent(componentModel);
        }

        private bool SkipRegistration(IKernelInternal internalKernel, ComponentModel componentModel)
        {
            return ifComponentRegisteredIgnore && internalKernel.HasComponent(componentModel.Name);
        }

        private IComponentModelDescriptor[] GetContributors(Type[] services)
        {
            var list = new List<IComponentModelDescriptor>
			{
				new ServicesDescriptor(services),
				new DefaultsDescriptor(name, implementation),
			};
            list.AddRange(descriptors);
            list.Add(new InterfaceProxyDescriptor());
            return list.ToArray();
        }

        private Type[] FilterServices(IKernel kernel)
        {
            var services = new List<Type>(potentialServices);
            if (registerNewServicesOnly)
            {
#if SILVERLIGHT
				services.ToArray().Where(kernel.HasComponent).ForEach(t => services.Remove(t));
#else
                services.RemoveAll(kernel.HasComponent);
#endif
            }
            return services.ToArray();
        }



        /// <summary>
        ///   Sets the concrete type that implements the service to <typeparamref name = "TImpl" />.
        ///   <para />
        ///   If not set, the class service type or first registered interface will be used as the implementation for this component.
        /// </summary>
        /// <typeparam name = "TImpl">The type that is the implementation for the service.</typeparam>
        /// <returns></returns>
        public ComponentRegistration<TService> ImplementedBy<TImpl>() where TImpl : TService
        {
            return ImplementedBy(typeof(TImpl));
        }

        /// <summary>
        ///   Sets the concrete type that implements the service to <paramref name = "type" />.
        ///   <para />
        ///   If not set, the class service type or first registered interface will be used as the implementation for this component.
        /// </summary>
        /// <param name = "type">The type that is the implementation for the service.</param>
        /// <returns></returns>
        public ComponentRegistration<TService> ImplementedBy(Type type)
        {
            return ImplementedBy(type, null);
        }

        /// <summary>
        ///   Sets the concrete type that implements the service to <paramref name = "type" />.
        ///   <para />
        ///   If not set, the class service type or first registered interface will be used as the implementation for this component.
        /// </summary>
        /// <param name = "type">The type that is the implementation for the service.</param>
        /// <param name = "genericImplementationMatchingStrategy">Provides ability to close open generic service. Ignored when registering closed or non-generic component.</param>
        /// <returns></returns>
        public ComponentRegistration<TService> ImplementedBy(Type type,
                                                             IGenericImplementationMatchingStrategy
                                                                genericImplementationMatchingStrategy)
        {
            if (implementation != null /*&& implementation != typeof(LateBoundComponent)*/)
            {
                var message = String.Format("This component has already been assigned implementation {0}",
                                            implementation.FullName);
                throw new ComponentRegistrationException(message);
            }

            implementation = type;
            /*if (genericImplementationMatchingStrategy == null)
            {
                return this;
            }*/
            return
                ExtendedProperties(
                    Property.ForKey(ComponentModel.GenericImplementationMatchingStrategy).Eq(genericImplementationMatchingStrategy));
        }

        /// <summary>
        ///   Sets <see cref = "ComponentModel.ExtendedProperties" /> for this component.
        /// </summary>
        /// <param name = "properties">The extended properties.</param>
        /// <returns></returns>
        public ComponentRegistration<TService> ExtendedProperties(params Property[] properties)
        {
            return AddDescriptor(new ExtendedPropertiesDescriptor(properties));
        }

        /// <summary>
        ///   Adds the descriptor.
        /// </summary>
        /// <param name = "descriptor">The descriptor.</param>
        /// <returns></returns>
        public ComponentRegistration<TService> AddDescriptor(IComponentModelDescriptor descriptor)
        {
            descriptors.Add(descriptor);
            var componentDescriptor = descriptor as AbstractOverwriteableDescriptor<TService>;
            if (componentDescriptor != null)
            {
                componentDescriptor.Registration = this;
            }
            return this;
        }


    }

    public interface IGenericImplementationMatchingStrategy
    {
    }
    /// <summary>
    ///   Exception threw when there is a problem
    ///   registering a component
    /// </summary>
    [Serializable]
    public class ComponentRegistrationException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ComponentRegistrationException" /> class.
        /// </summary>
        /// <param name = "message">The message.</param>
        public ComponentRegistrationException(string message)
            : base(message)
        {
            //ExceptionHelper.SetUp(this);
        }

        public ComponentRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
            //ExceptionHelper.SetUp(this);
        }
    }
}
