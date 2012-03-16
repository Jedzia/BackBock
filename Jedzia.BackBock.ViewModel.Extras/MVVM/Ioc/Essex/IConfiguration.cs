namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections;

    /// <summary>
    /// <see cref="IConfiguration"/> is a interface encapsulating a configuration node
    ///	used to retrieve configuration values.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        /// <value>
        /// The Name of the node.
        /// </value> 
        String Name { get; }

        /// <summary>
        /// Gets the value of the node.
        /// </summary>
        /// <value>
        /// The Value of the node.
        /// </value> 
        String Value { get; }

        /// <summary>
        /// Gets an <see cref="ConfigurationCollection"/> of <see cref="IConfiguration"/>
        /// elements containing all node children.
        /// </summary>
        /// <value>The Collection of child nodes.</value>
        ConfigurationCollection Children { get; }

        /// <summary>
        /// Gets an <see cref="IDictionary"/> of the configuration attributes.
        /// </summary>
        ConfigurationAttributeCollection Attributes { get; }

        /// <summary>
        /// Gets the value of the node and converts it 
        /// into specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/></param>
        /// <param name="defaultValue">
        /// The Default value returned if the conversion fails.
        /// </param>
        /// <returns>The Value converted into the specified type.</returns>
        object GetValue(Type type, object defaultValue);
    }
}