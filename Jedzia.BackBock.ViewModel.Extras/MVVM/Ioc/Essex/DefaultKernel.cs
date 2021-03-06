﻿namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class DefaultKernel : IKernel, IKernelEvents, IKernelInternal
    {
        private readonly object handlersChangedLock = new object();
        private bool handlersChanged;
        private volatile bool handlersChangedDeferred;
        public IComponentModelBuilder ComponentModelBuilder { get; set; }

        /// <summary>
        ///   Constructs a DefaultKernel with no component
        ///   proxy support.
        /// </summary>
        public DefaultKernel()
            : this(new NotSupportedProxyFactory())
        {
        }

        private IContainerInjector containerInjector;

        /// <summary>
        ///   Constructs a DefaultKernel with the specified
        ///   implementation of <see cref = "IProxyFactory" /> and <see cref = "IDependencyResolver" />
        /// </summary>
        /// <param name = "resolver"></param>
        /// <param name = "proxyFactory"></param>
        public DefaultKernel(IDependencyResolver resolver, IProxyFactory proxyFactory)
        {
            RegisterSubSystems();
            //ReleasePolicy = new LifecycledComponentsReleasePolicy(this);
            //HandlerFactory = new DefaultHandlerFactory(this);
            ComponentModelBuilder = new DefaultComponentModelBuilder(this);
            //ProxyFactory = proxyFactory;
            //Resolver = resolver;
            //Resolver.Initialize(this, RaiseDependencyResolving);
            containerInjector = new SimpleIocInjector();
        }

        /// <summary>
        ///   Constructs a DefaultKernel with the specified
        ///   implementation of <see cref = "IProxyFactory" />
        /// </summary>
        public DefaultKernel(IProxyFactory proxyFactory)
            : this(new DefaultDependencyResolver(), proxyFactory)
        {
        }


        #region IKernel Members

        public virtual bool HasComponent(String name)
        {
            if (name == null)
            {
                return false;
            }

            /*if (NamingSubSystem.Contains(name))
            {
                return true;
            }

            if (Parent != null)
            {
                return Parent.HasComponent(name);
            }*/

            return false;
        }

        public virtual bool HasComponent(Type serviceType)
        {
            if (serviceType == null)
            {
                return false;
            }

            /*if (NamingSubSystem.Contains(serviceType))
            {
                return true;
            }

            if (serviceType.IsGenericType && NamingSubSystem.Contains(serviceType.GetGenericTypeDefinition()))
            {
                return true;
            }

            if (Parent != null)
            {
                return Parent.HasComponent(serviceType);
            }*/

            return false;
        }

        public IKernel Register(params IRegistration[] registrations)
        {
            if (registrations == null)
            {
                throw new ArgumentNullException("registrations");
            }

            var token = OptimizeDependencyResolution();
            foreach (var registration in registrations)
            {
                registration.Register(this);
            }
            if (token != null)
            {
                token.Dispose();
            }
            return this;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IKernelInternal Members

        public IDisposable OptimizeDependencyResolution()
        {
            if (handlersChangedDeferred)
            {
                return null;
            }

            handlersChangedDeferred = true;

            return new OptimizeDependencyResolutionDisposable(this);
        }

        #endregion

        private void DoActualRaisingOfHandlersChanged()
        {
            var stateChanged = true;
            while (stateChanged)
            {
                stateChanged = false;
                HandlersChanged(ref stateChanged);
            }
        }


        public event HandlersChangedDelegate HandlersChanged = delegate { };
        public event EventHandler RegistrationCompleted = delegate { };

        protected virtual void RaiseRegistrationCompleted()
        {
            RegistrationCompleted(this, EventArgs.Empty);
        }

        private class OptimizeDependencyResolutionDisposable : IDisposable
        {
            private readonly DefaultKernel kernel;

            public OptimizeDependencyResolutionDisposable(DefaultKernel kernel)
            {
                this.kernel = kernel;
            }

            public void Dispose()
            {
                lock (kernel.handlersChangedLock)
                {
                    try
                    {
                        if (kernel.handlersChanged == false)
                        {
                            return;
                        }

                        kernel.DoActualRaisingOfHandlersChanged();
                        kernel.RaiseRegistrationCompleted();
                        kernel.handlersChanged = false;
                    }
                    finally
                    {
                        kernel.handlersChangedDeferred = false;
                    }
                }
            }
        }

        // NOTE: this is from IKernelInternal
        public IHandler AddCustomComponent(ComponentModel model)
        {
            return AddCustomComponent(model, false);
        }

        public virtual IHandler AddCustomComponent(ComponentModel model, bool isMetaHandler)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            containerInjector.AddCustomComponent(model, isMetaHandler);

            RaiseComponentModelCreated(model);
            //return HandlerFactory.Create(model, isMetaHandler);
            return null;
        }

        protected virtual void RaiseComponentModelCreated(ComponentModel model)
        {
            ComponentModelCreated(model);
        }
        public event ComponentModelDelegate ComponentModelCreated = delegate { };
        public delegate void ComponentModelDelegate(ComponentModel model);

        /// <summary>
        ///   Returns a component instance by the key
        /// </summary>
        /// <param name = "key"></param>
        /// <param name = "service"></param>
        /// <returns></returns>
        public virtual object Resolve(String key, Type service)
        {
            return (this as IKernelInternal).Resolve(key, service, null, ReleasePolicy);
        }
        public IReleasePolicy ReleasePolicy { get; set; }

        /// <summary>
        ///   Returns the component instance by the component key
        /// </summary>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T), null);
        }

        /// <summary>
        ///   Returns the component instance by the service type
        ///   using dynamic arguments
        /// </summary>
        /// <param name = "service"></param>
        /// <param name = "arguments"></param>
        /// <returns></returns>
        public object Resolve(Type service, IDictionary arguments)
        {
            return (this as IKernelInternal).Resolve(service, arguments, ReleasePolicy);
        }

        /// <summary>
        ///   Returns a component instance by the key
        /// </summary>
        /// <param name = "key"></param>
        /// <param name = "service"></param>
        /// <param name = "arguments"></param>
        /// <param name = "policy"></param>
        /// <returns></returns>
        object IKernelInternal.Resolve(String key, Type service, IDictionary arguments, IReleasePolicy policy)
        {
            throw new NotImplementedException();
            /*var handler = (this as IKernelInternal).LoadHandlerByName(key, service, arguments);
            if (handler == null)
            {
                var otherHandlers = GetHandlers(service).Length;
                //throw new ComponentNotFoundException(key, service, otherHandlers);
                throw new ArgumentOutOfRangeException(service.ToString());
            }
            return ResolveComponent(handler, service ?? typeof(object), arguments, policy);*/
        }

        object IKernelInternal.Resolve(Type service, IDictionary arguments, IReleasePolicy policy)
        {
            throw new NotImplementedException();
            /*var handler = (this as IKernelInternal).LoadHandlerByType(null, service, arguments);
            if (handler == null)
            {
                //throw new ComponentNotFoundException(key, service, otherHandlers);
                throw new ArgumentOutOfRangeException(service.ToString());
            }
            return ResolveComponent(handler, service, arguments, policy);*/
        }

        IHandler IKernelInternal.LoadHandlerByType(string name, Type service, IDictionary arguments)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            var handler = GetHandler(service);
            if (handler != null)
            {
                return handler;
            }
            return null;
            /*lock (lazyLoadingLock)
            {
                handler = GetHandler(service);
                if (handler != null)
                {
                    return handler;
                }

                if (isCheckingLazyLoaders)
                {
                    return null;
                }

                isCheckingLazyLoaders = true;
                try
                {
                    foreach (var loader in ResolveAll<ILazyComponentLoader>())
                    {
                        var registration = loader.Load(name, service, arguments);
                        if (registration != null)
                        {
                            registration.Register(this);
                            return GetHandler(service);
                        }
                    }
                    return null;
                }
                finally
                {
                    isCheckingLazyLoaders = false;
                }
            }*/
        }

        public virtual IHandler GetHandler(Type service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            IHandler handler = null;
            //var handler = NamingSubSystem.GetHandler(service);
            /*if (handler == null && service.IsGenericType)
            {
                handler = NamingSubSystem.GetHandler(service.GetGenericTypeDefinition());
            }

            if (handler == null && Parent != null)
            {
                handler = WrapParentHandler(Parent.GetHandler(service));
            }*/

            return handler;
        }

        protected object ResolveComponent(IHandler handler, Type service, IDictionary additionalArguments, IReleasePolicy policy)
        {
            throw new NotImplementedException();
            /*Debug.Assert(handler != null, "handler != null");
            var parent = currentCreationContext;
            var context = CreateCreationContext(handler, service, additionalArguments, parent, policy);
            currentCreationContext = context;

            try
            {
                return handler.Resolve(context);
            }
            finally
            {
                currentCreationContext = parent;
            }*/
        }

        public virtual IConfigurationStore ConfigurationStore
        {
            get { return GetSubSystem(SubSystemConstants.ConfigurationStoreKey) as IConfigurationStore; }
            set { AddSubSystem(SubSystemConstants.ConfigurationStoreKey, value); }
        }
        
        public virtual ISubSystem GetSubSystem(string name)
        {
            ISubSystem system;
            this.subsystems.TryGetValue(name, out system);
            return system;
        }
        protected virtual void RegisterSubSystems()
        {
            AddSubSystem(SubSystemConstants.ConfigurationStoreKey,
                         new DefaultConfigurationStore());

            AddSubSystem(SubSystemConstants.ConversionManagerKey,
                         new DefaultConversionManager());

            //AddSubSystem(SubSystemConstants.NamingKey,
            //             new DefaultNamingSubSystem());

            //AddSubSystem(SubSystemConstants.ResourceKey,
            //             new DefaultResourceSubSystem());

            //AddSubSystem(SubSystemConstants.DiagnosticsKey,
            //             new DefaultDiagnosticsSubSystem());
        }

        public virtual void AddSubSystem(String name, ISubSystem subsystem)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (subsystem == null)
            {
                throw new ArgumentNullException("subsystem");
            }

            subsystem.Init(this);
            subsystems[name] = subsystem;
        }

        /// <summary>
        ///   Map of subsystems registered.
        /// </summary>
        private readonly Dictionary<string, ISubSystem> subsystems = new Dictionary<string, ISubSystem>(StringComparer.OrdinalIgnoreCase);

    }
}
