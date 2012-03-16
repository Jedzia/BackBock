namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    /// Summary description for MutableConfiguration.
    /// </summary>
    [Serializable]
    public class MutableConfiguration : AbstractConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MutableConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public MutableConfiguration(String name)
            : this(name, null)
        {
        }

        public MutableConfiguration(String name, String value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the value of <see cref="IConfiguration"/>.
        /// </summary>
        /// <value>
        /// The Value of the <see cref="IConfiguration"/>.
        /// </value>
        public new string Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        public static MutableConfiguration Create(string name)
        {
            return new MutableConfiguration(name);
        }

        public MutableConfiguration Attribute(string name, string value)
        {
            Attributes[name] = value;
            return this;
        }

        public MutableConfiguration CreateChild(string name)
        {
            MutableConfiguration child = new MutableConfiguration(name);
            Children.Add(child);
            return child;
        }

        public MutableConfiguration CreateChild(string name, string value)
        {
            MutableConfiguration child = new MutableConfiguration(name, value);
            Children.Add(child);
            return child;
        }
    }
}