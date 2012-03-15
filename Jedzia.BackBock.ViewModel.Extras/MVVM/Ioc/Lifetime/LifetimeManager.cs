namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    using System;

    /// <summary>
    /// Manges the destruction of <see cref="SimpleIoc"/> container objects.
    /// </summary>
    /// <typeparam name="T">The object to provide with destruction abilities.</typeparam>
    public class LifetimeManager<T> : ILifetimeConfig<T>, ILifetimeManagement where T : class
    {
        private InstanceLifetime instanceLifetime;
        Action<T> action;

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

        public void DoDestroy()
        {
            if (action != null)
            {
                action((T)instanceLifetime.Instance);
                //this.action((ILifetimeManagement<T>)instanceLifetime);
            }
        }

        public void DestroyOnEvent(object handler)
        {
            //EventHandler
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
}
