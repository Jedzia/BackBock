namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of <see cref="IConfiguration"/> objects.
    /// </summary>
    [Serializable]
    public class ConfigurationCollection : List<IConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <c>ConfigurationCollection</c>.
        /// </summary>
        public ConfigurationCollection()
        {
        }

        /// <summary>
        /// Creates a new instance of <c>ConfigurationCollection</c>.
        /// </summary>
        public ConfigurationCollection(IEnumerable<IConfiguration> value)
            : base(value)
        {
        }

        public IConfiguration this[String name]
        {
            get
            {
                foreach (IConfiguration config in this)
                {
                    if (name.Equals(config.Name))
                    {
                        return config;
                    }
                }

                return null;
            }
        }
    }
}