// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleIoc.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// A very simple IOC container with basic functionality needed to register and resolve
    /// instances. If needed, this class can be replaced by another more elaborate
    /// IOC container implementing the <see cref="IServiceLocator"/> interface.
    /// The inspiration for this class is at https://gist.github.com/716137 but it has
    /// been extended with additional features.
    /// </summary>
    //// [ClassInfo(typeof(SimpleIoc),
    ////  VersionString = "4.0.0.0/BL0002",
    ////  DateString = "201109042117",
    ////  Description = "A very simple IOC container.",
    ////  UrlContacts = "http://www.galasoft.ch/contact_en.html",
    ////  Email = "laurent@galasoft.ch")]
    public class SimpleIoc : IServiceLocator
    {
        #region Fields

        /// <summary>
        /// Empty argument helper object.
        /// </summary>
        private readonly object[] emptyArguments = new object[0];

        /// <summary>
        /// Dictionary of factories methods.
        /// </summary>
        private readonly Dictionary<Type, Dictionary<string, Delegate>> factories
            = new Dictionary<Type, Dictionary<string, Delegate>>();

        /// <summary>
        /// Dictionary of instance info's.
        /// </summary>
        private readonly Dictionary<Type, InstanceInfo> instanceInfos = new Dictionary<Type, InstanceInfo>();

        /// <summary>
        /// Where the requested and instantiated instanced are kept.
        /// </summary>
        private readonly Dictionary<Type, Dictionary<string, object>> instancesRegistry
            = new Dictionary<Type, Dictionary<string, object>>();

        /// <summary>
        /// Holds the mapping from interfaces to implementation.
        /// </summary>
        private readonly Dictionary<Type, Type> interfaceToClassMap
            = new Dictionary<Type, Type>();

        /// <summary>
        /// Thread safe locking object.
        /// </summary>
        private readonly object @lock = new object();

        /// <summary>
        /// Unique key for this instance, unique for singletons.
        /// </summary>
        private readonly string uniqueKey = Guid.NewGuid().ToString();

        /// <summary>
        /// default container.
        /// </summary>
        private static SimpleIoc @default;

        #endregion

        #region Properties

        /// <summary>
        /// Gets this class' default instance.
        /// </summary>
        public static SimpleIoc Default
        {
            get
            {
                return @default ?? (@default = new SimpleIoc());
            }
        }

        #endregion

        /// <summary>
        /// Checks whether at least one instance of a given class is already created in the container.
        /// </summary>
        /// <typeparam name="TClass">The class that is queried.</typeparam>
        /// <returns>True if at least on instance of the class is already created, <c>false</c> otherwise.</returns>
        public bool Contains<TClass>()
        {
            return this.Contains<TClass>(null);
        }

        /// <summary>
        /// Checks whether the instance with the given key is already created for a given class
        /// in the container.
        /// </summary>
        /// <typeparam name="TClass">The class that is queried.</typeparam>
        /// <param name="key">The key that is queried.</param>
        /// <returns>True if the instance with the given key is already registered for the given class,
        /// <c>false</c> otherwise.</returns>
        public bool Contains<TClass>(string key)
        {
            var classType = typeof(TClass);

            if (!this.instancesRegistry.ContainsKey(classType))
            {
                return false;
            }

            if (string.IsNullOrEmpty(key))
            {
                return true;
            }

            return this.instancesRegistry[classType].ContainsKey(key);
        }

        /// <summary>
        /// Provides a way to get all the instances of a given type available in the
        /// cache.
        /// </summary>
        /// <param name="serviceType">The class of which all instances
        /// must be returned.</param>
        /// <returns>All the instances of the given type.</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            if (this.instancesRegistry.ContainsKey(serviceType))
            {
                return this.instancesRegistry[serviceType].Values;
            }

            return new List<object>();
        }

        /// <summary>
        /// Provides a way to get all the instances of a given type available in the
        /// cache.
        /// </summary>
        /// <typeparam name="TService">The class of which all instances
        /// must be returned.</typeparam>
        /// <returns>All the instances of the given type.</returns>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var serviceType = typeof(TService);

            if (this.instancesRegistry.ContainsKey(serviceType))
            {
                return this.instancesRegistry[serviceType].Values.Select(instance => (TService)instance);
            }

            return new List<TService>();
        }

        /// <summary>
        /// Provides a way to get an instance of a given type. If no instance had been instantiated 
        /// before, a new instance will be created. If an instance had already
        /// been created, that same instance will be returned.
        /// <remarks>If the class has not been registered before, this method
        /// returns <c>null</c>!</remarks>
        /// </summary>
        /// <param name="serviceType">The class of which an instance
        /// must be returned.</param>
        /// <returns>An instance of the given type.</returns>
        public object GetInstance(Type serviceType)
        {
            return this.DoGetService(serviceType, this.uniqueKey);
        }

        /// <summary>
        /// Provides a way to get an instance of a given type corresponding
        /// to a given key. If no instance had been instantiated with this
        /// key before, a new instance will be created. If an instance had already
        /// been created with the same key, that same instance will be returned.
        /// <remarks>If the class has not been registered before, this method
        /// returns <c>null</c>!</remarks>
        /// </summary>
        /// <param name="serviceType">The class of which an instance must be returned.</param>
        /// <param name="key">The key uniquely identifying this instance.</param>
        /// <returns>An instance corresponding to the given type and key.</returns>
        public object GetInstance(Type serviceType, string key)
        {
            return this.DoGetService(serviceType, key);
        }

        /// <summary>
        /// Provides a way to get an instance of a given type. If no instance had been instantiated 
        /// before, a new instance will be created. If an instance had already
        /// been created, that same instance will be returned.
        /// <remarks>If the class has not been registered before, this method
        /// returns <c>null</c>!</remarks>
        /// </summary>
        /// <typeparam name="TService">The class of which an instance
        /// must be returned.</typeparam>
        /// <returns>An instance of the given type.</returns>
        public TService GetInstance<TService>()
        {
            return (TService)this.DoGetService(typeof(TService), this.uniqueKey);
        }

        /// <summary>
        /// Provides a way to get an instance of a given type corresponding
        /// to a given key. If no instance had been instantiated with this
        /// key before, a new instance will be created. If an instance had already
        /// been created with the same key, that same instance will be returned.
        /// <remarks>If the class has not been registered before, this method
        /// returns <c>null</c>!</remarks>
        /// </summary>
        /// <typeparam name="TService">The class of which an instance must be returned.</typeparam>
        /// <param name="key">The key uniquely identifying this instance.</param>
        /// <returns>An instance corresponding to the given type and key.</returns>
        public TService GetInstance<TService>(string key)
        {
            return (TService)this.DoGetService(typeof(TService), key);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or- 
        /// <c>null</c> if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        public object GetService(Type serviceType)
        {
            return this.DoGetService(serviceType, this.uniqueKey);
        }

        /// <summary>
        /// Determines whether the generic type is registered.
        /// </summary>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <returns>
        ///   <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered<T>()
        {
            var classType = typeof(T);
            return this.interfaceToClassMap.ContainsKey(classType);
        }

        /// <summary>
        /// Determines whether the generic type is registered with the specified key.
        /// </summary>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <param name="key">The key for which the given instance is registered.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered<T>(string key)
        {
            var classType = typeof(T);

            if (!this.interfaceToClassMap.ContainsKey(classType)
                || !this.factories.ContainsKey(classType))
            {
                return false;
            }

            return this.factories[classType].ContainsKey(key);
        }


        /// <summary>
        /// Registers a given type for a given interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface for which instances will be resolved.</typeparam>
        /// <typeparam name="TClass">The type that must be used to create instances.</typeparam>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1004",
            Justification = "This syntax is better than the alternatives.")]
        public ILifetimeConfig<TClass> Register<TInterface, TClass>() where TClass : class, TInterface
        {
            return Register<TInterface, TClass>(new SingletonInstance());
        }

        /// <summary>
        /// Registers a given type for a given interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface for which instances will be resolved.</typeparam>
        /// <typeparam name="TClass">The type that must be used to create instances.</typeparam>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1004",
            Justification = "This syntax is better than the alternatives.")]
        public ILifetimeConfig<TClass> Register<TInterface, TClass>(InstanceLifetime lifetime) where TClass : class, TInterface
        {
            // Todo: Implement
            //throw new NotImplementedException();

            lock (this.@lock)
            {
                var interfaceType = typeof(TInterface);
                var classType = typeof(TClass);

                this.Register(interfaceType, classType, lifetime);
            }

            var ilm = new LifetimeManager<TClass>(lifetime);
            lifetime.LifetimeManager = ilm;
            return ilm;
        }

        /// <summary>
        /// Registers a given type.
        /// </summary>
        /// <typeparam name="TClass">The type that must be used to create instances.</typeparam>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1004", 
            Justification = "This syntax is better than the alternatives.")]
        public void Register<TClass>() where TClass : class
        {
            this.RegisterWithLifetime<TClass>(new SingletonInstance());
        }

        /// <summary>
        /// Registers a given type.
        /// </summary>
        /// <typeparam name="TClass">The type that must be used to create instances.</typeparam>
        /// <param name="lifetime">The lifetime of the registered instance.</param>
        /// <returns>A lifetime configuration instance.</returns>
        /// <exception cref="ArgumentException">An interface cannot be registered alone</exception>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1004", 
            Justification = "This syntax is better than the alternatives.")]
        public ILifetimeConfig<TClass> RegisterWithLifetime<TClass>(InstanceLifetime lifetime) where TClass : class
        {
            lock (this.@lock)
            {
                var classType = typeof(TClass);

                // IInstanceLifetime resultLifetime;
#if WIN8
                if (classType.GetTypeInfo().IsInterface)
#else
                if (classType.IsInterface)
#endif
                {
                    throw new ArgumentException("An interface cannot be registered alone");
                }

                if (this.interfaceToClassMap.ContainsKey(classType))
                {
                    this.interfaceToClassMap[classType] = null;
                    lifetime = new DummyInstanceHelper();
                }
                else
                {
                    this.interfaceToClassMap.Add(classType, null);
                    var ii = new InstanceInfo(classType, classType, lifetime);
                    this.instanceInfos.Add(classType, ii);
                }

                if (this.factories.ContainsKey(classType))
                {
                    this.factories.Remove(classType);
                }

                var ilm = new LifetimeManager<TClass>(lifetime);
                lifetime.LifetimeManager = ilm;
                return ilm;
            }
        }

        /// <summary>
        /// Registers a given instance for a given type.
        /// </summary>
        /// <typeparam name="TClass">The type that is being registered.</typeparam>
        /// <param name="factory">The factory method able to create the instance that
        /// must be returned when the given type is resolved.</param>
        public void Register<TClass>(Func<TClass> factory) where TClass : class
        {
            Register<TClass>(factory, this.uniqueKey);
        }

        /// <summary>
        /// Registers a given instance for a given type.
        /// </summary>
        /// <typeparam name="TClass">The type that is being registered.</typeparam>
        /// <param name="factory">The factory method able to create the instance that
        /// must be returned when the given type is resolved.</param>
        /// <param name="key">The key for which the given instance is registered.</param>
        public void Register<TClass>(Func<TClass> factory, string key) where TClass : class
        {
            RegisterWithLifetime<TClass>(new SingletonInstance(), factory, key);
        }

        /// <summary>
        /// Registers a given instance for a given type.
        /// </summary>
        /// <typeparam name="TClass">The type that is being registered.</typeparam>
        /// <param name="factory">The factory method able to create the instance that
        /// must be returned when the given type is resolved.</param>
        public ILifetimeConfig<TClass> RegisterWithLifetime<TClass>(InstanceLifetime lifetime, Func<TClass> factory) where TClass : class
        {
            return RegisterWithLifetime(lifetime, factory, this.uniqueKey);
        }

        /// <summary>
        /// Registers a given instance for a given type and a given key.
        /// </summary>
        /// <typeparam name="TClass">The type that is being registered.</typeparam>
        /// <param name="factory">The factory method able to create the instance that
        /// must be returned when the given type is resolved.</param>
        /// <param name="key">The key for which the given instance is registered.</param>
        public ILifetimeConfig<TClass> RegisterWithLifetime<TClass>(InstanceLifetime lifetime, Func<TClass> factory, string key) where TClass : class
        {
            lock (this.@lock)
            {
                var classType = typeof(TClass);

                if (this.interfaceToClassMap.ContainsKey(classType))
                {
                    this.interfaceToClassMap[classType] = null;
                }
                else
                {
                    this.interfaceToClassMap.Add(classType, null);
                    var ii = new InstanceInfo(classType, classType, lifetime);
                    this.instanceInfos.Add(classType, ii);
                }

                if (this.instancesRegistry.ContainsKey(classType)
                    && this.instancesRegistry[classType].ContainsKey(key))
                {
                    this.instancesRegistry[classType].Remove(key);
                }

                if (this.factories.ContainsKey(classType))
                {
                    if (this.factories[classType].ContainsKey(key))
                    {
                        this.factories[classType][key] = factory;
                    }
                    else
                    {
                        this.factories[classType].Add(key, factory);
                    }
                }
                else
                {
                    var list = new Dictionary<string, Delegate>
                                   {
                                       {
                                           key, 
                                           factory
                                           }
                                   };

                    this.factories.Add(classType, list);
                }
            }

            var ilm = new LifetimeManager<TClass>(lifetime);
            lifetime.LifetimeManager = ilm;
            return ilm;
        }

        /// <summary>
        /// Resets the instance in its original states. This deletes all the
        /// registrations.
        /// </summary>
        public void Reset()
        {
            this.interfaceToClassMap.Clear();
            this.instanceInfos.Clear();
            this.instancesRegistry.Clear();
            this.factories.Clear();
        }

        /// <summary>
        /// Unregisters a class from the cache and removes all the previously
        /// created instances.
        /// </summary>
        /// <typeparam name="TClass">The class that must be removed.</typeparam>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1004", 
            Justification = "This syntax is better than the alternatives.")]
        public void Unregister<TClass>() where TClass : class
        {
            lock (this.@lock)
            {
                var classType = typeof(TClass);

                if (this.instancesRegistry.ContainsKey(classType))
                {
                    this.instancesRegistry.Remove(classType);
                }

                if (this.interfaceToClassMap.ContainsKey(classType))
                {
                    this.interfaceToClassMap.Remove(classType);
                    this.instanceInfos.Remove(classType);
                }

                if (this.factories.ContainsKey(classType))
                {
                    this.factories.Remove(classType);
                }
            }
        }

        /// <summary>
        /// Removes the given instance from the cache. The class itself remains
        /// registered and can be used to create other instances.
        /// </summary>
        /// <typeparam name="TClass">The type of the instance to be removed.</typeparam>
        /// <param name="instance">The instance that must be removed.</param>
        public void Unregister<TClass>(TClass instance) where TClass : class
        {
            lock (this.@lock)
            {
                var classType = typeof(TClass);

                if (this.instancesRegistry.ContainsKey(classType))
                {
                    var list = this.instancesRegistry[classType];

                    var pairs = list.Where(pair => pair.Value == instance).ToList();
                    for (var index = 0; index < pairs.Count(); index++)
                    {
                        var key = pairs[index].Key;

                        list.Remove(key);

                        if (this.factories.ContainsKey(classType))
                        {
                            if (this.factories[classType].ContainsKey(key))
                            {
                                this.factories[classType].Remove(key);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the instance corresponding to the given key from the cache. The class itself remains
        /// registered and can be used to create other instances.
        /// </summary>
        /// <typeparam name="TClass">The type of the instance to be removed.</typeparam>
        /// <param name="key">The key corresponding to the instance that must be removed.</param>
        [SuppressMessage(
            "Microsoft.Design", 
            "CA1004", 
            Justification = "This syntax is better than the alternatives.")]
        public void Unregister<TClass>(string key) where TClass : class
        {
            lock (this.@lock)
            {
                var classType = typeof(TClass);

                if (this.instancesRegistry.ContainsKey(classType))
                {
                    var list = this.instancesRegistry[classType];

                    var pairs = list.Where(pair => pair.Key == key).ToList();
                    for (var index = 0; index < pairs.Count(); index++)
                    {
                        list.Remove(pairs[index].Key);
                    }
                }

                if (this.factories.ContainsKey(classType))
                {
                    if (this.factories[classType].ContainsKey(key))
                    {
                        this.factories[classType].Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Registers the specified interface type with an implementation.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="classType">Type of the implementation class.</param>
        internal void Register(Type interfaceType, Type classType, InstanceLifetime lifetime)
        {
            if (this.interfaceToClassMap.ContainsKey(interfaceType))
            {
                if (this.interfaceToClassMap[interfaceType] != classType)
                {
                    if (this.instancesRegistry.ContainsKey(interfaceType))
                    {
                        this.instancesRegistry.Remove(interfaceType);
                    }
                }

                this.interfaceToClassMap[interfaceType] = classType;
                var ii = new InstanceInfo(classType, interfaceType, lifetime);
                this.instanceInfos[classType] = ii;
            }
            else
            {
                this.interfaceToClassMap.Add(interfaceType, classType);
                var ii = new InstanceInfo(classType, interfaceType, lifetime);
                this.instanceInfos.Add(classType, ii);
            }

            if (this.factories.ContainsKey(interfaceType))
            {
                this.factories.Remove(interfaceType);
            }
        }

        /// <summary>
        /// Removes the given instance from the cache. The class itself remains
        /// registered and can be used to create other instances.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="instance">The instance that must be removed.</param>
        /// <param name="key">The key corresponding to the instance that must be removed.</param>
        internal void Unregister(Type classType, object instance, string key)
        {
            lock (this.@lock)
            {
                if (this.instancesRegistry.ContainsKey(classType))
                {
                    var list = this.instancesRegistry[classType];

                    list.Remove(key);
                }
            }
        }

        /// <summary>
        /// Finally resolve the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key corresponding to the instance that must be removed.</param>
        /// <returns>The requested instance with the given key.</returns>
        /// <exception cref="ActivationException"><c>ActivationException</c>.</exception>
        private object DoGetService(Type serviceType, string key)
        {
            lock (this.@lock)
            {
                InstanceInfo iinfo;

                Dictionary<string, object> instances;

                if (!this.instancesRegistry.ContainsKey(serviceType))
                {
                    if (!this.interfaceToClassMap.ContainsKey(serviceType))
                    {
                        throw new ActivationException("Type not found in cache: " + serviceType.FullName);
                    }

                    instances = new Dictionary<string, object>();
                    this.instancesRegistry.Add(serviceType, instances);
                }
                else
                {
                    instances = this.instancesRegistry[serviceType];
                }

                if (this.interfaceToClassMap.ContainsKey(serviceType))
                {
                    var resolveTo = this.interfaceToClassMap[serviceType] ?? serviceType;
                    iinfo = this.instanceInfos[resolveTo];
                }
                else
                {
                    iinfo = this.instanceInfos[serviceType];
                }

                key = iinfo.Lifetime.GetKey(key, this.uniqueKey);

                /*if (string.IsNullOrEmpty(key))
                                {
                                    key = _uniqueKey;
                                }*/
                var dictInstance = iinfo.Lifetime.GetInstance(instances, key);
                if (dictInstance != null)
                {
                    return dictInstance;
                }

                /*if (instances.ContainsKey(key))
                {
                    return instances[key];
                }*/
                object instance = null;

                if (this.factories.ContainsKey(serviceType))
                {
                    if (this.factories[serviceType].ContainsKey(key))
                    {
                        instance = this.factories[serviceType][key].DynamicInvoke();
                    }
                    else
                    {
                        if (this.factories[serviceType].ContainsKey(this.uniqueKey))
                        {
                            instance = this.factories[serviceType][this.uniqueKey].DynamicInvoke();
                        }
                    }
                }

                if (instance == null)
                {
                    var constructor = this.GetConstructorInfo(serviceType);
                    var parameterInfos = constructor.GetParameters();

                    if (parameterInfos.Length == 0)
                    {
                        instance = constructor.Invoke(this.emptyArguments);
                    }
                    else
                    {
                        var parameters = new object[parameterInfos.Length];
                        foreach(var parameterInfo in parameterInfos)
                        {
                            parameters[parameterInfo.Position] = this.GetService(parameterInfo.ParameterType);
                        }

                        instance = constructor.Invoke(parameters);
                    }
                }

                // var lifetime = iinfo.Lifetime.CreateInstance;
                // var lifetime = new NormalInstance();
                // var lifetime = new SingletonInstance();
                // iinfo.Lifetime.GetInstance();
                instances.Add(key, instance);

                // instances.Add(key, new InstanceInfo(lifetime.CreateInstance(instance), lifetime));
                iinfo.Lifetime.InstanceCreated(this, instance);
                return instance;
            }
        }

        /// <summary>
        /// Gets the constructor info for a requested service.
        /// </summary>
        /// <param name="serviceType">Type of the requested service.</param>
        /// <returns>A <see cref="ConstructorInfo"/> able to create a new type of the requested one.</returns>
        /// <exception cref="ActivationException">Cannot build instance: Multiple constructors found but none marked with PreferredConstructor</exception>
        private ConstructorInfo GetConstructorInfo(Type serviceType)
        {
            var resolveTo = this.interfaceToClassMap[serviceType] ?? serviceType;

#if WIN8
            var constructorInfos = resolveTo.GetTypeInfo().DeclaredConstructors.ToArray();
#else
            var constructorInfos = resolveTo.GetConstructors();
#endif

            if (constructorInfos.Length > 1)
            {
                var preferredConstructorInfo = (from t in constructorInfos
                                                let attribute =
                                                    Attribute.GetCustomAttribute(
                                                        t, typeof(PreferredConstructorAttribute))
                                                where attribute != null
                                                select t).FirstOrDefault();

                if (preferredConstructorInfo == null)
                {
                    throw new ActivationException(
                        "Cannot build instance: Multiple constructors found but none marked with PreferredConstructor");
                }

                return preferredConstructorInfo;
            }

            return constructorInfos[0];
        }

    }
}