using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    /// <summary>
    /// Provides the fluent configuration interface for the <see cref="LifetimeManager:T"/> 
    /// of <see cref="SimpleIoc"/> container objects.
    /// </summary>
    /// <typeparam name="T">The object to provide with configuration abilities.</typeparam>
    public interface ILifetimeConfig<T>
    {
        void OnDestroy(Action<T> action);
        //void DestroyOnEvent(Delegate handler);
        //void DestroyOnEvent(Action<ILifetimeManagement> lf);
    }

    /// <summary>
    /// Provides the implementation of a destroy method for lifetime management 
    /// of <see cref="SimpleIoc"/> container objects.
    /// </summary>
    public interface ILifetimeManagement
    {
        void DoDestroy();
        //void DoRelease(object o, EventArgs e);
    }

    /// <summary>
    /// Manges the destruction of <see cref="SimpleIoc"/> container objects.
    /// </summary>
    /// <typeparam name="T">The object to provide with destruction abilities.</typeparam>
    public class LifetimeManager<T> : ILifetimeConfig<T>, ILifetimeManagement where T : class
    {
        private InstanceLifetime instanceLifetime;
        Action<T> action;
        #region ILifetimeManagement<T> Members

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LifetimeManager"/> class.
        /// </summary>
        public LifetimeManager(InstanceLifetime instanceLifetime)
        {
            this.instanceLifetime = instanceLifetime;
        }

        public void OnDestroy(Action<T> action)
        {
            this.action = action;
        }

        #endregion

        #region ILifetimeManagement Members

        public void DoDestroy()
        {
            if (action != null)
            {
                action((T)instanceLifetime.Instance);
                //this.action((ILifetimeManagement<T>)instanceLifetime);
            }
        }

        #endregion

        #region ILifetimeManagement<T> Members


        public void DestroyOnEvent(object handler)
        {
            //EventHandler
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Provides lifetime destruction abilities to the <see cref="SimpleIoc"/> container for the implementor.
    /// </summary>
    public interface IDestructible
    {
        ILifetimeEnds Candidate { get; set; }
    }

    /// <summary>
    /// Provides a release method for object destruction.
    /// </summary>
    public interface ILifetimeEnds
    {
        void Release();
    }

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

    /// <summary>
    /// Helper class for the lifetime management of <see cref="SimpleIoc"/> empty lifetimes.
    /// </summary>
    internal class DummyInstanceHelper : InstanceLifetime
    {
        #region IInstanceLifetime Members

        /*internal override object CreateInstance(object initial)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Handles the instance creation. Not implemented for this kind of lifetime.
        /// </summary>
        /// <param name="instances">The list of already registered instances.</param>
        /// <param name="key">The key of the requested instance.</param>
        /// <returns>
        /// An already contained instance from the IoC-Container or null if a new instance
        /// should be created.
        /// </returns>
        protected internal override object GetInstance(Dictionary<string, object> instances, string key)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

    /*/// <summary>
    /// SingletonInstance.
    /// </summary>
    [Serializable]
    internal class SingletonInstance : IInstanceLifetime
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        /// <value>The Instance.</value>
        //internal object Instance { get; private set; }
        private static Dictionary<Type, object> instances;
        #endregion
        #region Constructors

        static SingletonInstance()
        {
            instances = new Dictionary<Type, object>();
        }
        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="SingletonInstance"/> class.
        /// </summary>
        /// <param name="Instance">The Instance</param>
        public SingletonInstance()
        {
        }
        #endregion

        #region IInstanceLifetime Members

        public object CreateInstance(object initial)
        {
            var instanceType = initial.GetType();
            if (!instances.ContainsKey(instanceType))
            {
                instances.Add(instanceType, initial);
            }
            return instances[instanceType];
        }

        #endregion

        #region IInstanceLifetime Members


        public object GetInstance(Dictionary<string, object> instances, string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }*/

    /// <summary>
    /// Implementation class for the lifetime management of <see cref="SimpleIoc"/> singleton container-instances.
    /// </summary>
    [Serializable]
    internal class SingletonInstance : InstanceLifetime
    {

        #region IInstanceLifetime Members

        /// <summary>
        /// Handles the instance creation. Objects, that are present in the instance dictionary
        /// are returned as is. That's what a Singleton is.
        /// </summary>
        /// <param name="instances">The list of already registered instances.</param>
        /// <param name="key">The key of the requested instance.</param>
        /// <returns>
        /// An already contained instance from the IoC-Container or null if a new instance
        /// should be created.
        /// </returns>
        protected internal override object GetInstance(Dictionary<string, object> instances, string key)
        {
            if (instances.ContainsKey(key))
            {
                return instances[key];
            }
            return null;
        }

        #endregion

    }


    /// <summary>
    /// Implementation class for the lifetime management of <see cref="SimpleIoc"/> container-instances
    /// with a transient lifetime behaviour.
    /// </summary>
    [Serializable]
    public class TransitionLifetime : InstanceLifetime
    {

        #region IInstanceLifetime Members

        /// <summary>
        /// Releases the container-instance by unregistering it and doing cleanup of 
        /// the lifetime management.
        /// </summary>
        public override void Release()
        {
            serviceLocator.Unregister(Instance.GetType(), Instance, key);
            base.Release();
        }

        internal override void InstanceCreated(SimpleIoc serviceLocator, object instance)
        {
            base.InstanceCreated(serviceLocator, instance);
            // Todo: also check for IDisposable
            if (instance is IDestructible)
            {
                var destructible = (IDestructible)instance;
                // inject this as IDestructible instance to create a point for lifetime release.
                destructible.Candidate = this;
            }
        }


        /// <summary>
        /// Handles the instance request from the container repository.
        /// </summary>
        /// <param name="instances">The list of already registered instances.</param>
        /// <param name="key">The key of the requested instance.</param>
        /// <returns>
        /// Always null to create a new container-instance on every request. This is what's called
        /// a transient behaviour.
        /// </returns>
        protected internal override object GetInstance(Dictionary<string, object> instances, string key)
        {
            // always renew instance creation on a request.
            if (instances.ContainsKey(key))
            {
                // Todo: do not remove this here. Do it later at Release()
                //instances.Remove(key);
                throw new NotSupportedException("Proper transient lifetimes cannot request an already registered " +
                    "key as a new one.");
                //Action removeAction = () => instances.Remove(key);
            }
            return null;
        }

        /// <summary>
        /// Determines the key used for instance creation.
        /// </summary>
        /// <param name="key">The requested key.</param>
        /// <param name="_uniqueKey">The unique key of the container.</param>
        /// <returns>
        /// Always a new key used for transient container-instance creation.
        /// </returns>
        protected internal override string GetKey(string key, string _uniqueKey)
        {
            //this.key = key;
            //if (string.IsNullOrEmpty(key))
            //{
                this.key = Guid.NewGuid().ToString();
            //}
            return this.key;
        }

        #endregion
    }

}
