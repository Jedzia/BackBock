namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public interface IConfigurationStore : ISubSystem
    {
        /// <summary>
        ///   Returns the configuration node associated with 
        ///   the specified component key. Should return null
        ///   if no association exists.
        /// </summary>
        /// <param name = "key">item key</param>
        /// <returns></returns>
        IConfiguration GetComponentConfiguration(String key);

        /// <summary>
        ///   Associates a configuration node with a component key
        /// </summary>
        /// <param name = "key">item key</param>
        /// <param name = "config">Configuration node</param>
        void AddComponentConfiguration(String key, IConfiguration config);
    }
}