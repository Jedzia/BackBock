namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Threading;

    /// <summary>
    ///   This is an abstract <see cref = "IConfiguration" /> implementation
    ///   that deals with methods that can be abstracted away
    ///   from underlying implementations.
    /// </summary>
    /// <remarks>
    ///   <para><b>AbstractConfiguration</b> makes easier to implementers 
    ///     to create a new version of <see cref = "IConfiguration" /></para>
    /// </remarks>
    [Serializable]
    public abstract class AbstractConfiguration : IConfiguration
    {
        private readonly ConfigurationAttributeCollection attributes = new ConfigurationAttributeCollection();
        private readonly ConfigurationCollection children = new ConfigurationCollection();

        /// <summary>
        ///   Gets node attributes.
        /// </summary>
        /// <value>
        ///   All attributes of the node.
        /// </value>
        public virtual ConfigurationAttributeCollection Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        ///   Gets all child nodes.
        /// </summary>
        /// <value>The <see cref = "ConfigurationCollection" /> of child nodes.</value>
        public virtual ConfigurationCollection Children
        {
            get { return children; }
        }

        /// <summary>
        ///   Gets the name of the <see cref = "IConfiguration" />.
        /// </summary>
        /// <value>
        ///   The Name of the <see cref = "IConfiguration" />.
        /// </value>
        public string Name { get; protected set; }

        /// <summary>
        ///   Gets the value of <see cref = "IConfiguration" />.
        /// </summary>
        /// <value>
        ///   The Value of the <see cref = "IConfiguration" />.
        /// </value>
        public string Value { get; protected set; }

        /// <summary>
        ///   Gets the value of the node and converts it
        ///   into specified <see cref = "Type" />.
        /// </summary>
        /// <param name = "type">The <see cref = "Type" /></param>
        /// <param name = "defaultValue">
        ///   The Default value returned if the conversion fails.
        /// </param>
        /// <returns>The Value converted into the specified type.</returns>
        public virtual object GetValue(Type type, object defaultValue)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            try
            {
                return Convert.ChangeType(Value, type, Thread.CurrentThread.CurrentCulture);
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }
    }
}