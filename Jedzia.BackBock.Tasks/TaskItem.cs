namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.ComponentModel;
    using System.Xml;

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
}