namespace Jedzia.BackBock.ViewModel
{
    using System;

    /// <summary>
    /// Declares which type a referenced item has to be of.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CheckTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the reference type.
        /// </summary>
        /// <value>
        /// The reference type of the item.
        /// </value>
        public Type Type
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">Declares which type the referenced item has to be of.</param>
        public CheckTypeAttribute(Type type)
        {
            this.Type = type;
        }
    }
}