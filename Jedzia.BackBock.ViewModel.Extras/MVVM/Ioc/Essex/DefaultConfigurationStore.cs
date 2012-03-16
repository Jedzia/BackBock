namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DefaultConfigurationStore : AbstractSubSystem, IConfigurationStore, ISubSystem
    {
        private readonly IDictionary<string, IConfiguration> components = new Dictionary<string, IConfiguration>();
        
        /// <summary>
        ///   Associates a configuration node with a component key
        /// </summary>
        /// <param name = "key">item key</param>
        /// <param name = "config">Configuration node</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddComponentConfiguration(String key, IConfiguration config)
        {
            components[key] = config;
        }

        /// <summary>
        ///   Returns the configuration node associated with
        ///   the specified component key. Should return null
        ///   if no association exists.
        /// </summary>
        /// <param name = "key">item key</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IConfiguration GetComponentConfiguration(String key)
        {
            IConfiguration value;
            components.TryGetValue(key, out value);
            return value;
        }
    }
}