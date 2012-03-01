// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks.BuildEngine
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml;
    using Jedzia.BackBock.Tasks.Shared;
    using Jedzia.BackBock.Tasks.Utilities;

    /// <summary>
    /// Represents a single item in an Task project.
    /// </summary>
    [DebuggerDisplay(
        "BuildItem (Name = { Name }, Include = { Include }, FinalItemSpec = { FinalItemSpec }, Condition = { Condition })"
        )]
    [DisplayName("BuildItem")]
    public class BuildItem
    {
        #region Fields

        private BuildItemGroup childItems;
        private XmlAttribute conditionAttribute;
        private CopyOnWriteHashtable evaluatedCustomMetadata;
        private string evaluatedItemSpecEscaped;
        private XmlAttribute excludeAttribute;
        private string finalItemSpecEscaped;
        private bool importedFromAnotherProject;
        private XmlAttribute includeAttribute;
        private XmlElement itemElement;
        private Hashtable itemSpecModifiers;
        private string name;
        private BuildItem parentPersistedItem;
        private BuildItemGroup parentPersistedItemGroup;
        private string recursivePortionOfFinalItemSpecDirectory;
        private CopyOnWriteHashtable unevaluatedCustomMetadata;
        private string virtualIncludeAttribute;

        #endregion

        #region Constructors

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
            foreach(string str in array)
            {
                var unescapedString = (string)dictionary[str];
                dictionary[str] = EscapingUtilities.Escape(unescapedString);
            }
            this.unevaluatedCustomMetadata = new CopyOnWriteHashtable(dictionary, StringComparer.OrdinalIgnoreCase);
            this.evaluatedCustomMetadata = new CopyOnWriteHashtable(dictionary, StringComparer.OrdinalIgnoreCase);
        }


        internal BuildItem(XmlDocument ownerDocument, string itemName, string itemInclude)
            : this(ownerDocument, itemName, itemInclude, true)
        {
        }


        private BuildItem(
            XmlDocument ownerDocument, string itemName, string itemInclude, bool createCustomMetadataCache)
        {
            this.finalItemSpecEscaped = string.Empty;
            ErrorUtilities.VerifyThrowArgumentLength(itemInclude, "itemInclude");
            if (itemName != null)
            {
                XmlUtilities.VerifyThrowValidElementName(itemName);
            }
            if (ownerDocument == null)
            {
                this.itemElement = null;
                this.includeAttribute = null;
                this.virtualIncludeAttribute = itemInclude;
            }
            else
            {
                ErrorUtilities.VerifyThrowArgumentLength(itemName, "itemType");
                this.itemElement = ownerDocument.CreateElement(itemName, "http://schemas.microsoft.com/developer/msbuild/2003");
                this.itemElement.SetAttribute("Include", itemInclude);
                this.includeAttribute = this.itemElement.Attributes["Include"];
            }
            if (createCustomMetadataCache)
            {
                this.InitializeCustomMetadataCache();
            }
            this.name = itemName;
            if (this.name != null)
            {
                ErrorUtilities.VerifyThrowInvalidOperation(XMakeElements.IllegalItemPropertyNames[this.name] == null, "CannotModifyReservedItem", this.name);
            }
            this.excludeAttribute = null;
            this.conditionAttribute = null;
            this.evaluatedItemSpecEscaped = itemInclude;
            this.finalItemSpecEscaped = itemInclude;
            this.importedFromAnotherProject = false;
        }

        #endregion

        #region Properties

        public string Condition
        {
            get
            {
                if (this.conditionAttribute != null)
                {
                    return this.conditionAttribute.Value;
                }
                return string.Empty;
            }
            set
            {
                ErrorUtilities.VerifyThrowInvalidOperation(this.itemElement != null, "CannotSetCondition");
                ErrorUtilities.VerifyThrowInvalidOperation(
                    !this.importedFromAnotherProject, "CannotModifyImportedProjects");
                this.SplitChildItemIfNecessary();
                if ((value == null) || (value.Length == 0))
                {
                    this.itemElement.RemoveAttribute("Condition");
                    this.conditionAttribute = null;
                }
                else
                {
                    this.itemElement.SetAttribute("Condition", value);
                    this.conditionAttribute = this.itemElement.Attributes["Condition"];
                }
                this.MarkItemAsDirty();
            }
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

        public string Include
        {
            get
            {
                if (this.includeAttribute != null)
                {
                    return this.includeAttribute.Value;
                }
                if (this.virtualIncludeAttribute != null)
                {
                    return this.virtualIncludeAttribute;
                }
                ErrorUtilities.VerifyThrow(false, "Item has not been initialized.");
                return null;
            }
            set
            {
                ErrorUtilities.VerifyThrowArgument(value != null, "NullIncludeNotAllowed", "Include");
                ErrorUtilities.VerifyThrowInvalidOperation(
                    !this.importedFromAnotherProject, "CannotModifyImportedProjects");
                if (this.includeAttribute != null)
                {
                    if ((this.ParentPersistedItem != null) &&
                        this.ParentPersistedItem.NewItemSpecMatchesExistingWildcard(value))
                    {
                        this.MarkItemAsDirtyForReevaluation();
                    }
                    else
                    {
                        this.SplitChildItemIfNecessary();
                        this.includeAttribute.Value = value;
                        this.MarkItemAsDirty();
                    }
                }
                else if (this.virtualIncludeAttribute != null)
                {
                    this.virtualIncludeAttribute = value;
                    this.MarkItemAsDirty();
                }
                else
                {
                    ErrorUtilities.VerifyThrow(false, "Item has not been initialized.");
                }
                this.evaluatedItemSpecEscaped = value;
                this.finalItemSpecEscaped = value;
            }
        }

        public bool IsImported
        {
            get
            {
                return this.importedFromAnotherProject;
            }
        }

        public string Name
        {
            get
            {
                ErrorUtilities.VerifyThrow(this.name != null, "Item has not been initialized.");
                return this.name;
            }
            set
            {
                ErrorUtilities.VerifyThrow(this.name != null, "Item has not been initialized.");
                ErrorUtilities.VerifyThrowArgumentLength(value, "Name");
                XmlUtilities.VerifyThrowValidElementName(value);
                ErrorUtilities.VerifyThrowInvalidOperation(
                    !this.importedFromAnotherProject, "CannotModifyImportedProjects");
                //ErrorUtilities.VerifyThrowInvalidOperation(XMakeElements.IllegalItemPropertyNames[value] == null, "CannotModifyReservedItem", value);
                this.SplitChildItemIfNecessary();
                this.name = value;
                if (this.itemElement != null)
                {
                    this.itemElement = XmlUtilities.RenameXmlElement(
                        this.itemElement, value, "http://schemas.microsoft.com/developer/msbuild/2003");
                    this.includeAttribute = this.itemElement.Attributes["Include"];
                    this.excludeAttribute = this.itemElement.Attributes["Exclude"];
                    this.conditionAttribute = this.itemElement.Attributes["Condition"];
                    if (this.ParentPersistedItem != null)
                    {
                        this.ParentPersistedItem.name = this.name;
                        this.ParentPersistedItem.itemElement = this.itemElement;
                        this.ParentPersistedItem.includeAttribute = this.includeAttribute;
                        this.ParentPersistedItem.excludeAttribute = this.excludeAttribute;
                        this.ParentPersistedItem.conditionAttribute = this.conditionAttribute;
                    }
                    if (this.childItems != null)
                    {
                        foreach(BuildItem item in this.childItems)
                        {
                            item.name = this.name;
                            item.itemElement = this.itemElement;
                            item.includeAttribute = this.includeAttribute;
                            item.excludeAttribute = this.excludeAttribute;
                            item.conditionAttribute = this.conditionAttribute;
                        }
                    }
                }
                this.MarkItemAsDirty();
            }
        }

        internal BuildItemGroup ChildItems
        {
            get
            {
                if (this.childItems == null)
                {
                    this.childItems = new BuildItemGroup();
                }
                return this.childItems;
            }
        }

        internal string FinalItemSpecEscaped
        {
            get
            {
                return this.finalItemSpecEscaped;
            }
        }

        internal XmlElement ItemElement
        {
            get
            {
                return this.itemElement;
            }
        }

        internal BuildItem ParentPersistedItem
        {
            get
            {
                return this.parentPersistedItem;
            }
            set
            {
                ErrorUtilities.VerifyThrow(
                    ((value == null) && (this.parentPersistedItem != null)) ||
                    ((value != null) && (this.parentPersistedItem == null)),
                    "Either new parent cannot be assigned because we already have a parent, or old parent cannot be removed because none exists.");
                this.parentPersistedItem = value;
            }
        }

        internal BuildItemGroup ParentPersistedItemGroup
        {
            get
            {
                return this.parentPersistedItemGroup;
            }
            set
            {
                ErrorUtilities.VerifyThrow(
                    ((value == null) && (this.parentPersistedItemGroup != null)) ||
                    ((value != null) && (this.parentPersistedItemGroup == null)),
                    "Either new parent cannot be assigned because we already have a parent, or old parent cannot be removed because none exists.");
                this.parentPersistedItemGroup = value;
            }
        }

        #endregion

        public string GetEvaluatedMetadata(string metadataName)
        {
            return EscapingUtilities.UnescapeAll(this.GetEvaluatedMetadataEscaped(metadataName));
        }

        public void SetMetadata(string metadataName, string metadataValue)
        {
            ErrorUtilities.VerifyThrowArgument(
                !FileUtilities.IsDerivableItemSpecModifier(metadataName),
                "Shared.CannotChangeItemSpecModifiers",
                metadataName);
            ErrorUtilities.VerifyThrowArgumentLength(metadataName, "metadataName");
            ErrorUtilities.VerifyThrowArgumentNull(metadataValue, "metadataValue");
            XmlUtilities.VerifyThrowValidElementName(metadataName);
            ErrorUtilities.VerifyThrowInvalidOperation(XMakeElements.IllegalItemPropertyNames[metadataName] == null, "CannotModifyReservedItemMetadata", metadataName);
            ErrorUtilities.VerifyThrowInvalidOperation(!this.importedFromAnotherProject, "CannotModifyImportedProjects");
            ErrorUtilities.VerifyThrow(
                (this.unevaluatedCustomMetadata != null) && (this.evaluatedCustomMetadata != null),
                "Item not initialized properly.");
            this.SetMetadataNoChecks(metadataName, metadataValue);
        }

        internal string ExtractRecursivePortionOfFinalItemSpecDirectory()
        {
            if (this.recursivePortionOfFinalItemSpecDirectory == null)
            {
                this.recursivePortionOfFinalItemSpecDirectory = string.Empty;
                if (this.unevaluatedCustomMetadata["RecursiveDir"] != null)
                {
                    this.recursivePortionOfFinalItemSpecDirectory =
                        (string)this.unevaluatedCustomMetadata["RecursiveDir"];
                    this.unevaluatedCustomMetadata.Remove("RecursiveDir");
                    this.evaluatedCustomMetadata.Remove("RecursiveDir");
                }
                else if (this.evaluatedItemSpecEscaped != null)
                {
                    FileMatcher.Result result = FileMatcher.FileMatch(
                        this.evaluatedItemSpecEscaped, this.FinalItemSpecEscaped);
                    if (result.isLegalFileSpec && result.isMatch)
                    {
                        this.recursivePortionOfFinalItemSpecDirectory = result.wildcardDirectoryPart;
                    }
                }
            }
            return this.recursivePortionOfFinalItemSpecDirectory;
        }

        internal IDictionary GetAllCustomEvaluatedMetadata()
        {
            ErrorUtilities.VerifyThrow(
                this.evaluatedCustomMetadata != null,
                "Item not initialized properly. evaluatedCustomAttributes is null.");
            return this.evaluatedCustomMetadata;
        }

        internal string GetEvaluatedMetadataEscaped(string metadataName)
        {
            string itemSpecModifier = null;
            if (FileUtilities.IsItemSpecModifier(metadataName))
            {
                itemSpecModifier = this.GetItemSpecModifier(metadataName);
            }
            else
            {
                ErrorUtilities.VerifyThrow(
                    this.evaluatedCustomMetadata != null,
                    "Item not initialized properly.  evaluatedCustomAttributes is null.");
                itemSpecModifier = (string)this.evaluatedCustomMetadata[metadataName];
            }
            if (itemSpecModifier != null)
            {
                return itemSpecModifier;
            }
            return string.Empty;
        }

        internal bool NewItemSpecMatchesExistingWildcard(string newItemSpec)
        {
            /*Project parentProject = this.GetParentProject();
            ErrorUtilities.VerifyThrow(parentProject != null, "This method should only get called on persisted items.");
            BuildPropertyGroup evaluatedProperties = parentProject.evaluatedProperties;
            if ((FileMatcher.HasWildcards(this.Include) && (this.Condition.Length == 0)) && ((this.Exclude.Length == 0) && !FileMatcher.HasWildcards(newItemSpec)))
            {
                Expander expander = new Expander(evaluatedProperties);
                string escapedString = expander.ExpandAllIntoStringLeaveEscaped(newItemSpec, null);
                if (-1 == escapedString.IndexOf(';'))
                {
                    string fileToMatch = EscapingUtilities.UnescapeAll(escapedString);
                    foreach (string str3 in expander.ExpandAllIntoStringListLeaveEscaped(this.Include, this.IncludeAttribute))
                    {
                        bool flag = EscapingUtilities.ContainsEscapedWildcards(str3);
                        if (FileMatcher.HasWildcards(str3) && !flag)
                        {
                            FileMatcher.Result result = FileMatcher.FileMatch(EscapingUtilities.UnescapeAll(str3), fileToMatch);
                            if (result.isLegalFileSpec && result.isMatch)
                            {
                                return true;
                            }
                        }
                    }
                }
            }*/
            return false;
        }

        internal void SetFinalItemSpecEscaped(string finalItemSpecValueEscaped)
        {
            this.finalItemSpecEscaped = finalItemSpecValueEscaped;
            this.itemSpecModifiers = null;
            this.recursivePortionOfFinalItemSpecDirectory = null;
        }

        internal void SplitChildItemIfNecessary()
        {
            if (this.ParentPersistedItem != null)
            {
                this.ParentPersistedItem.SplitItem();
            }
        }

        private string GetItemSpecModifier(string modifier)
        {
            string str = FileUtilities.GetItemSpecModifier(
                this.FinalItemSpecEscaped, modifier, ref this.itemSpecModifiers);
            if ((str.Length == 0) && (string.Compare(modifier, "RecursiveDir", StringComparison.OrdinalIgnoreCase) == 0))
            {
                str = this.ExtractRecursivePortionOfFinalItemSpecDirectory();
                ErrorUtilities.VerifyThrow(
                    this.itemSpecModifiers != null,
                    "The FileUtilities.GetItemSpecModifier() method should have created the cache for the \"{0}\" modifier.",
                    "RecursiveDir");
                this.itemSpecModifiers[modifier] = str;
            }
            return str;
        }

        private void InitializeCustomMetadataCache()
        {
            this.unevaluatedCustomMetadata = new CopyOnWriteHashtable(StringComparer.OrdinalIgnoreCase);
            this.evaluatedCustomMetadata = new CopyOnWriteHashtable(StringComparer.OrdinalIgnoreCase);
        }

        private void InitializeFromItemElement(XmlElement itemElementToParse)
        {
            ErrorUtilities.VerifyThrow(itemElementToParse != null, "Need an XML node representing the item element.");
            int num = XmlUtilities.LocateFirstInvalidElementNameCharacter(itemElementToParse.Name);
            if (-1 != num)
            {
                ProjectErrorUtilities.VerifyThrowInvalidProject(
                    false, itemElementToParse, "NameInvalid", itemElementToParse.Name, itemElementToParse.Name[num]);
            }
            this.itemElement = itemElementToParse;
            this.name = itemElementToParse.Name;
            this.conditionAttribute = null;
            this.includeAttribute = null;
            this.virtualIncludeAttribute = null;
            this.excludeAttribute = null;
            this.itemSpecModifiers = null;
            this.recursivePortionOfFinalItemSpecDirectory = null;
            this.InitializeCustomMetadataCache();
            foreach(XmlAttribute attribute in itemElementToParse.Attributes)
            {
                string name = attribute.Name;
                if (name == null)
                {
                    goto Label_0110;
                }
                if (!(name == "Include"))
                {
                    if (name == "Exclude")
                    {
                        goto Label_00FE;
                    }
                    if (name == "Condition")
                    {
                        goto Label_0107;
                    }
                    goto Label_0110;
                }
                this.includeAttribute = attribute;
                this.evaluatedItemSpecEscaped = attribute.Value;
                this.finalItemSpecEscaped = attribute.Value;
                continue;
                Label_00FE:
                this.excludeAttribute = attribute;
                continue;
                Label_0107:
                this.conditionAttribute = attribute;
                continue;
                Label_0110:
                ProjectErrorUtilities.VerifyThrowInvalidProject(
                    false, attribute, "UnrecognizedAttribute", attribute.Name, itemElementToParse.Name);
            }
            ProjectErrorUtilities.VerifyThrowInvalidProject(
                (this.includeAttribute != null) && (this.includeAttribute.Value.Length != 0),
                itemElementToParse,
                "MissingRequiredAttribute",
                "Include",
                itemElementToParse.Name);
            foreach(XmlNode node in itemElementToParse)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string str = node.Name;
                            num = XmlUtilities.LocateFirstInvalidElementNameCharacter(str);
                            if (-1 != num)
                            {
                                ProjectErrorUtilities.VerifyThrowInvalidProject(
                                    false, node, "NameInvalid", str, str[num]);
                            }
                            ProjectErrorUtilities.VerifyThrowInvalidProject(
                                (node.Prefix.Length == 0) &&
                                (string.Compare(
                                    node.NamespaceURI,
                                    "http://schemas.microsoft.com/developer/msbuild/2003",
                                    StringComparison.OrdinalIgnoreCase) == 0),
                                node,
                                "CustomNamespaceNotAllowedOnThisChildElement",
                                node.Name,
                                itemElementToParse.Name);
                            ProjectErrorUtilities.VerifyThrowInvalidProject(
                                !FileUtilities.IsItemSpecModifier(str),
                                node,
                                "ItemSpecModifierCannotBeCustomAttribute",
                                str);
                            ProjectErrorUtilities.VerifyThrowInvalidProject(
                                XMakeElements.IllegalItemPropertyNames[str] == null,
                                node,
                                "CannotModifyReservedItemMetadata",
                                str);
                            continue;
                        }
                    case XmlNodeType.Comment:
                    case XmlNodeType.Whitespace:
                        {
                            continue;
                        }
                }
                ProjectErrorUtilities.VerifyThrowInvalidProject(
                    false, node, "UnrecognizedChildElement", node.Name, itemElementToParse.Name);
            }
        }

        private void MarkItemAsDirty()
        {
            /*Project parentProject = this.GetParentProject();
            if (parentProject != null)
            {
                parentProject.MarkProjectAsDirty();
            }*/
        }

        private void MarkItemAsDirtyForReevaluation()
        {
            /*Project parentProject = this.GetParentProject();
            if (parentProject != null)
            {
                parentProject.MarkProjectAsDirtyForReevaluation();
            }*/
        }


        private void SetMetadataNoChecks(string metadataName, string metadataValue)
        {
            bool flag = false;
            if (this.ItemElement != null)
            {
                this.SplitChildItemIfNecessary();
                XmlNode oldChild = null;
                XmlNode nextSibling = null;
                for (XmlNode node3 = this.ItemElement.FirstChild; node3 != null; node3 = nextSibling)
                {
                    nextSibling = node3.NextSibling;
                    if (((node3.NodeType != XmlNodeType.Comment) && (node3.NodeType != XmlNodeType.Whitespace)) &&
                        (string.Compare(metadataName, node3.Name, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        if (oldChild != null)
                        {
                            this.ItemElement.RemoveChild(oldChild);
                        }
                        oldChild = node3;
                    }
                }
                if (oldChild == null)
                {
                    oldChild = this.ItemElement.OwnerDocument.CreateElement(
                        metadataName, "http://schemas.microsoft.com/developer/msbuild/2003");
                    this.ItemElement.AppendChild(oldChild);
                    flag = true;
                }
                if (flag ||
                    (string.Compare(
                        Utilities.GetXmlNodeInnerContents(oldChild), metadataValue, StringComparison.Ordinal) != 0))
                {
                    Utilities.SetXmlNodeInnerContents(oldChild, metadataValue);
                    flag = true;
                }
            }
            this.unevaluatedCustomMetadata[metadataName] = metadataValue;
            this.evaluatedCustomMetadata[metadataName] = metadataValue;
            if (flag)
            {
                this.MarkItemAsDirty();
            }
        }

        private void SplitItem()
        {
            BuildItemGroup parentPersistedItemGroup = this.ParentPersistedItemGroup;
            ErrorUtilities.VerifyThrow(
                parentPersistedItemGroup != null, "No parent BuildItemGroup for item to be removed.");
            if (this.ChildItems.Count > 1)
            {
                foreach(BuildItem item in this.ChildItems)
                {
                    ErrorUtilities.VerifyThrow(item.ItemElement != null, "This is not a persisted item!");
                    item.ParentPersistedItem = null;
                    BuildItem item2 = parentPersistedItemGroup.AddNewItem(item.Name, item.FinalItemSpecEscaped);
                    if (item.Condition.Length > 0)
                    {
                        item2.Condition = item.Condition;
                    }
                    foreach(XmlNode node in item.ItemElement)
                    {
                        item2.ItemElement.AppendChild(node.Clone());
                    }
                    item.InitializeFromItemElement(item2.ItemElement);
                    item.ParentPersistedItem = item2;
                    item2.ChildItems.AddItem(item);
                }
                parentPersistedItemGroup.RemoveItem(this);
            }
        }
    }


    internal interface IItemPropertyGrouping
    {
    }
}