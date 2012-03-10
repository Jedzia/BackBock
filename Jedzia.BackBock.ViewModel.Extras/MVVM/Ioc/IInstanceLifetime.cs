using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    public interface IDestructible
    {
        ILifetimeEnds Candidate { get; set; }
    }

    public interface ILifetimeEnds
    {
        void Destroy();
    }

    public abstract class InstanceLifetime : ILifetimeEnds
    {
        //internal abstract object CreateInstance(object initial);
        protected object instance;
        protected SimpleIoc serviceLocator;
        public abstract void Destroy();

        internal virtual void InstanceCreated(SimpleIoc serviceLocator, object instance)
        {
            this.instance = instance;
            this.serviceLocator = serviceLocator;
        }

        internal virtual object CreateInstance(object initial)
        {
            return initial;
        }
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
        public virtual InstanceLifetime Release(IDestructible destruction)
        {
            throw new NotImplementedException("Can't Release this sort of type.");
        }
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

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
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

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
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
        private string key;
        public override void Destroy()
        {
            //this.instance = instance;
            // does not work ... object is removed
            serviceLocator.Unregister(instance.GetType(), instance, key);
            this.instance = null;
            this.serviceLocator = null;
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
