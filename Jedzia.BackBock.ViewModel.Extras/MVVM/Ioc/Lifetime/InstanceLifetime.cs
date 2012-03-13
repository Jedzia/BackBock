namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    using System.Collections.Generic;

    /// <summary>
    /// Abstract base class for the lifetime management of <see cref="SimpleIoc"/> container-instances.
    /// </summary>
    public abstract class InstanceLifetime : ILifetimeEnds
    {
        
        /// <summary>
        /// The unique key associated with the container-instance.
        /// </summary>
        protected string key;
       
        /// <summary>
        /// Backing field for the container-instance;
        /// </summary>
        private object instance;

        /// <summary>
        /// Gets or sets the managed container-instance of this lifetime.
        /// </summary>
        /// <value>
        /// The in container-instance.
        /// </value>
        protected internal object Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        /// <summary>
        /// Holds a reference to the creating IoC-Container. 
        /// </summary>
        protected SimpleIoc serviceLocator;

        /// <summary>
        /// Releases the container-instance by checking the LifetimeManager
        /// for destroyable behaviour and cleans up this lifetime management.
        /// </summary>
        public virtual void Release()
        {
            if (this.LifetimeManager != null)
            {
                this.LifetimeManager.DoDestroy();
            }
            this.LifetimeManager = null;
            this.Instance = null;
            this.serviceLocator = null;
            //throw new NotImplementedException();
        }


        //internal abstract object CreateInstance(object initial);
        /// <summary>
        /// Gets or sets the lifetime manager.
        /// </summary>
        /// <value>
        /// The lifetime manager.
        /// </value>
        protected internal virtual ILifetimeManagement LifetimeManager
        {
            get;
            set;
        }

        /// <summary>
        /// Tracks instances post-creation.
        /// </summary>
        /// <param name="serviceLocator">The IoC container.</param>
        /// <param name="instance">The instance that was created.</param>
        internal virtual void InstanceCreated(SimpleIoc serviceLocator, object instance)
        {
            this.Instance = instance;
            this.serviceLocator = serviceLocator;
        }

        /*internal virtual object CreateInstance(object initial)
        {
            return initial;
        }*/
        //object CreateFromFactory(Dictionary<string, object> instances, string key);
        //object CreateWithConstructor(Dictionary<string, object> instances, string key);

        /// <summary>
        /// Handles the instance request from the container repository.
        /// </summary>
        /// <param name="instances">The list of already registered instances.</param>
        /// <param name="key">The key of the requested instance.</param>
        /// <returns>An already contained instance from the IoC-Container or null if a new instance 
        /// should be created.</returns>
        protected internal abstract object GetInstance(Dictionary<string, object> instances, string key);

        /// <summary>
        /// Determines the key used for instance creation.
        /// </summary>
        /// <param name="key">The requested key.</param>
        /// <param name="_uniqueKey">The unique key of the container.</param>
        /// <returns>The final key used for container-instance creation.</returns>
        protected internal virtual string GetKey(string key, string _uniqueKey)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = _uniqueKey;
            }
            return key;
        }

        //public abstract InstanceLifetime Release(IDestructor destruction);
        /*public virtual InstanceLifetime Release(IDestructible destruction)
        {
            throw new NotImplementedException("Can't Release this sort of type.");
        }*/
    }
}