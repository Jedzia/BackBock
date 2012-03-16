namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Indicates that the target components wants a
    ///   custom lifestyle.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomLifestyleAttribute : LifestyleAttribute
    {
        private readonly Type lifestyleHandlerType;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CustomLifestyleAttribute" /> class.
        /// </summary>
        /// <param name = "lifestyleHandlerType">The lifestyle handler.</param>
        public CustomLifestyleAttribute(Type lifestyleHandlerType)
            : base(LifestyleType.Custom)
        {
            this.lifestyleHandlerType = lifestyleHandlerType;
        }

        /// <summary>
        ///   Gets the type of the lifestyle handler.
        /// </summary>
        /// <value>The type of the lifestyle handler.</value>
        public Type LifestyleHandlerType
        {
            get { return lifestyleHandlerType; }
        }
    }
}