namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

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
}