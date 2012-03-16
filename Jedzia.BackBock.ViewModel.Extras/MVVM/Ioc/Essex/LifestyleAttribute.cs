namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Base for Attributes that want to express lifestyle
    ///   chosen by the component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class LifestyleAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "LifestyleAttribute" /> class.
        /// </summary>
        /// <param name = "type">The type.</param>
        protected LifestyleAttribute(LifestyleType type)
        {
            Lifestyle = type;
        }

        /// <summary>
        ///   Gets or sets the lifestyle.
        /// </summary>
        /// <value>The lifestyle.</value>
        public LifestyleType Lifestyle { get; set; }
    }
}