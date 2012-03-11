using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    public interface ILifetimeManagement<T>
    {
        void Release(Action<T> action);
        //void DestroyOnEvent(Delegate handler);
        //void DestroyOnEvent(Action<ILifetimeManagement> lf);
    }

    public interface ILifetimeManagement
    {
        void DoRelease();
        //void DoRelease(object o, EventArgs e);
    }

    /// <summary>
    /// Summary
    /// </summary>
    public class LifetimeManager<T> : ILifetimeManagement<T>, ILifetimeManagement where T : class
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

        public void Release(Action<T> action)
        {
            this.action = action;
        }

        #endregion

        #region ILifetimeManagement Members

        public void DoRelease()
        {
            action((T)instanceLifetime.Instance);
            //this.action((ILifetimeManagement<T>)instanceLifetime);
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

    public interface IDestructible
    {
        ILifetimeEnds Candidate { get; set; }
    }

    public interface ILifetimeEnds
    {
        void Release();
    }

    public abstract class InstanceLifetime : ILifetimeEnds
    {

        protected string key;
        private object instance;

        protected internal object Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        protected SimpleIoc serviceLocator;

        public virtual void Release()
        {
            throw new NotImplementedException();
        }


        //internal abstract object CreateInstance(object initial);
        /// <summary>
        /// Gets or sets 
        /// </summary>
        protected internal virtual ILifetimeManagement LifetimeManager
        {
            get;
            set;
        }

        internal virtual void InstanceCreated(SimpleIoc serviceLocator, object instance)
        {
        }

        /*internal virtual object CreateInstance(object initial)
        {
            return initial;
        }*/
        //object CreateFromFactory(Dictionary<string, object> instances, string key);
        //object CreateWithConstructor(Dictionary<string, object> instances, string key);
        protected internal abstract object GetInstance(Dictionary<string, object> instances, string key);

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

    internal class DummyInstanceHelper : InstanceLifetime
    {
        #region IInstanceLifetime Members

        /*internal override object CreateInstance(object initial)
        {
            throw new NotImplementedException();
        }*/

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
    /// SingletonInstance.
    /// </summary>
    [Serializable]
    internal class SingletonInstance : InstanceLifetime
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        /// <value>The Instance.</value>
        //internal object Instance { get; private set; }
        #endregion
        #region Constructors

        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="SingletonInstance"/> class.
        /// </summary>
        /// <param name="Instance">The Instance</param>
        public SingletonInstance()
        {
            //Instance = instance;
        }
        #endregion

        #region IInstanceLifetime Members

        /*internal override object CreateInstance(object initial)
        {
            return initial;
        }*/

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
    /// TransitionLifetime.
    /// </summary>
    [Serializable]
    public class TransitionLifetime : InstanceLifetime
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        /// <value>The Instance.</value>
        //internal object Instance { get; private set; }
        #endregion
        #region Constructors

        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="SingletonInstance"/> class.
        /// </summary>
        /// <param name="Instance">The Instance</param>
        public TransitionLifetime()
        {
            //Instance = instance;
        }
        #endregion

        #region IInstanceLifetime Members

        /*internal override object CreateInstance(object initial)
        {
            return initial;
        }*/

        public override void Release()
        {
            serviceLocator.Unregister(Instance.GetType(), Instance, key);
            if (this.LifetimeManager != null)
            {
                this.LifetimeManager.DoRelease();
            }
            this.LifetimeManager = null;
            this.Instance = null;
            this.serviceLocator = null;
        }

        internal override void InstanceCreated(SimpleIoc serviceLocator, object instance)
        {
            this.Instance = instance;
            this.serviceLocator = serviceLocator;
            if (instance is IDestructible)
            {
                var destructible = (IDestructible)instance;
                // inject this as IDestructible instance to create a point for lifetime release.
                destructible.Candidate = this;
            }
        }


        protected internal override object GetInstance(Dictionary<string, object> instances, string key)
        {
            if (instances.ContainsKey(key))
            {
                instances.Remove(key);
            }
            return null;
        }

        protected internal override string GetKey(string key, string _uniqueKey)
        {
            this.key = key;
            //if (string.IsNullOrEmpty(key))
            {
                this.key = Guid.NewGuid().ToString();
            }
            return this.key;
        }

        /*public override InstanceLifetime Release(IDestructor destruction)
        {
            return this;
        }*/

        #endregion
    }

}
