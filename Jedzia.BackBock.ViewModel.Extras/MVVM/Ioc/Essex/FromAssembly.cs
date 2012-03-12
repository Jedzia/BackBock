using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Collections;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    public class DefaultConfigurationStore : /*AbstractSubSystem,*/ IConfigurationStore, ISubSystem
    {
        #region ISubSystem Members

        public void Init(IKernelInternal kernel)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class PartialConfigurationStore : IConfigurationStore, ISubSystem, IDisposable
    {
        // Fields
        private readonly IConfigurationStore inner;
        private readonly IConfigurationStore partial;

        public PartialConfigurationStore(IKernelInternal kernel)
        {
            //this.inner = kernel.ConfigurationStore;
            this.partial = new DefaultConfigurationStore();
            //this.partial.Init(kernel);
        }

        #region ISubSystem Members

        public void Init(IKernelInternal kernel)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            // Todo: Implement
            //throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Terminate();
        }

        #endregion
    }

    public interface IComponentModelDescriptor
    {
        // Methods
        void BuildComponentModel(IKernel kernel, ComponentModel model);
        void ConfigureComponentModel(IKernel kernel, ComponentModel model);
    }

    /// <summary>
    ///   Implementors must construct a populated
    ///   instance of ComponentModel by inspecting the component
    ///   and|or the configuration.
    /// </summary>
    public interface IComponentModelBuilder
    {
        //IContributeComponentModelConstruction[] Contributors { get; }

        /// <summary>
        ///   "To give or supply in common with others; give to a 
        ///   common fund or for a common purpose". The contributor
        ///   should inspect the component, or even the configuration
        ///   associated with the component, to add or change information
        ///   in the model that can be used later.
        /// </summary>
        //void AddContributor(IContributeComponentModelConstruction contributor);

        /// <summary>
        ///   Constructs a new ComponentModel by invoking
        ///   the registered contributors.
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "services"></param>
        /// <param name = "classType"></param>
        /// <param name = "extendedProperties"></param>
        /// <returns></returns>
        //ComponentModel BuildModel(ComponentName name, Type[] services, Type classType, IDictionary extendedProperties);

        ComponentModel BuildModel(IComponentModelDescriptor[] customContributors);

        /// <summary>
        ///   Removes the specified contributor
        /// </summary>
        /// <param name = "contributor"></param>
        //void RemoveContributor(IContributeComponentModelConstruction contributor);
    }

    public interface IHandler
    { 
    }
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

    public interface IKernelEvents
    {
    }
    public interface IReleasePolicy : IDisposable
    {
    }
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

    public interface ISubSystem
    {
        // Methods
        void Init(IKernelInternal kernel);
        void Terminate();
    }

    public interface IRegistration
    {
        // Methods
        void Register(IKernelInternal kernel);
    }

    public interface IWindsorContainer : IDisposable
    {
        IWindsorContainer Install(params IWindsorInstaller[] installers);
        IWindsorContainer Register(params IRegistration[] registrations);

        /// <summary>
        ///   Returns a component instance by the service
        /// </summary>
        /// <typeparam name = "T">Service type</typeparam>
        /// <returns>The component instance</returns>
        T Resolve<T>();
    }

    //[Serializable, DebuggerTypeProxy(typeof(KernelDebuggerProxy)), DebuggerDisplay("{name,nq}")]
    public class WindsorContainer : MarshalByRefObject, IWindsorContainer, IDisposable
    {
        // Fields
        private readonly Dictionary<string, IWindsorContainer> childContainers;
        private readonly object childContainersLocker;
        //private readonly IComponentsInstaller installer;
        private readonly IKernel kernel;
        private readonly string name;
        private IWindsorContainer parent;

        /// <summary>
        ///   Returns a component instance by the service
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
		{
			return (T)kernel.Resolve(typeof(T), (IDictionary)null);
		}

        /// <summary>
        ///   Constructs a container without any external 
        ///   configuration reference
        /// </summary>
        public WindsorContainer()
            : this(new DefaultKernel(), new DefaultComponentInstaller())
        {
        }

        /// <summary>
        ///   Constructs a container using the specified <see cref = "IKernel" />
        ///   implementation. Rarely used.
        /// </summary>
        /// <remarks>
        ///   This constructs sets the Kernel.ProxyFactory property to
        ///   <c>Proxy.DefaultProxyFactory</c>
        /// </remarks>
        /// <param name = "kernel">Kernel instance</param>
        /// <param name = "installer">Installer instance</param>
        public WindsorContainer(IKernel kernel, IComponentsInstaller installer)
            : this(Guid.NewGuid().ToString(), kernel, installer)
        {
        }

        /// <summary>
        ///   Constructs a container using the specified <see cref = "IKernel" />
        ///   implementation. Rarely used.
        /// </summary>
        /// <remarks>
        ///   This constructs sets the Kernel.ProxyFactory property to
        ///   <c>Proxy.DefaultProxyFactory</c>
        /// </remarks>
        /// <param name = "name">Container's name</param>
        /// <param name = "kernel">Kernel instance</param>
        /// <param name = "installer">Installer instance</param>
        public WindsorContainer(String name, IKernel kernel, IComponentsInstaller installer)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            if (installer == null)
            {
                throw new ArgumentNullException("installer");
            }

            this.name = name;
            this.kernel = kernel;
            // Todo: Implement?
            //this.kernel.ProxyFactory = new DefaultProxyFactory();
            //this.installer = installer;
        }

        private void RunInstaller()
        {
            // Todo: Implement
            //throw new NotImplementedException();
        }

        /*/// <summary>
        ///   Constructs a container using the specified 
        ///   <see cref = "IConfigurationInterpreter" /> implementation.
        /// </summary>
        /// <param name = "interpreter">The instance of an <see cref = "IConfigurationInterpreter" /> implementation.</param>
        public WindsorContainer(IConfigurationInterpreter interpreter)
            : this()
        {
            if (interpreter == null)
            {
                throw new ArgumentNullException("interpreter");
            }
            interpreter.ProcessResource(interpreter.Source, kernel.ConfigurationStore, kernel);

            RunInstaller();
        }

                /// <summary>
        ///   Constructs a container using the specified 
        ///   <see cref = "IConfigurationStore" /> implementation.
        /// </summary>
        /// <param name = "store">The instance of an <see cref = "IConfigurationStore" /> implementation.</param>
        public WindsorContainer(IConfigurationStore store)
            : this()
        {
            kernel.ConfigurationStore = store;

            RunInstaller();
        }

 
        
        /// <summary>
        ///   Initializes a new instance of the <see cref = "WindsorContainer" /> class.
        /// </summary>
        /// <param name = "interpreter">The interpreter.</param>
        /// <param name = "environmentInfo">The environment info.</param>
        public WindsorContainer(IConfigurationInterpreter interpreter, IEnvironmentInfo environmentInfo)
            : this()
        {
            if (interpreter == null)
            {
                throw new ArgumentNullException("interpreter");
            }
            if (environmentInfo == null)
            {
                throw new ArgumentNullException("environmentInfo");
            }

            interpreter.EnvironmentName = environmentInfo.GetEnvironmentName();
            interpreter.ProcessResource(interpreter.Source, kernel.ConfigurationStore, kernel);

            RunInstaller();
        }*/


        public IWindsorContainer Install(params IWindsorInstaller[] installers)
        {
            if (installers == null)
            {
                throw new ArgumentNullException("installers");
            }
            if (installers.Length != 0)
            {
                DefaultComponentInstaller scope = new DefaultComponentInstaller();
                IKernelInternal kernel = this.kernel as IKernelInternal;
                if (kernel == null)
                {
                    this.Install(installers, scope);
                }
                else
                {
                    IDisposable disposable = kernel.OptimizeDependencyResolution();
                    this.Install(installers, scope);
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            return this;
        }

        private void Install(IWindsorInstaller[] installers, DefaultComponentInstaller scope)
        {
            using (PartialConfigurationStore store = new PartialConfigurationStore((IKernelInternal)this.kernel))
            {
                foreach (IWindsorInstaller installer in installers)
                {
                    installer.Install(this, store);
                }
                scope.SetUp(this, store);
            }
        }

        public virtual void Dispose()
        {
            this.Parent = null;
            this.childContainers.Clear();
            //this.kernel.Dispose();
        }

        public virtual IWindsorContainer Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (value == null)
                {
                    if (this.parent != null)
                    {
                        //this.parent.RemoveChildContainer(this);
                        this.parent = null;
                    }
                }
                else if (value != this.parent)
                {
                    this.parent = value;
                    //this.parent.AddChildContainer(this);
                }
            }
        }

        public IWindsorContainer Register(params IRegistration[] registrations)
        {
            this.Kernel.Register(registrations);
            return this;
        }

        public virtual IKernel Kernel
        {
            get
            {
                return this.kernel;
            }
        }


    }

    public interface IConfigurationStore //: ISubSystem
    {
    }

    public class InstallerFactory
    {
        // Methods
        public virtual IWindsorInstaller CreateInstance(Type installerType)
        {
            return installerType.CreateInstance<IWindsorInstaller>(new object[0]);
        }

        public virtual IEnumerable<Type> Select(IEnumerable<Type> installerTypes)
        {
            return installerTypes;
        }
    }


    public interface IWindsorInstaller
    {
        // Methods
        void Install(IWindsorContainer container, IConfigurationStore store);
    }

    public class CompositeInstaller : IWindsorInstaller
    {

        // Fields
        private readonly HashSet<IWindsorInstaller> installers = new HashSet<IWindsorInstaller>();

        // Methods
        public void Add(IWindsorInstaller instance)
        {
            this.installers.Add(instance);
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (IWindsorInstaller installer in this.installers)
            {
                installer.Install(container, store);
            }
        }
    }

    public class FromAssembly
    {
        public static IWindsorInstaller InThisApplication()
        {
            return ApplicationAssemblies(Assembly.GetCallingAssembly(), new InstallerFactory());
        }
        
        public static IWindsorInstaller InThisEntry()
        {
            return ApplicationAssemblies(Assembly.GetEntryAssembly(), new InstallerFactory());
        }

        public static IWindsorInstaller InThisApplication(InstallerFactory installerFactory)
        {
            return ApplicationAssemblies(Assembly.GetCallingAssembly(), installerFactory);
        }

        //... 



        private static IWindsorInstaller ApplicationAssemblies(Assembly rootAssembly, InstallerFactory installerFactory)
        {
            HashSet<Assembly> set = new HashSet<Assembly>(ReflectionUtil.GetApplicationAssemblies(rootAssembly));
            CompositeInstaller installer = new CompositeInstaller();
            foreach (Assembly assembly in set)
            {
                if (assembly == typeof(FromAssembly).Assembly)
                {
                    continue;
                }
                installer.Add(Instance(assembly, installerFactory));
            }
            return installer;
        }

        public static IWindsorInstaller Instance(Assembly assembly, InstallerFactory installerFactory)
        {
            return new AssemblyInstaller(assembly, installerFactory);
        }
    }

    public class AssemblyInstaller : IWindsorInstaller
    {
        // Fields
        private readonly Assembly assembly;
        private readonly InstallerFactory factory;

        // Methods
        public AssemblyInstaller(Assembly assembly, InstallerFactory factory)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            this.assembly = assembly;
            this.factory = factory;
        }

        private IEnumerable<Type> FilterInstallerTypes(IEnumerable<Type> types)
        {
            return (from t in types
                    where ((t.IsClass && !t.IsAbstract) && !t.IsGenericTypeDefinition) && t.Is<IWindsorInstaller>()
                    select t);
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IEnumerable<Type> enumerable = this.factory.Select(this.FilterInstallerTypes(this.assembly.GetExportedTypes()));
            if (enumerable != null)
            {
                foreach (Type type in enumerable)
                {
                    var ci = this.factory.CreateInstance(type);
                    ci.Install(container, store);
                    //this.factory.CreateInstance(type).Install(container, store);
                }
            }
        }
    }



    public static class ReflectionUtil
    {
        public static IEnumerable<Assembly> GetApplicationAssemblies(Assembly rootAssembly)
        {
            int length = rootAssembly.FullName.IndexOfAny(new char[] { '.', ',' });
            if (length < 0)
            {
                throw new ArgumentException(string.Format("Could not determine application name for assembly \"{0}\". Please use a different method for obtaining assemblies.", rootAssembly.FullName));
            }
            string applicationName = rootAssembly.FullName.Substring(0, length);
            HashSet<Assembly> assemblies = new HashSet<Assembly>();
            AddApplicationAssemblies(rootAssembly, assemblies, applicationName);
            return assemblies;
        }

        public static Type[] GetAvailableTypes(this Assembly assembly)
        {
            bool includeNonExported = false;
            return GetAvailableTypes(assembly, includeNonExported);
        }

        public static Type[] GetAvailableTypes(this Assembly assembly, bool includeNonExported)
        {
            try
            {
                if (includeNonExported)
                {
                    return assembly.GetTypes();
                }
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                //return e.Types.FindAll(t => t != null);
                throw;
                // NOTE: perhaps we should not ignore the exceptions here, and log them?
            }
        }

        private static void AddApplicationAssemblies(Assembly assembly, HashSet<Assembly> assemblies, string applicationName)
        {
            if (assemblies.Add(assembly))
            {
                foreach (AssemblyName name in assembly.GetReferencedAssemblies())
                {
                    if (IsApplicationAssembly(applicationName, name.FullName))
                    {
                        AddApplicationAssemblies(LoadAssembly(name), assemblies, applicationName);
                    }
                }
            }
        }

        private static bool IsApplicationAssembly(string applicationName, string assemblyName)
        {
            return assemblyName.StartsWith(applicationName);
        }

        private static Assembly LoadAssembly(AssemblyName assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        public static TBase CreateInstance<TBase>(this Type subtypeofTBase, params object[] ctorArgs)
        {
            EnsureIsAssignable<TBase>(subtypeofTBase);
            return Instantiate<TBase>(subtypeofTBase, ctorArgs ?? new object[0]);
        }

        private static void EnsureIsAssignable<TBase>(Type subtypeofTBase)
        {
            if (!subtypeofTBase.Is<TBase>())
            {
                string str;
                if (typeof(TBase).IsInterface)
                {
                    str = string.Format("Type {0} does not implement the interface {1}.", subtypeofTBase.FullName, typeof(TBase).FullName);
                }
                else
                {
                    str = string.Format("Type {0} does not inherit from {1}.", subtypeofTBase.FullName, typeof(TBase).FullName);
                }
                throw new InvalidCastException(str);
            }
        }

        public static bool Is<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }




        private static TBase Instantiate<TBase>(Type subtypeofTBase, object[] ctorArgs)
        {
            Func<object, Type> selector = null;
            TBase local;
            ctorArgs = ctorArgs ?? new object[0];
            Type[] types = ctorArgs.ConvertAll<object, Type>(delegate(object a)
            {
                if (a != null)
                {
                    return a.GetType();
                }
                return typeof(object);
            });
            ConstructorInfo ctor = subtypeofTBase.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, types, null);
            if (ctor != null)
            {
                return (TBase)ctor.Instantiate(ctorArgs);
            }
            try
            {
                local = (TBase)Activator.CreateInstance(subtypeofTBase, ctorArgs);
            }
            catch (MissingMethodException exception)
            {
                string str;
                if (ctorArgs.Length == 0)
                {
                    str = string.Format("Type {0} does not have a public default constructor and could not be instantiated.", subtypeofTBase.FullName);
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine(string.Format("Type {0} does not have a public constructor matching arguments of the following types:", subtypeofTBase.FullName));
                    if (selector == null)
                    {
                        selector = o => o.GetType();
                    }
                    foreach (Type type in ctorArgs.Select<object, Type>(selector))
                    {
                        builder.AppendLine(type.FullName);
                    }
                    str = builder.ToString();
                }
                throw new ArgumentException(str, exception);
            }
            catch (Exception exception2)
            {
                throw new Exception(string.Format("Could not instantiate {0}.", subtypeofTBase.FullName), exception2);
            }
            return local;
        }

        public static TResult[] ConvertAll<T, TResult>(this T[] items, Converter<T, TResult> transformation)
        {
            return Array.ConvertAll<T, TResult>(items, transformation);
        }

        public static object Instantiate(this ConstructorInfo ctor, object[] ctorArgs)
        {
            Func<object[], object> func;
            if (!factories.TryGetValue(ctor, out func))
            {
                //using (@lock.ForWriting())
                {
                    if (!factories.TryGetValue(ctor, out func))
                    {
                        func = BuildFactory(ctor);
                        factories[ctor] = func;
                    }
                }
            }
            return func(ctorArgs);
        }

        private static Func<object[], object> BuildFactory(ConstructorInfo ctor)
        {
            var parameterInfos = ctor.GetParameters();
            var parameterExpressions = new Expression[parameterInfos.Length];
            var argument = Expression.Parameter(typeof(object[]), "parameters");
            for (var i = 0; i < parameterExpressions.Length; i++)
            {
                parameterExpressions[i] = Expression.Convert(
                    Expression.ArrayIndex(argument, Expression.Constant(i, typeof(int))),
                    parameterInfos[i].ParameterType.IsByRef ? parameterInfos[i].ParameterType.GetElementType() : parameterInfos[i].ParameterType);
            }
            return Expression.Lambda<Func<object[], object>>(
                Expression.New(ctor, parameterExpressions),
                new[] { argument }).Compile();
        }

        private static readonly Lock @lock;
        private static readonly IDictionary<ConstructorInfo, Func<object[], object>> factories;
        static ReflectionUtil()
        {
            factories = new Dictionary<ConstructorInfo, Func<object[], object>>();
            @lock = Lock.Create();
        }
    }

    public abstract class Lock
    {
        // Methods
        protected Lock()
        {
        }

        public static Lock Create()
        {
            return new SlimReadWriteLock();
        }

        public abstract ILockHolder ForReading();
        public abstract ILockHolder ForReading(bool waitForLock);
        public abstract IUpgradeableLockHolder ForReadingUpgradeable();
        public abstract IUpgradeableLockHolder ForReadingUpgradeable(bool waitForLock);
        public abstract ILockHolder ForWriting();
        public abstract ILockHolder ForWriting(bool waitForLock);
    }

    public interface ILockHolder : IDisposable
    {
        // Properties
        bool LockAcquired { get; }
    }

    public interface IUpgradeableLockHolder : ILockHolder, IDisposable
    {
        // Methods
        ILockHolder Upgrade();
        ILockHolder Upgrade(bool waitForLock);
    }

    public class SlimReadWriteLock : Lock
    {
        public override ILockHolder ForReading()
        {
            throw new NotImplementedException();
        }

        public override ILockHolder ForReading(bool waitForLock)
        {
            throw new NotImplementedException();
        }

        public override IUpgradeableLockHolder ForReadingUpgradeable()
        {
            throw new NotImplementedException();
        }

        public override IUpgradeableLockHolder ForReadingUpgradeable(bool waitForLock)
        {
            throw new NotImplementedException();
        }

        public override ILockHolder ForWriting()
        {
            // Todo: impl.
            throw new NotImplementedException();
        }

        public override ILockHolder ForWriting(bool waitForLock)
        {
            throw new NotImplementedException();
        }
    }

}
