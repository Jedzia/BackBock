namespace Jedzia.BackBock.Tasks.BuildEngine
{
    using System;
    using System.Collections;
    using Jedzia.BackBock.Tasks.Shared;

    internal class GroupingCollection : IEnumerable
    {
        // Fields
        private int chooseCount;
        private ArrayList combinedGroupList = new ArrayList();
        private int itemGroupCount;
        private GroupingCollection parentGroupingCollection;
        private int propertyGroupCount;

        // Methods
        internal GroupingCollection(GroupingCollection parentGroupingCollection)
        {
            this.parentGroupingCollection = parentGroupingCollection;
        }

        internal void ChangeItemGroupCount(int delta)
        {
            this.itemGroupCount += delta;
            ErrorUtilities.VerifyThrow(this.itemGroupCount >= 0, "The item group count should never be negative");
            if (this.parentGroupingCollection != null)
            {
                this.parentGroupingCollection.ChangeItemGroupCount(delta);
            }
        }

        internal void ChangePropertyGroupCount(int delta)
        {
            this.propertyGroupCount += delta;
            ErrorUtilities.VerifyThrow(this.propertyGroupCount >= 0, "The property group count should never be negative");
            if (this.parentGroupingCollection != null)
            {
                this.parentGroupingCollection.ChangePropertyGroupCount(delta);
            }
        }

        internal void Clear()
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Clear();
            this.ChangeItemGroupCount(-this.itemGroupCount);
            this.ChangePropertyGroupCount(-this.propertyGroupCount);
            this.chooseCount = 0;
        }

        public IEnumerator GetEnumerator()
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            return this.combinedGroupList.GetEnumerator();
        }

        internal IEnumerator GetItemEnumerator()
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.ItemGroupsAll).GetEnumerator();
        }

        internal IEnumerator GetPropertyEnumerator()
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.PropertyGroupsAll).GetEnumerator();
        }

        internal void InsertAfter(BuildItemGroup newItemGroup, BuildItemGroup insertionPoint)
        {
            this.InsertAfter((IItemPropertyGrouping) newItemGroup, (IItemPropertyGrouping) insertionPoint);
        }

        internal void InsertAfter(BuildPropertyGroup newPropertyGroup, BuildPropertyGroup insertionPoint)
        {
            this.InsertAfter((IItemPropertyGrouping) newPropertyGroup, (IItemPropertyGrouping) insertionPoint);
        }

        internal void InsertAfter(IItemPropertyGrouping newGroup, IItemPropertyGrouping insertionPoint)
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Insert(this.combinedGroupList.IndexOf(insertionPoint) + 1, newGroup);
            if (newGroup is BuildItemGroup)
            {
                ((BuildItemGroup) newGroup).ParentCollection = this;
                this.ChangeItemGroupCount(1);
            }
            else if (newGroup is BuildPropertyGroup)
            {
                ((BuildPropertyGroup) newGroup).ParentCollection = this;
                this.ChangePropertyGroupCount(1);
            }
            else if (newGroup is Choose)
            {
                this.chooseCount++;
            }
        }

        internal void InsertAtBeginning(BuildPropertyGroup newPropertyGroup)
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Insert(0, newPropertyGroup);
            newPropertyGroup.ParentCollection = this;
            this.ChangePropertyGroupCount(1);
        }

        internal void InsertAtEnd(BuildItemGroup newItemGroup)
        {
            this.InsertAtEnd((IItemPropertyGrouping) newItemGroup);
        }

        internal void InsertAtEnd(BuildPropertyGroup newPropertyGroup)
        {
            this.InsertAtEnd((IItemPropertyGrouping) newPropertyGroup);
        }

        internal void InsertAtEnd(IItemPropertyGrouping newGroup)
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Add(newGroup);
            if (newGroup is BuildItemGroup)
            {
                ((BuildItemGroup) newGroup).ParentCollection = this;
                this.ChangeItemGroupCount(1);
            }
            else if (newGroup is BuildPropertyGroup)
            {
                ((BuildPropertyGroup) newGroup).ParentCollection = this;
                this.ChangePropertyGroupCount(1);
            }
            else if (newGroup is Choose)
            {
                this.chooseCount++;
            }
        }

        internal void ItemCopyTo(Array array, int index)
        {
            foreach (BuildItemGroup group in this.ItemGroupsAll)
            {
                array.SetValue(group, index++);
            }
        }

        internal void PropertyCopyTo(Array array, int index)
        {
            foreach (BuildPropertyGroup group in this.PropertyGroupsAll)
            {
                array.SetValue(group, index++);
            }
        }

        internal void RemoveAllItemGroups()
        {
            ArrayList list = new ArrayList(this.itemGroupCount);
            foreach (BuildItemGroup group in this.ItemGroupsAll)
            {
                if (!group.IsImported)
                {
                    list.Add(group);
                }
            }
            foreach (BuildItemGroup group2 in list)
            {
                group2.ParentProject.RemoveItemGroup(group2);
            }
        }

        internal void RemoveAllItemGroupsByCondition(string condition)
        {
            ArrayList list = new ArrayList(this.itemGroupCount);
            foreach (BuildItemGroup group in this.ItemGroupsAll)
            {
                if ((string.Compare(condition.Trim(), group.Condition.Trim(), StringComparison.OrdinalIgnoreCase) == 0) && !group.IsImported)
                {
                    list.Add(group);
                }
            }
            foreach (BuildItemGroup group2 in list)
            {
                group2.ParentProject.RemoveItemGroup(group2);
            }
        }

        internal void RemoveAllPropertyGroups()
        {
            ArrayList list = new ArrayList(this.propertyGroupCount);
            foreach (BuildPropertyGroup group in this.PropertyGroupsAll)
            {
                if (!group.IsImported)
                {
                    list.Add(group);
                }
            }
            foreach (BuildPropertyGroup group2 in list)
            {
                group2.ParentProject.RemovePropertyGroup(group2);
            }
        }

        internal void RemoveAllPropertyGroupsByCondition(string condition)
        {
            ArrayList list = new ArrayList();
            foreach (BuildPropertyGroup group in this.PropertyGroupsAll)
            {
                if ((string.Compare(condition.Trim(), group.Condition.Trim(), StringComparison.OrdinalIgnoreCase) == 0) && !group.IsImported)
                {
                    list.Add(group);
                }
            }
            foreach (BuildPropertyGroup group2 in list)
            {
                group2.ParentProject.RemovePropertyGroup(group2);
            }
        }

        internal void RemoveChoose(Choose choose)
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Remove(choose);
            this.chooseCount--;
            ErrorUtilities.VerifyThrow(this.chooseCount >= 0, "Too many calls to RemoveChoose().");
        }

        internal void RemoveItemGroup(BuildItemGroup itemGroupToRemove)
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Remove(itemGroupToRemove);
            itemGroupToRemove.ParentCollection = null;
            this.ChangeItemGroupCount(-1);
            ErrorUtilities.VerifyThrow(this.itemGroupCount >= 0, "Too many calls to RemoveItemGroup().");
        }

        internal void RemoveItemsByName(string itemName)
        {
            BuildItemGroup group = new BuildItemGroup();
            foreach (BuildItemGroup group2 in this.ItemGroupsAll)
            {
                foreach (BuildItem item in group2)
                {
                    if ((string.Compare(item.Name, itemName, StringComparison.OrdinalIgnoreCase) == 0) && !item.IsImported)
                    {
                        group.AddItem(item);
                    }
                }
            }
            foreach (BuildItem item2 in group)
            {
                item2.ParentPersistedItemGroup.ParentProject.RemoveItem(item2);
            }
        }

        internal void RemovePropertyGroup(BuildPropertyGroup propertyGroup)
        {
            ErrorUtilities.VerifyThrow(this.combinedGroupList != null, "Arraylist not initialized!");
            this.combinedGroupList.Remove(propertyGroup);
            this.ChangePropertyGroupCount(-1);
            propertyGroup.ParentCollection = null;
            ErrorUtilities.VerifyThrow(this.propertyGroupCount >= 0, "Too many calls to RemovePropertyGroup().");
        }

        // Properties
        internal GroupEnumeratorHelper ChoosesTopLevel
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.ChoosesTopLevel);
            }
        }

        internal bool IsSynchronized
        {
            get
            {
                return this.combinedGroupList.IsSynchronized;
            }
        }

        internal int ItemGroupCount
        {
            get
            {
                return this.itemGroupCount;
            }
        }

        internal GroupEnumeratorHelper ItemGroupsAll
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.ItemGroupsAll);
            }
        }

        internal GroupEnumeratorHelper ItemGroupsTopLevel
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.ItemGroupsTopLevel);
            }
        }

        internal GroupEnumeratorHelper ItemGroupsTopLevelAndChooses
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.ItemGroupsTopLevelAndChoose);
            }
        }

        internal int PropertyGroupCount
        {
            get
            {
                return this.propertyGroupCount;
            }
        }

        internal GroupEnumeratorHelper PropertyGroupsAll
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.PropertyGroupsAll);
            }
        }

        internal GroupEnumeratorHelper PropertyGroupsTopLevel
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.PropertyGroupsTopLevel);
            }
        }

        internal GroupEnumeratorHelper PropertyGroupsTopLevelAndChooses
        {
            get
            {
                return new GroupEnumeratorHelper(this, GroupEnumeratorHelper.ListType.PropertyGroupsTopLevelAndChoose);
            }
        }

        internal object SyncRoot
        {
            get
            {
                return this.combinedGroupList.SyncRoot;
            }
        }
    }
}