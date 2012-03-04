// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml;
    using Jedzia.BackBock.Tasks.BuildEngine;
    using Jedzia.BackBock.Tasks.Shared;
    using Jedzia.BackBock.Tasks.Utilities;
    using System.Text;

    /// <summary>
    /// Defines a single item of the project as it is passed into a task.
    /// </summary>
    [DisplayName("TaskItem")]
    public sealed class TaskItem : MarshalByRefObject, ITaskItem
    {
        #region Fields

        private readonly BuildItem2 item;
        private readonly XmlElement mainProjectElement;
        private readonly XmlDocument mainProjectEntireContents;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="TaskItem"/> class from being created.
        /// </summary>
        /// <remarks>This constructor enables this type to be COM-creatable.</remarks>
        private TaskItem()
        {
            // Todo: after XamlReader testing, make this internal.
            /*ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");

            this.mainProjectEntireContents = new XmlDocument();
            this.mainProjectElement = this.mainProjectEntireContents.CreateElement(
                "Project", "http://schemas.microsoft.com/developer/msbuild/2003");
            this.mainProjectEntireContents.AppendChild(this.mainProjectElement);


            this.item = new BuildItem(null, string.Empty);*/
        }
        
        /*internal TaskItem(BuildItem item)
        {
            ErrorUtilities.VerifyThrow(item != null, "Need to specify backing item.");
            this.item = item.VirtualClone();
        }*/

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem"/> class
        /// using the specified source ITaskItem.
        /// </summary>
        /// <param name="sourceItem">The item to copy.</param>
        public TaskItem(ITaskItem sourceItem)
            : this(sourceItem.ItemSpec)
        {
            ErrorUtilities.VerifyThrowArgumentNull(sourceItem, "sourceItem");
            sourceItem.CopyMetadataTo(this);
        }

 

 


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem"/> class
        /// using the specified item-specification string.
        /// </summary>
        /// <param name="itemSpec">The item specification.</param>
        public TaskItem(string itemSpec)
        {
            // Todo: after XamlReader testing, make this internal.
            ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");
            this.item = new BuildItem2(null, itemSpec);
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ItemSpec: ");
            sb.Append(this.ItemSpec);
            return sb.ToString();
        }


        #region Properties

        /// <summary>
        /// Gets or sets the item specification.
        /// </summary>
        /// <value>
        /// A String that represents the item specification.
        /// </value>
        /// <remarks>
        /// The ItemSpec for the following item declaration in a project file is File.cs
        /// <example>
        /// <code><![CDATA[
        /// <ItemGroup>
        ///    <Compile Include="File.cs"/>
        /// </ItemGroup>
        /// ]]>
        /// </code>
        /// </example>
        /// </remarks>
        public string ItemSpec
        {
            get
            {
                return this.item.FinalItemSpec;
            }
            set
            {
                ErrorUtilities.VerifyThrowArgumentNull(value, "ItemSpec");
                this.item.SetFinalItemSpecEscaped(EscapingUtilities.Escape(value));
            }
        }

        /// <summary>
        /// Gets the number of metadata entries associated with the item.
        /// </summary>
        [Browsable(false)]
        public int MetadataCount
        {
            get
            {
                return (this.item.GetCustomMetadataCount() + FileUtilities.ItemSpecModifiers.All.Length);
            }
        }

        /// <summary>
        /// Gets the names of all the metadata on the item.
        /// </summary>
        [Browsable(false)]
        public ICollection MetadataNames
        {
            get
            {
                ArrayList allCustomMetadataNames = this.item.GetAllCustomMetadataNames();
                // Todo: check the correctness of this comment, the custom names are not exposed now.
                allCustomMetadataNames.AddRange(FileUtilities.ItemSpecModifiers.All);
                var distinct = allCustomMetadataNames.ToArray().Distinct();
                return distinct.ToList();
                //return allCustomMetadataNames;
            }
        }

        #endregion

        /// <summary>
        /// Gets the collection of custom metadata.
        /// </summary>
        /// <returns>
        /// The collection of custom metadata.
        /// </returns>
        public IDictionary CloneCustomMetadata()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the custom metadata entries to another item.
        /// </summary>
        /// <param name="destinationItem">The item to copy the metadata entries to.</param>
        public void CopyMetadataTo(ITaskItem destinationItem)
        {
            ErrorUtilities.VerifyThrowArgumentNull(destinationItem, "destinationItem");
            foreach (DictionaryEntry entry in this.item.GetAllCustomEvaluatedMetadata())
            {
                string key = (string)entry.Key;
                string str2 = destinationItem.GetMetadata(key);
                if ((str2 == null) || (str2.Length == 0))
                {
                    destinationItem.SetMetadata(key, EscapingUtilities.UnescapeAll((string)entry.Value));
                }
            }
            string metadata = destinationItem.GetMetadata("OriginalItemSpec");
            if ((metadata == null) || (metadata.Length == 0))
            {
                destinationItem.SetMetadata("OriginalItemSpec", this.ItemSpec);
            }
        }

        /// <summary>
        /// Gets the value of the specified metadata entry.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry.</param>
        /// <returns></returns>
        public string GetMetadata(string metadataName)
        {
            ErrorUtilities.VerifyThrowArgumentNull(metadataName, "metadataName");
            return this.item.GetEvaluatedMetadata(metadataName);
        }

        /// <summary>
        /// Removes the specified metadata entry from the item.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry to remove.</param>
        public void RemoveMetadata(string metadataName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds or changes a custom metadata entry to the item.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry.</param>
        /// <param name="metadataValue">The value of the metadata entry.</param>
        public void SetMetadata(string metadataName, string metadataValue)
        {
            ErrorUtilities.VerifyThrowArgumentLength(metadataName, "metadataName");
            ErrorUtilities.VerifyThrowArgumentNull(metadataValue, "metadataValue");
            this.item.SetMetadata(metadataName, EscapingUtilities.Escape(metadataValue));
        }
    }

    /// <summary>
    /// Represents a single item in an MSBuild project. This is a placeholder class.
    /// </summary>
    /// <remarks>
    /// An Item element in a project can represent multiple items through the use of
    /// wildcards. Therefore, each BuildItem object does not necessarily represent an
    /// Item element in the project.
    /// </remarks>
    /// <example>
    /// The following example creates a Project object and uses the LoadXml method to
    /// add content to the project. The BuildItem, BuildItemGroup, and
    /// BuildItemGroupCollection classes are used to add, remove, and change items in
    /// the project. 
    /// <code lang="C#">
    /// using System;
    /// using System.Collections.Generic;
    /// using System.Text;
    /// 
    /// using Microsoft.Build.BuildEngine;
    /// 
    /// namespace AddNewItem
    /// {
    ///     class Program
    ///     {
    ///         /// &lt;summary&gt;
    ///         /// This code demonstrates the use of the following methods:
    ///         ///     Engine constructor
    ///         ///     Project constructor
    ///         ///     Project.LoadFromXml
    ///         ///     Project.Xml
    ///         ///     BuildItemGroupCollection.GetEnumerator
    ///         ///     BuildItemGroup.GetEnumerator
    ///         ///     BuildItem.Name (get)
    ///         ///     BuildItem.Include (set)
    ///         ///     BuildItem.GetMetadata
    ///         ///     BuildItem.SetMetadata
    ///         ///     BuildItemGroup.RemoveItem
    ///         ///     BuildItemGroup.AddNewItem
    ///         /// &lt;/summary&gt;
    ///         /// &lt;param name=&quot;args&quot;&gt;&lt;/param&gt;
    ///         static void Main(string[] args)
    ///         {
    ///             // Create a new Engine object.
    ///             Engine engine = new Engine(Environment.CurrentDirectory);
    /// 
    ///             // Create a new Project object.
    ///             Project project = new Project(engine);
    /// 
    ///             // Load the project with the following XML, which contains
    ///             // two ItemGroups.
    ///             project.LoadXml(@&quot;
    ///                 &lt;Project
    /// xmlns='http://schemas.microsoft.com/developer/msbuild/2003'&gt;
    /// 
    ///                     &lt;ItemGroup&gt;
    ///                         &lt;Compile Include='Program.cs'/&gt;
    ///                         &lt;Compile Include='Class1.cs'/&gt;
    ///                         &lt;RemoveThisItemPlease Include='readme.txt'/&gt;
    ///                     &lt;/ItemGroup&gt;
    /// 
    ///                     &lt;ItemGroup&gt;
    ///                         &lt;EmbeddedResource Include='Strings.resx'&gt;
    /// 
    /// &lt;LogicalName&gt;Strings.resources&lt;/LogicalName&gt;
    ///                             &lt;Culture&gt;fr-fr&lt;/Culture&gt;
    ///                         &lt;/EmbeddedResource&gt;
    ///                     &lt;/ItemGroup&gt;
    /// 
    ///                 &lt;/Project&gt;
    ///                 &quot;);
    /// 
    ///             // Iterate through each ItemGroup in the Project.  There are two.
    ///             foreach (BuildItemGroup ig in project.ItemGroups)
    ///             {
    ///                 BuildItem itemToRemove = null;
    /// 
    ///                 // Iterate through each Item in the ItemGroup.
    ///                 foreach (BuildItem item in ig)
    ///                 {
    ///                     // If the item's name is &quot;RemoveThisItemPlease&quot;,
    /// then
    ///                     // store a reference to this item in a local variable,
    ///                     // so we can remove it later.
    ///                     if (item.Name == &quot;RemoveThisItemPlease&quot;)
    ///                     {
    ///                         itemToRemove = item;
    ///                     }
    /// 
    ///                     // If the item's name is &quot;EmbeddedResource&quot; and it
    /// has a metadata Culture
    ///                     // set to &quot;fr-fr&quot;, then ...
    ///                     if ((item.Name == &quot;EmbeddedResource&quot;) &amp;&amp;
    /// (item.GetMetadata(&quot;Culture&quot;) == &quot;fr-fr&quot;))
    ///                     {
    ///                         // Change the item's Include path to
    /// &quot;FrenchStrings.fr.resx&quot;,
    ///                         // and add a new metadata Visiable=&quot;false&quot;.
    ///                         item.Include = @&quot;FrenchStrings.fr.resx&quot;;
    ///                         item.SetMetadata(&quot;Visible&quot;,
    /// &quot;false&quot;);
    ///                     }
    ///                 }
    /// 
    ///                 // Remove the item named &quot;RemoveThisItemPlease&quot; from
    /// the
    ///                 // ItemGroup
    ///                 if (itemToRemove != null)
    ///                 {
    ///                     ig.RemoveItem(itemToRemove);
    ///                 }
    /// 
    ///                 // For each ItemGroup that we found, add to the end of it
    ///                 // a new item Content with Include=&quot;SplashScreen.bmp&quot;.
    ///                 ig.AddNewItem(&quot;Content&quot;,
    /// &quot;SplashScreen.bmp&quot;);
    ///             }
    /// 
    ///             // The project now looks like this:
    ///             //
    ///             //     &lt;?xml version=&quot;1.0&quot;
    /// encoding=&quot;utf-16&quot;?&gt;
    ///             //     &lt;Project
    /// xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
    ///             //       &lt;ItemGroup&gt;
    ///             //         &lt;Compile Include=&quot;Program.cs&quot; /&gt;
    ///             //         &lt;Compile Include=&quot;Class1.cs&quot; /&gt;
    ///             //         &lt;Content Include=&quot;SplashScreen.bmp&quot; /&gt;
    ///             //       &lt;/ItemGroup&gt;
    ///             //       &lt;ItemGroup&gt;
    ///             //         &lt;EmbeddedResource
    /// Include=&quot;FrenchStrings.fr.resx&quot;&gt;
    ///             //
    /// &lt;LogicalName&gt;Strings.resources&lt;/LogicalName&gt;
    ///             //           &lt;Culture&gt;fr-fr&lt;/Culture&gt;
    ///             //           &lt;Visible&gt;false&lt;/Visible&gt;
    ///             //         &lt;/EmbeddedResource&gt;
    ///             //         &lt;Content Include=&quot;SplashScreen.bmp&quot; /&gt;
    ///             //       &lt;/ItemGroup&gt;
    ///             //     &lt;/Project&gt;
    ///             //
    ///             Console.WriteLine(project.Xml);
    ///         }
    ///     }
    /// }</code>
    /// </example>
    internal class BuildItem2
    {
        private string finalItemSpec;

        public BuildItem2(string itemName, string itemInclude)
        {
            this.finalItemSpec = itemInclude;
        }

        public string FinalItemSpec
        {
            get
            {
                return finalItemSpec;
            }
            set
            {
                finalItemSpec = value;
            }
        }

        internal ArrayList GetAllCustomMetadataNames()
        {
            ErrorUtilities.VerifyThrow(this.metadata != null, "Item not initialized properly. unevaluatedCustomAttributes is null.");
            return new ArrayList(this.metadata.Keys);
        }

        public IEnumerable<DictionaryEntry> GetAllCustomEvaluatedMetadata()
        {
            //List<DictionaryEntry> dl = new List<DictionaryEntry>();
            foreach (var item in metadata)
            {
                var de = new DictionaryEntry(item.Key, item.Value);
                //dl.Add(de);
                yield return de;
            }

            //return dl;
            //return new DictionaryEntry[0];
        }

        public void SetFinalItemSpecEscaped(string escape)
        {
            FinalItemSpec = escape;
        }
        internal int GetCustomMetadataCount()
        {
            ErrorUtilities.VerifyThrow(this.metadata != null, "Item not initialized properly. unevaluatedCustomAttributes is null.");
            return this.metadata.Count;
        }

        public string GetEvaluatedMetadata(string metadataName)
        {
            string result = null;
            var found = metadata.TryGetValue(metadataName, out result);
            if (found)
            {

            }
            return result;
        }
        //Hashtable metadata = new Hashtable();
        //HashSet<string> metadata = new HashSet<string>();
        Dictionary<string,string> metadata = new Dictionary<string,string>();
        public void SetMetadata(string metadataName, string escape)
        {
            metadata[metadataName] = escape;
            string result;
            var found = metadata.TryGetValue(metadataName, out result);
            if (found)
            {
                
            }
        }

        public BuildItem2 VirtualClone()
        {
            throw new NotImplementedException();
        }
    }
}