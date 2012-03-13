namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    using System;
    using System.Collections.Generic;

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
}