using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    internal interface IInstanceLifetime
    {
        object CreateInstance(object initial);
    }

    /// <summary>
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
    }

    /// <summary>
    /// SingletonInstance.
    /// </summary>
    [Serializable]
    internal class NormalInstance : IInstanceLifetime
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
        public NormalInstance()
        {
                //Instance = instance;
        }
        #endregion

        #region IInstanceLifetime Members

        public object CreateInstance(object initial)
        {
            return initial;
        }

        #endregion
    }


}
