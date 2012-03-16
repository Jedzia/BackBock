namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    using System;
    using System.Collections.Generic;

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
            serviceLocator.Unregister(this.SetupType, Instance, key);
            if (Instance is System.Windows.FrameworkElement)
            {
                var inst = (System.Windows.FrameworkElement)Instance;
                inst.DataContext = null;
            }

            if (Instance is IDestructible)
            {
                var destructible = (IDestructible)Instance;
                // remove this from the IDestructible instance.
                destructible.Candidate = null;
            }
            //serviceLocator.Unregister(key);
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