namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Indicates that the target components wants a
    ///   pooled lifestyle.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PooledAttribute : LifestyleAttribute
    {
        private static readonly int Initial_PoolSize = 5;
        private static readonly int Max_PoolSize = 15;

        private readonly int initialPoolSize;
        private readonly int maxPoolSize;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PooledAttribute" /> class
        ///   using the default initial pool size (5) and the max pool size (15).
        /// </summary>
        public PooledAttribute()
            : this(Initial_PoolSize, Max_PoolSize)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PooledAttribute" /> class.
        /// </summary>
        /// <param name = "initialPoolSize">Initial size of the pool.</param>
        /// <param name = "maxPoolSize">Max pool size.</param>
        public PooledAttribute(int initialPoolSize, int maxPoolSize)
            : base(LifestyleType.Pooled)
        {
            this.initialPoolSize = initialPoolSize;
            this.maxPoolSize = maxPoolSize;
        }

        /// <summary>
        ///   Gets the initial size of the pool.
        /// </summary>
        /// <value>The initial size of the pool.</value>
        public int InitialPoolSize
        {
            get { return initialPoolSize; }
        }

        /// <summary>
        ///   Gets the maximum pool size.
        /// </summary>
        /// <value>The size of the max pool.</value>
        public int MaxPoolSize
        {
            get { return maxPoolSize; }
        }
    }
}