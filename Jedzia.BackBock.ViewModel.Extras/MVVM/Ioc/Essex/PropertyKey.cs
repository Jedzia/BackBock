namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Represents a property key.
    /// </summary>
    public class PropertyKey
    {
        private readonly object key;

        internal PropertyKey(object key)
        {
            this.key = key;
        }

        /// <summary>
        ///   The property key key.
        /// </summary>
        public object Key
        {
            get { return key; }
        }

        /// <summary>
        ///   Builds the <see cref = "Property" /> with key/value.
        /// </summary>
        /// <param key = "value">The property value.</param>
        /// <returns>The new <see cref = "Property" /></returns>
        public Property Eq(Object value)
        {
            return new Property(key, value);
        }

        /*/// <summary>
		///   Builds a service override using other component registered with given <paramref name = "componentName" /> as value for dependency with given <see
		///    cref = "Key" />.
		/// </summary>
		/// <param name = "componentName"></param>
		/// <returns></returns>
		public ServiceOverride Is(string componentName)
		{
			return GetServiceOverrideKey().Eq(componentName);
		}

		/// <summary>
		///   Builds a service override using other component registered with given <paramref name = "componentImplementation" /> and no explicit name, as value for dependency with given <see
		///    cref = "Key" />.
		/// </summary>
		/// <returns></returns>
		public ServiceOverride Is(Type componentImplementation)
		{
			if (componentImplementation == null)
			{
				throw new ArgumentNullException("componentImplementation");
			}
			return GetServiceOverrideKey().Eq(ComponentName.DefaultNameFor(componentImplementation));
		}*/

        /*/// <summary>
		///   Builds a service override using other component registered with given <typeparam name = "TComponentImplementation" /> and no explicit name, as value for dependency with given <see
		///    cref = "Key" />.
		/// </summary>
		/// <returns></returns>
		public ServiceOverride Is<TComponentImplementation>()
		{
			return Is(typeof(TComponentImplementation));
		}

		private ServiceOverrideKey GetServiceOverrideKey()
		{
			if (key is Type)
			{
				return ServiceOverride.ForKey((Type)key);
			}
			return ServiceOverride.ForKey((string)key);
		}*/
    }
}