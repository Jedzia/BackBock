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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml;
    using Jedzia.BackBock.Tasks.Shared;

    [DebuggerDisplay("BuildItemGroup (Count = { Count }, Condition = { Condition })")]
    public class BuildItemGroup : List<BuildItem>, IItemPropertyGrouping, IEnumerable
    {
        #region Fields

        private XmlAttribute conditionAttribute;
        private bool importedFromAnotherProject;
        private XmlElement itemGroupElement;
        private ArrayList items;
        private XmlDocument ownerDocument;

        #endregion

        public BuildItem AddNewItem(string itemName, string itemInclude)
        {
            BuildItem item;
            if (this.IsPersisted())
            {
                item = new BuildItem(this.ownerDocument, itemName, itemInclude);
            }
            else
            {
                item = new BuildItem(itemName, itemInclude);
            }
            Add(item);
            return item;
        }

        public void RemoveItem(BuildItem itemToRemove)
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup object not initialized.");
            this.RemoveItemElement(itemToRemove);
            if (this.IsPersisted())
            {
                ErrorUtilities.VerifyThrow(
                    itemToRemove.ParentPersistedItemGroup == this, "This item doesn't belong to this itemgroup.");
                itemToRemove.ParentPersistedItemGroup = null;
            }
            this.items.Remove(itemToRemove);
            this.MarkItemGroupAsDirty();
        }

        internal void AddExistingItem(BuildItem itemToAdd)
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup has not been initialized.");
            this.items.Add(itemToAdd);
            if (this.IsPersisted())
            {
                itemToAdd.ParentPersistedItemGroup = this;
            }
            this.MarkItemGroupAsDirty();
        }

        internal void AddExistingItemAt(int index, BuildItem itemToAdd)
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup has not been initialized.");
            ErrorUtilities.VerifyThrow(index <= this.items.Count, "Index out of range");
            this.items.Insert(index, itemToAdd);
            if (this.IsPersisted())
            {
                itemToAdd.ParentPersistedItemGroup = this;
            }
            this.MarkItemGroupAsDirty();
        }

        internal void AddItem(BuildItem itemToAdd)
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup has not been initialized.");
            if (!this.IsPersisted())
            {
                this.AddExistingItem(itemToAdd);
            }
            else
            {
                this.MustBePersisted("InvalidInVirtualItemGroup");
                ErrorUtilities.VerifyThrowInvalidOperation(
                    !this.importedFromAnotherProject, "CannotModifyImportedProjects");
                ErrorUtilities.VerifyThrow(itemToAdd.ItemElement != null, "Item does not have an XML element");
                ErrorUtilities.VerifyThrow(
                    itemToAdd.ItemElement.OwnerDocument == this.ownerDocument,
                    "Cannot add an Item with a different XML owner document.");
                int count = this.items.Count;
                for (int i = 0; i < this.items.Count; i++)
                {
                    if (
                        string.Compare(
                            itemToAdd.Name, ((BuildItem)this.items[i]).Name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        count = i + 1;
                        if (0 >
                            string.Compare(
                                itemToAdd.Include,
                                ((BuildItem)this.items[i]).Include,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            count = i;
                            break;
                        }
                    }
                }
                if (this.items.Count > 0)
                {
                    if (count == this.items.Count)
                    {
                        XmlElement itemElement = ((BuildItem)this.items[this.items.Count - 1]).ItemElement;
                        itemElement.ParentNode.InsertAfter(itemToAdd.ItemElement, itemElement);
                    }
                    else
                    {
                        XmlElement refChild = ((BuildItem)this.items[count]).ItemElement;
                        refChild.ParentNode.InsertBefore(itemToAdd.ItemElement, refChild);
                    }
                    this.AddExistingItemAt(count, itemToAdd);
                }
                else
                {
                    this.itemGroupElement.AppendChild(itemToAdd.ItemElement);
                    this.AddExistingItem(itemToAdd);
                }
            }
        }

        private bool IsPersisted()
        {
            return (this.itemGroupElement != null);
        }

        private bool IsVirtual()
        {
            return (this.itemGroupElement == null);
        }

        private void MarkItemGroupAsDirty()
        {
            /*if (this.ParentProject != null)
            {
                this.ParentProject.MarkProjectAsDirty();
            }*/
        }

        private void MustBePersisted(string errorResourceName)
        {
            this.MustBePersisted(errorResourceName, null);
        }

        private void MustBePersisted(string errorResourceName, params object[] args)
        {
            ErrorUtilities.VerifyThrowInvalidOperation(this.IsPersisted(), errorResourceName, args);
            ErrorUtilities.VerifyThrow(
                this.ownerDocument != null,
                "There must be an owner document. It should have been set in the constructor.");
        }

        private void MustBeVirtual(string errorResourceName)
        {
            this.MustBeVirtual(errorResourceName, null);
        }

        private void MustBeVirtual(string errorResourceName, params object[] args)
        {
            ErrorUtilities.VerifyThrowInvalidOperation(this.IsVirtual(), errorResourceName, args);
            ErrorUtilities.VerifyThrow(
                this.conditionAttribute == null, "Expected condition attribute to be null for virtual group.");
            ErrorUtilities.VerifyThrow(
                this.itemGroupElement == null, "Expected item group element to be null for virtual group.");
        }

        private void RemoveItemElement(BuildItem itemToRemove)
        {
            if (this.IsPersisted())
            {
                this.MustBePersisted("InvalidInVirtualItemGroup");
                ErrorUtilities.VerifyThrowInvalidOperation(
                    !this.importedFromAnotherProject, "CannotModifyImportedProjects");
                XmlElement itemElement = itemToRemove.ItemElement;
                ErrorUtilities.VerifyThrowInvalidOperation(itemElement != null, "ItemDoesNotBelongToItemGroup");
                ErrorUtilities.VerifyThrowInvalidOperation(
                    itemElement.ParentNode == this.itemGroupElement, "ItemDoesNotBelongToItemGroup");
                itemElement.ParentNode.RemoveChild(itemElement);
            }
        }

        /* // Fields
        private GroupingCollection parentCollection;
        private Project parentProject;

        // Methods
        public BuildItemGroup()
        {
            this.itemGroupElement = null;
            this.importedFromAnotherProject = false;
            this.conditionAttribute = null;
            this.items = new ArrayList();
        }

        internal BuildItemGroup(XmlDocument ownerDocument, bool importedFromAnotherProject)
        {
            ErrorUtilities.VerifyThrow(ownerDocument != null, "Need valid XmlDocument owner for this item group.");
            this.itemGroupElement = ownerDocument.CreateElement("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003");
            this.importedFromAnotherProject = importedFromAnotherProject;
            this.ownerDocument = ownerDocument;
            this.conditionAttribute = null;
            this.items = new ArrayList();
            this.MustBePersisted("InvalidInVirtualItemGroup");
        }

        internal BuildItemGroup(XmlElement itemGroupElement, bool importedFromAnotherProject)
        {
            ErrorUtilities.VerifyThrow(itemGroupElement != null, "Need a valid XML node.");
            ErrorUtilities.VerifyThrow(itemGroupElement.Name == "ItemGroup", "Expected <{0}> element; received <{1}> element.", "ItemGroup", itemGroupElement.Name);
            this.itemGroupElement = itemGroupElement;
            this.importedFromAnotherProject = importedFromAnotherProject;
            this.conditionAttribute = null;
            this.ownerDocument = itemGroupElement.OwnerDocument;
            this.items = new ArrayList();
            foreach (XmlAttribute attribute in itemGroupElement.Attributes)
            {
                string str;
                if (((str = attribute.Name) != null) && (str == "Condition"))
                {
                    this.conditionAttribute = attribute;
                }
                else
                {
                    ProjectErrorUtilities.VerifyThrowInvalidProject(false, attribute, "UnrecognizedAttribute", attribute.Name, itemGroupElement.Name);
                }
            }
            foreach (XmlNode node in itemGroupElement)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            ProjectErrorUtilities.VerifyThrowInvalidProject((node.Prefix.Length == 0) && (string.Compare(node.NamespaceURI, "http://schemas.microsoft.com/developer/msbuild/2003", StringComparison.OrdinalIgnoreCase) == 0), node, "CustomNamespaceNotAllowedOnThisChildElement", node.Name, itemGroupElement.Name);
                            this.AddExistingItem(new BuildItem((XmlElement) node, importedFromAnotherProject));
                            continue;
                        }
                    case XmlNodeType.Comment:
                    case XmlNodeType.Whitespace:
                        {
                            continue;
                        }
                }
                ProjectErrorUtilities.VerifyThrowInvalidProject(false, node, "UnrecognizedChildElement", node.Name, itemGroupElement.Name);
            }
            this.MustBePersisted("InvalidInVirtualItemGroup");
        }

        internal void AddItem(BuildItem itemToAdd)
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup has not been initialized.");
            if (!this.IsPersisted())
            {
                this.AddExistingItem(itemToAdd);
            }
            else
            {
                this.MustBePersisted("InvalidInVirtualItemGroup");
                ErrorUtilities.VerifyThrowInvalidOperation(!this.importedFromAnotherProject, "CannotModifyImportedProjects");
                ErrorUtilities.VerifyThrow(itemToAdd.ItemElement != null, "Item does not have an XML element");
                ErrorUtilities.VerifyThrow(itemToAdd.ItemElement.OwnerDocument == this.ownerDocument, "Cannot add an Item with a different XML owner document.");
                int count = this.items.Count;
                for (int i = 0; i < this.items.Count; i++)
                {
                    if (string.Compare(itemToAdd.Name, ((BuildItem) this.items[i]).Name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        count = i + 1;
                        if (0 > string.Compare(itemToAdd.Include, ((BuildItem) this.items[i]).Include, StringComparison.OrdinalIgnoreCase))
                        {
                            count = i;
                            break;
                        }
                    }
                }
                if (this.items.Count > 0)
                {
                    if (count == this.items.Count)
                    {
                        XmlElement itemElement = ((BuildItem) this.items[this.items.Count - 1]).ItemElement;
                        itemElement.ParentNode.InsertAfter(itemToAdd.ItemElement, itemElement);
                    }
                    else
                    {
                        XmlElement refChild = ((BuildItem) this.items[count]).ItemElement;
                        refChild.ParentNode.InsertBefore(itemToAdd.ItemElement, refChild);
                    }
                    this.AddExistingItemAt(count, itemToAdd);
                }
                else
                {
                    this.itemGroupElement.AppendChild(itemToAdd.ItemElement);
                    this.AddExistingItem(itemToAdd);
                }
            }
        }

        public BuildItem AddNewItem(string itemName, string itemInclude)
        {
            BuildItem item;
            if (this.IsPersisted())
            {
                item = new BuildItem(this.ownerDocument, itemName, itemInclude);
            }
            else
            {
                item = new BuildItem(itemName, itemInclude);
            }
            this.AddItem(item);
            return item;
        }

        public BuildItem AddNewItem(string itemName, string itemInclude, bool treatItemIncludeAsLiteral)
        {
            return this.AddNewItem(itemName, treatItemIncludeAsLiteral ? EscapingUtilities.Escape(itemInclude) : itemInclude);
        }

        public void Clear()
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup object not initialized.");
            if (this.IsPersisted())
            {
                this.MustBePersisted("InvalidInVirtualItemGroup");
                ErrorUtilities.VerifyThrowInvalidOperation(!this.importedFromAnotherProject, "CannotModifyImportedProjects");
                foreach (BuildItem item in this.items)
                {
                    XmlElement itemElement = item.ItemElement;
                    ErrorUtilities.VerifyThrowInvalidOperation(itemElement != null, "ItemDoesNotBelongToItemGroup");
                    ErrorUtilities.VerifyThrowInvalidOperation(itemElement.ParentNode == this.itemGroupElement, "ItemDoesNotBelongToItemGroup");
                    itemElement.ParentNode.RemoveChild(itemElement);
                    item.ParentPersistedItemGroup = null;
                }
                this.conditionAttribute = null;
            }
            this.items.Clear();
            this.MarkItemGroupAsDirty();
        }

        public BuildItemGroup Clone(bool deepClone)
        {
            BuildItemGroup group;
            if (this.IsPersisted())
            {
                this.MustBePersisted("InvalidInVirtualItemGroup");
                ErrorUtilities.VerifyThrowInvalidOperation(deepClone, "ShallowCloneNotAllowed");
                group = new BuildItemGroup(this.ownerDocument, this.importedFromAnotherProject) {
                                                                                                    Condition = this.Condition
                                                                                                };
            }
            else
            {
                this.MustBeVirtual("InvalidInPersistedItemGroup");
                group = new BuildItemGroup();
            }
            foreach (BuildItem item in this)
            {
                group.AddItem(deepClone ? item.Clone() : item);
            }
            return group;
        }

        internal void Evaluate(BuildPropertyGroup parentPropertyBag, bool ignoreCondition, bool honorCondition, Hashtable conditionedPropertiesTable, ProcessingPass pass)
        {
            ErrorUtilities.VerifyThrow(pass == ProcessingPass.Pass2, "Pass should be Pass2 for ItemGroups.");
            this.parentProject.EvaluateItemGroup(this, parentPropertyBag, ignoreCondition, honorCondition);
        }

        public IEnumerator GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        internal void ImportItems(BuildItemGroup itemsToImport)
        {
            ErrorUtilities.VerifyThrow(itemsToImport != null, "Null BuildItemGroup passed in.");
            foreach (BuildItem item in itemsToImport)
            {
                this.AddItem(item);
            }
        }


        internal void RemoveAllIntermediateItems()
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup object not initialized.");
            this.MustBeVirtual("InvalidInPersistedItemGroup");
            ArrayList list = new ArrayList(this.items.Count);
            for (int i = 0; i < this.items.Count; i++)
            {
                if (((BuildItem) this.items[i]).ItemElement != null)
                {
                    list.Add(this.items[i]);
                }
            }
            this.items = list;
            this.MarkItemGroupAsDirty();
        }

        public void RemoveItemAt(int index)
        {
            ErrorUtilities.VerifyThrow(this.items != null, "BuildItemGroup object not initialized.");
            this.RemoveItemElement((BuildItem) this.items[index]);
            if (this.IsPersisted())
            {
                ErrorUtilities.VerifyThrow(((BuildItem) this.items[index]).ParentPersistedItemGroup == this, "This item doesn't belong to this itemgroup.");
                ((BuildItem) this.items[index]).ParentPersistedItemGroup = null;
            }
            this.items.RemoveAt(index);
            this.MarkItemGroupAsDirty();
        }

        public BuildItem[] ToArray()
        {
            return (BuildItem[]) this.items.ToArray(typeof(BuildItem));
        }

        // Properties
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
                this.MustBePersisted("CannotSetCondition");
                ErrorUtilities.VerifyThrowInvalidOperation(!this.importedFromAnotherProject, "CannotModifyImportedProjects");
                if ((value == null) || (value.Length == 0))
                {
                    this.itemGroupElement.RemoveAttribute("Condition");
                    this.conditionAttribute = null;
                }
                else
                {
                    this.itemGroupElement.SetAttribute("Condition", value);
                    this.conditionAttribute = this.itemGroupElement.Attributes["Condition"];
                }
                this.MarkItemGroupAsDirty();
            }
        }

        internal XmlAttribute ConditionAttribute
        {
            get
            {
                return this.conditionAttribute;
            }
        }

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        public bool IsImported
        {
            get
            {
                return this.importedFromAnotherProject;
            }
        }

        public BuildItem this[int index]
        {
            get
            {
                return (BuildItem) this.items[index];
            }
        }

        internal XmlElement ItemGroupElement
        {
            get
            {
                return this.itemGroupElement;
            }
        }

        internal GroupingCollection ParentCollection
        {
            get
            {
                return this.parentCollection;
            }
            set
            {
                this.parentCollection = value;
            }
        }

        internal XmlElement ParentElement
        {
            get
            {
                if ((this.itemGroupElement != null) && (this.itemGroupElement.ParentNode is XmlElement))
                {
                    return (XmlElement) this.itemGroupElement.ParentNode;
                }
                return null;
            }
        }

        internal Project ParentProject
        {
            get
            {
                return this.parentProject;
            }
            set
            {
                ErrorUtilities.VerifyThrow(((value == null) && (this.parentProject != null)) || ((value != null) && (this.parentProject == null)), "Either new parent cannot be assigned because we already have a parent, or old parent cannot be removed because none exists.");
                this.parentProject = value;
            }
        }*/
    }
}