namespace Jedzia.BackBock.Tasks.Shared
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Xml;
    using Jedzia.BackBock.Tasks.BuildEngine;

    [Serializable]
    public sealed class InvalidProjectFileException : Exception
    {
        // Fields
        private int columnNumber;
        private int endColumnNumber;
        private int endLineNumber;
        private string errorCode;
        private string errorSubcategory;
        private bool hasBeenLogged;
        private string helpKeyword;
        private int lineNumber;
        private string projectFile;

        // Methods
        public InvalidProjectFileException()
        {
        }

        public InvalidProjectFileException(string message) : base(message)
        {
        }

        private InvalidProjectFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.projectFile = info.GetString("projectFile");
            this.lineNumber = info.GetInt32("lineNumber");
            this.columnNumber = info.GetInt32("columnNumber");
            this.endLineNumber = info.GetInt32("endLineNumber");
            this.endColumnNumber = info.GetInt32("endColumnNumber");
            this.errorSubcategory = info.GetString("errorSubcategory");
            this.errorCode = info.GetString("errorCode");
            this.helpKeyword = info.GetString("helpKeyword");
            this.hasBeenLogged = info.GetBoolean("hasBeenLogged");
        }

        public InvalidProjectFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidProjectFileException(XmlNode xmlNode, string message, string errorSubcategory, string errorCode, string helpKeyword) : base(message)
        {
            ErrorUtilities.VerifyThrowArgumentLength(message, "message");
            if (xmlNode != null)
            {
                this.projectFile = XmlUtilities.GetXmlNodeFile(xmlNode, string.Empty);
                XmlSearcher.GetLineColumnByNode(xmlNode, out this.lineNumber, out this.columnNumber);
            }
            this.errorSubcategory = errorSubcategory;
            this.errorCode = errorCode;
            this.helpKeyword = helpKeyword;
        }

        public InvalidProjectFileException(string projectFile, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, string errorSubcategory, string errorCode, string helpKeyword) : base(message)
        {
            ErrorUtilities.VerifyThrowArgumentNull(projectFile, "projectFile");
            ErrorUtilities.VerifyThrowArgumentLength(message, "message");
            this.projectFile = projectFile;
            this.lineNumber = lineNumber;
            this.columnNumber = columnNumber;
            this.endLineNumber = endLineNumber;
            this.endColumnNumber = endColumnNumber;
            this.errorSubcategory = errorSubcategory;
            this.errorCode = errorCode;
            this.helpKeyword = helpKeyword;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("projectFile", this.projectFile);
            info.AddValue("lineNumber", this.lineNumber);
            info.AddValue("columnNumber", this.columnNumber);
            info.AddValue("endLineNumber", this.endLineNumber);
            info.AddValue("endColumnNumber", this.endColumnNumber);
            info.AddValue("errorSubcategory", this.errorSubcategory);
            info.AddValue("errorCode", this.errorCode);
            info.AddValue("helpKeyword", this.helpKeyword);
            info.AddValue("hasBeenLogged", this.hasBeenLogged);
        }

        // Properties
        public string BaseMessage
        {
            get
            {
                return base.Message;
            }
        }

        public int ColumnNumber
        {
            get
            {
                return this.columnNumber;
            }
        }

        public int EndColumnNumber
        {
            get
            {
                return this.endColumnNumber;
            }
        }

        public int EndLineNumber
        {
            get
            {
                return this.endLineNumber;
            }
        }

        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        public string ErrorSubcategory
        {
            get
            {
                return this.errorSubcategory;
            }
        }

        internal bool HasBeenLogged
        {
            get
            {
                return this.hasBeenLogged;
            }
            set
            {
                this.hasBeenLogged = value;
            }
        }

        public string HelpKeyword
        {
            get
            {
                return this.helpKeyword;
            }
        }

        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        public override string Message
        {
            get
            {
                return (base.Message + ((this.ProjectFile != null) ? ("  " + this.ProjectFile) : null));
            }
        }

        public string ProjectFile
        {
            get
            {
                return this.projectFile;
            }
        }
    }
}