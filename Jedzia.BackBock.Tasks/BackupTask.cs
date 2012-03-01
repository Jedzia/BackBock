using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Xml;
using System.ComponentModel;

namespace Jedzia.BackBock.Tasks
{
    [DebuggerDisplay("BuildItem (Name = { Name }, Include = { Include }, FinalItemSpec = { FinalItemSpec }, Condition = { Condition })")]
    [DisplayName("BuildItem")]
    public class BuildItem
    {
        private string finalItemSpecEscaped;
        private Hashtable itemSpecModifiers;
        private string recursivePortionOfFinalItemSpecDirectory;

        public BuildItem(string itemName, string itemInclude)
            : this(null, itemName, itemInclude)
        {
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

    [DisplayName("TaskItem")]
    public /*internal*/ sealed class TaskItem : MarshalByRefObject, ITaskItem
    {
        private XmlElement mainProjectElement;
        private XmlDocument mainProjectEntireContents;

        public TaskItem()
        {
            // Todo: after XamlReader testing, make this internal.
            //ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");

            this.mainProjectEntireContents = new XmlDocument();
            this.mainProjectElement = this.mainProjectEntireContents.CreateElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
            this.mainProjectEntireContents.AppendChild(this.mainProjectElement);


            this.item = new BuildItem(null, string.Empty);
        }

        internal TaskItem(string itemSpec)
        {
            //ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");
            this.item = new BuildItem(null, itemSpec);
        }

 
        // Fields
        private BuildItem item;

        #region ITaskItem Members

        // Properties
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

        public System.Collections.ICollection MetadataNames
        {
            get { throw new NotImplementedException(); }
        }

        public int MetadataCount
        {
            get { throw new NotImplementedException(); }
        }

        public string GetMetadata(string metadataName)
        {
            throw new NotImplementedException();
        }

        public void SetMetadata(string metadataName, string metadataValue)
        {
            throw new NotImplementedException();
        }

        public void RemoveMetadata(string metadataName)
        {
            throw new NotImplementedException();
        }

        public void CopyMetadataTo(ITaskItem destinationItem)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IDictionary CloneCustomMetadata()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [DisplayName("BackupTask")]
    public class BackupTask : ITask
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BackupTask"/> class.
        /// </summary>
        public BackupTask()
        {
            Files = new TaskItem[5];
            //FilesXX = new List<object>();
            for (int index = 0; index < 5; index++)
            {
                var taskItem = new TaskItem(@"C:\tmp\My0" + index.ToString() + ".txt");
                //taskItem.ItemSpec = @"C:\tmp\txt";
                Files[index] = taskItem;
                //FilesXX.Add(taskItem);
            }
        }

        // Properties
        public bool AlwaysCreate { get; set; }
        //[Required]
        public ITaskItem[] Files { get; set; }
        //public List<object> FilesXX { get; set; }
        public bool ForceTouch { get; set; }
        public string Time { get; set; }
        //[Output]
        public ITaskItem[] TouchedFiles { get; set; }


        #region ITask Members

        public string Name
        {
            get { return "Backup"; }
        }

        public bool Execute()
        {
            return true;
        }
        #endregion
    }
}
