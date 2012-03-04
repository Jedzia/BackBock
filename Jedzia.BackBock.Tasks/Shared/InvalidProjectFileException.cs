namespace Jedzia.BackBock.Tasks.Shared
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Xml;
    using Jedzia.BackBock.Tasks.BuildEngine;

    /// <summary>
    /// This exception is thrown whenever there is a problem with the user's XML project file. 
    /// The problem might be semantic or syntactical. If the problem is in the syntax, it can 
    /// typically be caught by XSD validation.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProjectFileException"/> class.
        /// </summary>
        public InvalidProjectFileException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProjectFileException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProjectFileException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidProjectFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProjectFileException"/> class
        /// using rich error information.
        /// </summary>
        /// <param name="xmlNode">The XML node where the error is located. Can be null.</param>
        /// <param name="message">The error message text for the exception.</param>
        /// <param name="errorSubcategory">A description for the error. This parameter can be a null reference (Nothing in Visual Basic).</param>
        /// <param name="errorCode">The error code. This parameter can be a null reference (Nothing).</param>
        /// <param name="helpKeyword">The F1-help keyword for the host IDE. Can be null.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProjectFileException"/> class
        /// using rich error information.
        /// </summary>
        /// <param name="projectFile">The invalid project file. Can be an empty string.</param>
        /// <param name="lineNumber">The invalid line number in the project. Set to zero if not available.</param>
        /// <param name="columnNumber">The invalid column number in the project. Set to zero if not available.</param>
        /// <param name="endLineNumber">The end of a range of invalid lines in the project. Set to zero if not available.</param>
        /// <param name="endColumnNumber">The end of a range of invalid columns in the project. Set to zero if not available.</param>
        /// <param name="message">The error message text for the exception.</param>
        /// <param name="errorSubcategory">The description of the error. This parameter can be a null reference (Nothing in Visual Basic).</param>
        /// <param name="errorCode">The error code. This parameter can be a null reference (Nothing).</param>
        /// <param name="helpKeyword">The F1-help keyword for the host IDE. This parameter can be a null reference (Nothing).</param>
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

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic).
        ///   </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        ///   </PermissionSet>
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

        /// <summary>
        /// Gets the exception message, not including the project file.
        /// </summary>
        /// <value>The error message string only.</value>
        public string BaseMessage
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        /// Gets the invalid column number, if any, in the project.
        /// </summary>
        /// <value>The invalid column number, or zero.</value>
        public int ColumnNumber
        {
            get
            {
                return this.columnNumber;
            }
        }

        /// <summary>
        /// Gets the last column number, if any, of a range of invalid columns in the project.
        /// </summary>
        public int EndColumnNumber
        {
            get
            {
                return this.endColumnNumber;
            }
        }

        /// <summary>
        /// Gets the last line number, if any, of a range of invalid lines in the project.
        /// </summary>
        public int EndLineNumber
        {
            get
            {
                return this.endLineNumber;
            }
        }

        /// <summary>
        /// Gets the error code, if any, associated with the exception message.
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// Gets the error sub-category, if any that describes the type of this error.
        /// </summary>
        public string ErrorSubcategory
        {
            get
            {
                return this.errorSubcategory;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has been logged.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has been logged; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets the F1-help keyword, if any, associated with this error, for the host IDE.
        /// </summary>
        public string HelpKeyword
        {
            get
            {
                return this.helpKeyword;
            }
        }

        /// <summary>
        /// Gets the invalid line number, if any, in the project.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        /// <summary>
        /// Gets the exception message, including the affected project file, if any.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        ///   </returns>
        public override string Message
        {
            get
            {
                return (base.Message + ((this.ProjectFile != null) ? ("  " + this.ProjectFile) : null));
            }
        }

        /// <summary>
        /// Gets the project file, if any, associated with this exception.
        /// </summary>
        public string ProjectFile
        {
            get
            {
                return this.projectFile;
            }
        }
    }
}