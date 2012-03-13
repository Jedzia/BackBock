namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    using System;
    using System.Collections.Generic;

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
}