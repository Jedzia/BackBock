namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Only to hold internal constants and get rid of 
    ///   magic numbers and hardcode names.
    /// </summary>
    internal abstract class ExtendedPropertiesConstants
    {
        public static readonly int Pool_Default_InitialPoolSize = 5;
        public static readonly int Pool_Default_MaxPoolSize = 15;

        public static readonly String Pool_InitialPoolSize = "pool.initial.pool.size";
        public static readonly String Pool_MaxPoolSize = "pool.max.pool.size";
    }
}