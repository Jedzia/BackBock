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
    using System.Collections;
    using System.ComponentModel;
    using System.Xml;

    [DisplayName("TaskItem")]
    internal sealed class TaskItem : MarshalByRefObject, ITaskItem
    {
        #region Fields

        private readonly BuildItem item;
        private readonly XmlElement mainProjectElement;
        private readonly XmlDocument mainProjectEntireContents;

        #endregion

        #region Constructors

        internal TaskItem()
        {
            // Todo: after XamlReader testing, make this internal.
            //ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");

            this.mainProjectEntireContents = new XmlDocument();
            this.mainProjectElement = this.mainProjectEntireContents.CreateElement(
                "Project", "http://schemas.microsoft.com/developer/msbuild/2003");
            this.mainProjectEntireContents.AppendChild(this.mainProjectElement);


            this.item = new BuildItem(null, string.Empty);
        }

        internal TaskItem(string itemSpec)
        {
            //ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");
            this.item = new BuildItem(null, itemSpec);
        }

        #endregion

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
                //ErrorUtilities.VerifyThrowArgumentNull(value, "ItemSpec");
                //this.item.SetFinalItemSpecEscaped(EscapingUtilities.Escape(value));
                this.item.SetFinalItemSpecEscaped(value);
            }
        }

        /// <summary>
        /// Gets the number of metadata entries associated with the item.
        /// </summary>
        public int MetadataCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the names of the metadata entries associated with the item.
        /// </summary>
        public ICollection MetadataNames
        {
            get
            {
                throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of the specified metadata entry.
        /// </summary>
        /// <param name="metadataName">The name of the metadata entry.</param>
        /// <returns></returns>
        public string GetMetadata(string metadataName)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}