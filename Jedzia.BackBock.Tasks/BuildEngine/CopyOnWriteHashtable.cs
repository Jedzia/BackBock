namespace Jedzia.BackBock.Tasks.BuildEngine
{
    using System;
    using System.Collections;
    using Jedzia.BackBock.Tasks.Shared;

    internal sealed class CopyOnWriteHashtable : IDictionary, ICollection, IEnumerable, ICloneable
    {
        // Fields
        private Hashtable readonlyData;
        private StringComparer stringComparer;
        private Hashtable writeableData;

        // Methods
        private CopyOnWriteHashtable(CopyOnWriteHashtable that)
        {
            this.ConstructFrom(that);
        }

        internal CopyOnWriteHashtable(StringComparer stringComparer)
        {
            ErrorUtilities.VerifyThrowArgumentNull(stringComparer, "stringComparer");
            this.writeableData = new Hashtable(stringComparer);
            this.readonlyData = null;
            this.stringComparer = stringComparer;
        }

        internal CopyOnWriteHashtable(IDictionary dictionary, StringComparer stringComparer)
        {
            ErrorUtilities.VerifyThrowArgumentNull(dictionary, "dictionary");
            ErrorUtilities.VerifyThrowArgumentNull(stringComparer, "stringComparer");
            if (dictionary is CopyOnWriteHashtable)
            {
                CopyOnWriteHashtable that = (CopyOnWriteHashtable) dictionary;
                if (that.stringComparer != stringComparer)
                {
                    throw new NotImplementedException("Bug: Changing the case-sensitiveness of a copied hash-table.");
                    //throw new InternalErrorException("Bug: Changing the case-sensitiveness of a copied hash-table.", false);
                }
                this.ConstructFrom(that);
            }
            else
            {
                this.writeableData = new Hashtable(dictionary, stringComparer);
                this.readonlyData = null;
                this.stringComparer = stringComparer;
            }
        }

        public void Add(object key, object value)
        {
            this.WriteOperation.Add(key, value);
        }

        public void Clear()
        {
            ErrorUtilities.VerifyThrow(this.stringComparer != null, "Should have a valid string comparer.");
            this.writeableData = new Hashtable(this.stringComparer);
            this.readonlyData = null;
        }

        public object Clone()
        {
            return new CopyOnWriteHashtable(this);
        }

        private void ConstructFrom(CopyOnWriteHashtable that)
        {
            this.writeableData = null;
            if (that.writeableData != null)
            {
                that.readonlyData = that.writeableData;
                that.writeableData = null;
            }
            this.readonlyData = that.readonlyData;
            this.stringComparer = that.stringComparer;
        }

        public bool Contains(object key)
        {
            return this.ReadOperation.Contains(key);
        }

        public bool ContainsKey(object key)
        {
            return this.ReadOperation.Contains(key);
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            this.ReadOperation.CopyTo(array, arrayIndex);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return this.ReadOperation.GetEnumerator();
        }

        public void Remove(object key)
        {
            this.WriteOperation.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this.ReadOperation).GetEnumerator();
        }

        // Properties
        public int Count
        {
            get
            {
                return this.ReadOperation.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return this.ReadOperation.IsFixedSize;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.ReadOperation.IsFixedSize;
            }
        }

        internal bool IsShallowCopy
        {
            get
            {
                return (this.readonlyData != null);
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this.ReadOperation.IsSynchronized;
            }
        }

        public object this[object key]
        {
            get
            {
                return this.ReadOperation[key];
            }
            set
            {
                if (this.writeableData != null)
                {
                    this.writeableData[key] = value;
                }
                else if ((this.readonlyData[key] != value) || !this.readonlyData.ContainsKey(key))
                {
                    this.WriteOperation[key] = value;
                }
            }
        }

        public ICollection Keys
        {
            get
            {
                return this.ReadOperation.Keys;
            }
        }

        private Hashtable ReadOperation
        {
            get
            {
                if (this.readonlyData != null)
                {
                    return this.readonlyData;
                }
                return this.writeableData;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.ReadOperation.SyncRoot;
            }
        }

        public ICollection Values
        {
            get
            {
                return this.ReadOperation.Values;
            }
        }

        private Hashtable WriteOperation
        {
            get
            {
                if (this.writeableData == null)
                {
                    this.writeableData = (Hashtable) this.readonlyData.Clone();
                    this.readonlyData = null;
                }
                return this.writeableData;
            }
        }
    }
}