namespace Jedzia.BackBock.Tasks
{
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml;

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
}