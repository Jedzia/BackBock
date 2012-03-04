﻿// <copyright file="$FileName$" company="$Company$">
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

    [DisplayName("TaskItem")]
    public sealed class TaskItem : MarshalByRefObject, ITaskItem
    {
        #region Fields

        private readonly BuildItem2 item;
        private readonly XmlElement mainProjectElement;
        private readonly XmlDocument mainProjectEntireContents;

        #endregion

        #region Constructors

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

        public TaskItem(ITaskItem sourceItem)
            : this(sourceItem.ItemSpec)
        {
            ErrorUtilities.VerifyThrowArgumentNull(sourceItem, "sourceItem");
            sourceItem.CopyMetadataTo(this);
        }

 

 


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem"/> class.
        /// </summary>
        /// <param name="itemSpec">The item spec.</param>
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
        /// Gets or sets the item spec.
        /// </summary>
        /// <value>
        /// The item specification.
        /// </value>
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
        /// Gets the names of the metadata entries associated with the item.
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