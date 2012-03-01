namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml;
    using Jedzia.BackBock.Tasks.Utilities;


    /// <summary>
    /// Represents a single item in an Task project.
    /// </summary>
    [DebuggerDisplay("BuildItem (Name = { Name }, Include = { Include }, FinalItemSpec = { FinalItemSpec }, Condition = { Condition })")]
    [DisplayName("BuildItem")]
    public class BuildItem
    {
        private string finalItemSpecEscaped;
        private Hashtable itemSpecModifiers;
        private string recursivePortionOfFinalItemSpecDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildItem"/> class 
        /// with the specified Name and Include property values..
        /// </summary>
        /// <param name="itemName">The Name property of the BuildItem.</param>
        /// <param name="itemInclude">The Include property of the BuildItem.</param>
        public BuildItem(string itemName, string itemInclude)
            : this(null, itemName, itemInclude)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildItem"/> class based on an <see cref="ITaskItem"/> object..
        /// </summary>
        /// <param name="itemName">The Name property of the BuildItem.</param>
        /// <param name="taskItem">The <see cref="ITaskItem"/> from which to create the Include property of the BuildItem.</param>
        public BuildItem(string itemName, ITaskItem taskItem)
            : this(null, itemName, EscapingUtilities.Escape(taskItem.ItemSpec), false)
        {
            IDictionary dictionary = taskItem.CloneCustomMetadata();
            var array = new string[dictionary.Count];
            dictionary.Keys.CopyTo(array, 0);
            foreach (string str in array)
            {
                var unescapedString = (string)dictionary[str];
                dictionary[str] = EscapingUtilities.Escape(unescapedString);
            }
            //this.unevaluatedCustomMetadata = new CopyOnWriteHashtable(dictionary, StringComparer.OrdinalIgnoreCase);
            //this.evaluatedCustomMetadata = new CopyOnWriteHashtable(dictionary, StringComparer.OrdinalIgnoreCase);
        }


        internal BuildItem(XmlDocument ownerDocument, string itemName, string itemInclude)
            : this(ownerDocument, itemName, itemInclude, true)
        {
        }

        private BuildItem(XmlDocument ownerDocument, string itemName, string itemInclude, bool createCustomMetadataCache)
        {
            //this.finalItemSpecEscaped = string.Empty;
            this.finalItemSpecEscaped = itemInclude;
        }

        /// <summary>
        /// Gets the final specification of the item after all wildcards and properties have been evaluated.
        /// </summary>
        public string FinalItemSpec
        {
            get
            {
                //return EscapingUtilities.UnescapeAll(this.FinalItemSpecEscaped);
                return this.FinalItemSpecEscaped;
            }
        }
        
        internal string FinalItemSpecEscaped
        {
            get
            {
                return this.finalItemSpecEscaped;
            }
        }

        internal void SetFinalItemSpecEscaped(string finalItemSpecValueEscaped)
        {
            this.finalItemSpecEscaped = finalItemSpecValueEscaped;
            this.itemSpecModifiers = null;
            this.recursivePortionOfFinalItemSpecDirectory = null;
        }


    }
}