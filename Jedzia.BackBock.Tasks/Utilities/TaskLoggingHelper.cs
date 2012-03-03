// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks.Utilities
{
    using System;
    using System.Resources;
    using Jedzia.BackBock.Tasks.Shared;
    using System.Threading;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides helper logging methods used by tasks.
    /// </summary>
    public class TaskLoggingHelper : MarshalByRefObject
    {
        #region Fields

        private ITask taskInstance;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLoggingHelper"/> class 
        /// and associates it with the specified task instance..
        /// </summary>
        /// <param name="taskInstance">The task containing an instance of this task.</param>
        public TaskLoggingHelper(ITask taskInstance)
        {
            ErrorUtilities.VerifyThrowArgumentNull(taskInstance, "taskInstance");
            this.taskInstance = taskInstance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the prefix used to compose Help keywords from resource names.
        /// </summary>
        /// <value>
        /// The prefix used to compose Help keywords from resource names.
        /// </value>
        /// <remarks>
        /// If a task does not have Help keywords associated with its messages, it can ignore
        /// this property or set it to <c>null</c>. If this property is set to an empty String, 
        /// resource names will be used verbatim as Help keywords.
        /// </remarks>
        public string HelpKeywordPrefix { get; set; }

        /// <summary>
        /// Gets or sets the culture-specific resources used by the logging methods.
        /// </summary>
        /// <value>
        /// A <see cref="ResourceManager"/> that represents the culture-specific resources 
        /// used by the logging methods. This value can be <c>null</c>.
        /// </value>
        /// <remarks>
        /// If derived classes have localized strings, then they should register their resources with this property.
        /// </remarks>
        public ResourceManager TaskResources { get; set; }

        #endregion

        /// <summary>
        /// Loads the specified resource string and optionally formats it using the given arguments.
        /// </summary>
        /// <param name="resourceName">The name of the string resource to load.</param>
        /// <param name="args">Optional arguments for formatting the loaded string.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks>
        /// The culture of the current thread is used for formatting.
        /// This method requires the owner task to have registered its resources with either 
        /// the Task base class constructor, or the <see cref="TaskResources"/> property.
        /// </remarks>
        public virtual string FormatResourceString(string resourceName, params object[] args)
        {
            ErrorUtilities.VerifyThrowArgumentNull(resourceName, "resourceName");
            ErrorUtilities.VerifyThrowInvalidOperation(this.TaskResources != null, "Shared.TaskResourcesNotRegistered", this.TaskName);
            string unformatted = this.TaskResources.GetString(resourceName, CultureInfo.CurrentUICulture);
            ErrorUtilities.VerifyThrowArgument(unformatted != null, "Shared.TaskResourceNotFound", resourceName, this.TaskName);
            return this.FormatString(unformatted, args);
        }
 

        /// <summary>
        /// Logs the command line for an underlying tool, executable file, or shell command of a task using the specified importance level.
        /// </summary>
        /// <param name="importance">One of the values of <see cref="MessageImportance"/> that indicates the importance level of the command line.</param>
        /// <param name="commandLine">The command line string.</param>
        public void LogCommandLine(MessageImportance importance, string commandLine)
        {
            ErrorUtilities.VerifyThrowArgumentNull(commandLine, "commandLine");
            TaskCommandLineEventArgs e = new TaskCommandLineEventArgs(commandLine, this.TaskName, importance);
            ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", e.Message);
            this.BuildEngine.LogMessageEvent(e);
        }

        /// <summary>
        /// Logs an error with an error code using the specified resource string.   
        /// </summary>
        /// <param name="messageResourceName">The name of the string resource to load.</param>
        /// <param name="messageArgs">The arguments for formatting the loaded string.</param>
        /// <remarks>
        /// If the message begins with an error code, the code is extracted and logged with the message.
        /// If a Help keyword prefix has been provided, it is also logged with the message. 
        /// The Help keyword is composed by appending the string resource name to the Help keyword 
        /// prefix. A task can provide a Help keyword prefix with either the Task base class 
        /// constructor, or the <see cref="HelpKeywordPrefix"/> property.
        /// </remarks>
        public void LogErrorWithCodeFromResources(string messageResourceName, params object[] messageArgs)
        {
            this.LogErrorWithCodeFromResources(null, null, 0, 0, 0, 0, messageResourceName, messageArgs);
            //throw new NotImplementedException("LogErrorWithCodeFromResources not implemented.");
        }
        public void LogErrorWithCodeFromResources(string subcategoryResourceName, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string messageResourceName, params object[] messageArgs)
        {
            string str2;
            ErrorUtilities.VerifyThrowArgumentNull(messageResourceName, "messageResourceName");
            string subcategory = null;
            if (subcategoryResourceName != null)
            {
                subcategory = this.FormatResourceString(subcategoryResourceName, new object[0]);
            }
            string errorCode = this.ExtractMessageCode(this.FormatResourceString(messageResourceName, messageArgs), false, out str2);
            string helpKeyword = null;
            if (this.HelpKeywordPrefix != null)
            {
                helpKeyword = this.HelpKeywordPrefix + messageResourceName;
            }
            this.LogError(subcategory, errorCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, str2, new object[0]);
        }
        public void LogError(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs)
        {
            ErrorUtilities.VerifyThrowArgumentNull(message, "message");
            ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", message);
            if (((file == null) || (file.Length == 0)) && (((lineNumber == 0) && (columnNumber == 0)) && !this.BuildEngine.ContinueOnError))
            {
                //file = this.BuildEngine.ProjectFileOfTaskNode;
                file = "NO FILE";
                //lineNumber = this.BuildEngine.LineNumberOfTaskNode;
                lineNumber = 0;
                //columnNumber = this.BuildEngine.ColumnNumberOfTaskNode;
                columnNumber = 0;
            }
            BuildErrorEventArgs e = new BuildErrorEventArgs(subcategory, errorCode, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, this.FormatString(message, messageArgs), helpKeyword, this.TaskName);
            this.BuildEngine.LogErrorEvent(e);
            this.hasLoggedErrors = true;
        }

        private bool hasLoggedErrors;

        internal string ExtractMessageCode(string message, bool filterMSBuildOnlyMessages, out string messageWithoutCodePrefix)
        {
            string str;
            ErrorUtilities.VerifyThrowArgumentNull(message, "message");
            if (filterMSBuildOnlyMessages)
            {
                messageWithoutCodePrefix = ResourceUtilities.ExtractMessageCode(null, message, out str);
                return str;
            }
            messageWithoutCodePrefix = ResourceUtilities.ExtractMessageCode(messageCodePattern, message, out str);
            return str;
        }

        private static readonly Regex messageCodePattern;
        static TaskLoggingHelper()
        {
            messageCodePattern = new Regex(@"^\s*(?<CODE>[A-Za-z]+\d+):\s*(?<MESSAGE>.*)$", RegexOptions.Singleline);
        }

        /// <summary>
        /// Logs a message with the specified resource string and importance.
        /// </summary>
        /// <param name="importance">One of the values of <see cref="MessageImportance"/> that indicates the importance level of the command line.</param>
        /// <param name="messageResourceName">The name of the string resource to load.</param>
        /// <param name="messageArgs">The arguments for formatting the loaded string.</param>
        /// <remarks>Take care to order the parameters correctly or the other overload will be called inadvertently.</remarks>
        public void LogMessageFromResources(
            MessageImportance importance, string messageResourceName, params object[] messageArgs)
        {
            ErrorUtilities.VerifyThrowArgumentNull(messageResourceName, "messageResourceName");
            this.LogMessage(importance, this.FormatResourceString(messageResourceName, messageArgs), new object[0]);
        }

        public void LogMessage(MessageImportance importance, string message, params object[] messageArgs)
        {
            ErrorUtilities.VerifyThrowArgumentNull(message, "message");
            BuildMessageEventArgs e = new BuildMessageEventArgs(this.FormatString(message, messageArgs), null, this.TaskName, importance);
            ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", e.Message);
            this.BuildEngine.LogMessageEvent(e);
        }


        public virtual string FormatString(string unformatted, params object[] args)
        {
            ErrorUtilities.VerifyThrowArgumentNull(unformatted, "unformatted");
            return ResourceUtilities.FormatString(unformatted, args);
        }


        private string taskName;
 

        protected string TaskName
        {
            get
            {
                if (this.taskName == null)
                {
                    this.taskName = this.taskInstance.GetType().Name;
                }
                return this.taskName;
            }
        }
 

 

        protected IBuildEngine BuildEngine
        {
            get
            {
                return this.taskInstance.BuildEngine;
            }
        }

    }


    [Serializable]
    public abstract class BuildEventArgs : EventArgs
    { 
        // Fields
        private string helpKeyword;
        private string message;
        private string senderName;
        private int threadId;
        private DateTime timestamp;

        // Methods
        protected BuildEventArgs()
        {
            this.timestamp = DateTime.Now;
            this.threadId = Thread.CurrentThread.GetHashCode();
        }

        protected BuildEventArgs(string message, string helpKeyword, string senderName)
            : this()
        {
            this.message = message;
            this.helpKeyword = helpKeyword;
            this.senderName = senderName;
        }

        // Properties
        public string HelpKeyword
        {
            get
            {
                return this.helpKeyword;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public string SenderName
        {
            get
            {
                return this.senderName;
            }
        }

        public int ThreadId
        {
            get
            {
                return this.threadId;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }
    }

    [Serializable]
    public class BuildMessageEventArgs : BuildEventArgs
    {
        // Fields
        private MessageImportance importance;

        // Methods
        protected BuildMessageEventArgs()
        {
        }

        public BuildMessageEventArgs(string message, string helpKeyword, string senderName, MessageImportance importance)
            : base(message, helpKeyword, senderName)
        {
            this.importance = importance;
        }

        // Properties
        public MessageImportance Importance
        {
            get
            {
                return this.importance;
            }
        }
    }

    [Serializable]
    public class TaskCommandLineEventArgs : BuildMessageEventArgs
    {
        // Methods
        protected TaskCommandLineEventArgs()
        {
        }

        public TaskCommandLineEventArgs(string commandLine, string taskName, MessageImportance importance)
            : base(commandLine, null, taskName, importance)
        {
        }

        // Properties
        public string CommandLine
        {
            get
            {
                return base.Message;
            }
        }

        public string TaskName
        {
            get
            {
                return base.SenderName;
            }
        }
    }

[Serializable]
public class BuildErrorEventArgs : BuildEventArgs
{
    // Fields
    private string code;
    private int columnNumber;
    private int endColumnNumber;
    private int endLineNumber;
    private string file;
    private int lineNumber;
    private string subcategory;

    // Methods
    protected BuildErrorEventArgs()
    {
    }

    public BuildErrorEventArgs(string subcategory, string code, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, string helpKeyword, string senderName) : base(message, helpKeyword, senderName)
    {
        this.subcategory = subcategory;
        this.code = code;
        this.file = file;
        this.lineNumber = lineNumber;
        this.columnNumber = columnNumber;
        this.endLineNumber = endLineNumber;
        this.endColumnNumber = endColumnNumber;
    }

    // Properties
    public string Code
    {
        get
        {
            return this.code;
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

    public string File
    {
        get
        {
            return this.file;
        }
    }

    public int LineNumber
    {
        get
        {
            return this.lineNumber;
        }
    }

    public string Subcategory
    {
        get
        {
            return this.subcategory;
        }
    }
}

}