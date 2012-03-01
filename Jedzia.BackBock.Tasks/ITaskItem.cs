namespace Jedzia.BackBock.Tasks
{
    using System.Collections;

    /// <summary>
    /// Defines an MSBuild item that can be consumed and emitted by tasks.
    /// </summary>
    public interface ITaskItem
    {
        /// <summary>
        /// Gets or sets the item spec.
        /// </summary>
        /// <value>
        /// The item specification.
        /// </value>
        /// <remarks>
        /// The item specification is an arbitrary string. If the item represents a file on disk, the item specification will be the path of that file.
        /// <example>
        /// The <c>ItemSpec</c> for the following item declaration in a project file is File.cs.
        /// <code><![CDATA[
        /// <ItemGroup>
        ///   <Compile Include="File.cs"/>
        /// </ItemGroup>
        /// ]]></code>
        /// </example>
        /// </remarks>
        string ItemSpec { get; set; }
       
        /// <summary>
        /// Gets the names of the metadata entries associated with the item.
        /// </summary>
        ICollection MetadataNames { get; }

        /// <summary>
        /// Gets the number of metadata entries associated with the item.
        /// </summary>
        int MetadataCount { get; }

        /// <summary>
        /// Gets the value of the specified metadata entry.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry.</param>
        /// <returns></returns>
        string GetMetadata(string metadataName);

        /// <summary>
        /// Adds or changes a custom metadata entry to the item.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry.</param>
        /// <param name="metadataValue">The value of the metadata entry.</param>
        void SetMetadata(string metadataName, string metadataValue);

        /// <summary>
        /// Removes the specified metadata entry from the item.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry to remove.</param>
        void RemoveMetadata(string metadataName);

        /// <summary>
        /// Copies the custom metadata entries to another item.
        /// </summary>
        /// <param name="destinationItem">The item to copy the metadata entries to.</param>
        /// <remarks>
        /// Follow the guidelines below when implementing this method.
        /// <list type="bullet">
        /// <item>Do not overwrite the <see cref="ItemSpec"/> property.</item>
        /// <item>Do not overwrite existing metadata entries.</item>
        /// <item>Do not copy metadata entries that do not make sense on the destination item.</item>
        /// </list>
        /// </remarks>
        void CopyMetadataTo(ITaskItem destinationItem);

        /// <summary>
        /// Gets the collection of custom metadata.
        /// </summary>
        /// <returns>
        /// The collection of custom metadata.
        /// </returns>
        /// <remarks>
        ///   <list type="bullet">
        ///   <item>Does not include built-in metadata.</item>
        ///   <item>This method should return a clone of the metadata.</item>
        ///   <item>Writing to this dictionary should not be reflected in the underlying item.</item>
        ///   </list>
        /// </remarks>
        IDictionary CloneCustomMetadata();
    }
}