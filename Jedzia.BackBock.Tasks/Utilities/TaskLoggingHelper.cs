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
    /// <example>
    /// The following example shows the code for a task that creates one or more directories. 
    /// <para> </para>
    /// <code lang="C#">
    /// using System;
    /// using System.IO;
    /// using System.Security;
    /// using System.Collections;
    /// using Microsoft.Build.Framework;
    /// using Microsoft.Build.Utilities;
    /// 
    /// namespace Microsoft.Build.Tasks
    /// {
    ///     /*
    ///      * Class: MakeDir
    ///      *
    ///      * An MSBuild task that creates one or more directories.
    ///      *
    ///      */
    ///     public class MakeDir : Task
    ///     {
    ///         // The Required attribute indicates the following to MSBuild:
    ///         //         - if the parameter is a scalar type, and it is not supplied, fail the build immediately
    ///         //         - if the parameter is an array type, and it is not supplied, pass in an empty array
    ///         // In this case the parameter is an array type, so if a project fails to pass in a value for the 
    ///             // Directories parameter, the task will get invoked, but this implementation will do nothing,
    ///             // because the array will be empty.
    ///         [Required]
    ///             // Directories to create.
    ///         public ITaskItem[] Directories
    ///         {
    ///             get
    ///             {
    ///                 return directories;
    ///             }
    /// 
    ///             set
    ///             {
    ///                 directories = value;
    ///             }
    ///         }
    /// 
    ///         // The Output attribute indicates to MSBuild that the value of this property can be gathered after the
    ///         // task has returned from Execute(), if the project has an &lt;Output&gt; tag under this task's element for 
    ///         // this property.
    ///         [Output]
    ///         // A project may need the subset of the inputs that were actually created, so make that available here.
    ///         public ITaskItem[] DirectoriesCreated
    ///         {
    ///             get
    ///             {
    ///                 return directoriesCreated;
    ///             }
    ///         }
    /// 
    ///         private ITaskItem[] directories;
    ///         private ITaskItem[] directoriesCreated;
    /// 
    ///         /// &lt;summary&gt;
    ///         /// Execute is part of the Microsoft.Build.Framework.ITask interface.
    ///         /// When it's called, any input parameters have already been set on the task's properties.
    ///         /// It returns true or false to indicate success or failure.
    ///         /// &lt;/summary&gt;
    ///         public override bool Execute()
    ///         {
    ///             ArrayList items = new ArrayList();
    ///             foreach (ITaskItem directory in Directories)
    ///             {
    ///                 // ItemSpec holds the filename or path of an Item
    ///                 if (directory.ItemSpec.Length &gt; 0)
    ///                 {
    ///                     try
    ///                     {
    ///                         // Only log a message if we actually need to create the folder
    ///                         if (!Directory.Exists(directory.ItemSpec))
    ///                         {
    ///                             Log.LogMessage(MessageImportance.Normal, &quot;Creating directory &quot; + directory.ItemSpec);
    ///                             Directory.CreateDirectory(directory.ItemSpec);
    ///                         }
    /// 
    ///                         // Add to the list of created directories
    ///                         items.Add(directory);
    ///                     }
    ///                     // If a directory fails to get created, log an error, but proceed with the remaining 
    ///                     // directories.
    ///                     catch (Exception ex)
    ///                     {
    ///                         if (ex is IOException
    ///                             || ex is UnauthorizedAccessException
    ///                             || ex is PathTooLongException
    ///                             || ex is DirectoryNotFoundException
    ///                             || ex is SecurityException)
    ///                         {
    ///                             Log.LogError(&quot;Error trying to create directory &quot; + directory.ItemSpec + &quot;. &quot; + ex.Message);
    ///                         }
    ///                         else
    ///                         {
    ///                             throw;
    ///                         }
    ///                     }
    ///                 }
    ///             }
    /// 
    ///             // Populate the &quot;DirectoriesCreated&quot; output items.
    ///             directoriesCreated = (ITaskItem[])items.ToArray(typeof(ITaskItem));
    /// 
    ///             // Log.HasLoggedErrors is true if the task logged any errors -- even if they were logged 
    ///             // from a task's constructor or property setter. As long as this task is written to always log an error
    ///             // when it fails, we can reliably return HasLoggedErrors.
    ///             return !Log.HasLoggedErrors;
    ///         }
    ///     }
    /// }</code>
    /// </example>
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
        /// <summary>
        /// Logs an error using the specified resource string and other error details.
        /// </summary>
        /// <remarks>
        /// If the message begins with an error code, the code is extracted and logged with the message. 
        /// <para> </para>
        /// <para>If a Help keyword prefix has been provided, it is also logged with the message. The Help keyword is composed by appending the string resource name to the Help keyword prefix. A task can provide a Help keyword prefix with either the Task base class constructor, or the HelpKeywordPrefix property.</para>
        /// <para> </para>
        /// <para>The parameters subCategoryResourceName, and file can be a null reference (Nothing in Visual Basic).</para>
        /// <para> </para>
        /// <para>The parameters lineNumber, columnNumber, endLineNumber, and endColumnNumber should be set to 0 if they are not available.</para>
        /// </remarks>
        /// <param name="subcategoryResourceName">The name of the string resource that describes the error type.</param>
        /// <param name="file">The path to the file containing the error.</param>
        /// <param name="lineNumber">The line in the file where the error occurs.</param>
        /// <param name="columnNumber">The column in the file where the error occurs.</param>
        /// <param name="endLineNumber">The end line in the file where the error occurs.</param>
        /// <param name="endColumnNumber">The end column in the file where the error occurs.</param>
        /// <param name="messageResourceName">The name of the string resource to load.</param>
        /// <param name="messageArgs">The arguments for formatting the loaded string.</param>
        /// <exception cref="ArgumentNullException">messageResourceName is a null reference (Nothing in Visual Basic).</exception>
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

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <remarks>
        /// The parameters subCategory, errorCode, helpKeyword, and file can be a null reference (Nothing in Visual Basic).
        /// <para></para>
        /// <para>The parameters lineNumber, columnNumber, endLineNumber, and endColumnNumber should be set to 0 if they are not available.</para>
        /// </remarks>
        /// <param name="subcategory">The description of the error type.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="helpKeyword">The Help keyword to associate with the error.</param>
        /// <param name="file">The path to the file containing the error.</param>
        /// <param name="lineNumber">The line in the file where the error occurs.</param>
        /// <param name="columnNumber">The column in the file where the error occurs.</param>
        /// <param name="endLineNumber">The end line in the file where the error occurs.</param>
        /// <param name="endColumnNumber">The end column in the file where the error occurs.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The arguments for formatting the loaded string.</param>
        /// <exception cref="ArgumentNullException">messageResourceName is a null reference (Nothing in Visual Basic).</exception>
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

        /// <summary>
        /// Logs a message with the specified string and importance.
        /// </summary>
        /// <param name="importance">One of the enumeration values that specifies the importance of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The arguments for formatting the message.</param>
        /// <exception cref="ArgumentNullException">messageResourceName is a null reference (Nothing in Visual Basic).</exception>
        public void LogMessage(MessageImportance importance, string message, params object[] messageArgs)
        {
            ErrorUtilities.VerifyThrowArgumentNull(message, "message");
            BuildMessageEventArgs e = new BuildMessageEventArgs(this.FormatString(message, messageArgs), null, this.TaskName, importance);
            ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", e.Message);
            this.BuildEngine.LogMessageEvent(e);
        }


        /// <summary>
        /// Formats the given string using the given arguments.
        /// </summary>
        /// <param name="unformatted">The string to format.</param>
        /// <param name="args">Arguments for formatting.</param>
        /// <returns>The formatted string.</returns>
        /// <exception cref="ArgumentNullException">unformatted is a null reference (Nothing in Visual Basic).</exception>
        public virtual string FormatString(string unformatted, params object[] args)
        {
            ErrorUtilities.VerifyThrowArgumentNull(unformatted, "unformatted");
            return ResourceUtilities.FormatString(unformatted, args);
        }


        private string taskName;


        /// <summary>
        /// Gets the name of the parent task.
        /// </summary>
        /// <value>
        /// The name of the task.
        /// </value>
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




        /// <summary>
        /// Gets the build engine that is associated with the task.
        /// </summary>
        protected IBuildEngine BuildEngine
        {
            get
            {
                return this.taskInstance.BuildEngine;
            }
        }

    }


    /// <summary>
    /// Provides data for the AnyEventRaised event
    /// </summary>
    /// <remarks>
    /// The BuildEventArgs class is an abstract base class for all Microsoft.Build.Framework event argument classes.
    /// </remarks>
    /// <example>
    /// The following example shows how to write a basic logger that responds to build events.
    /// <para></para>
    /// <code source="..\Jedzia.BackBock.Tasks\Docs\Examples\BuildEventArgs-Summary.cs" lang="cs" title="The following example shows how to write a basic logger that responds to build events."/>
    /// </example>
    [Serializable]
    public abstract class BuildEventArgs : EventArgs
    {
        private string helpKeyword;
        private string message;
        private string senderName;
        private int threadId;
        private DateTime timestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEventArgs">BuildEventArgs</see> class.
        /// </summary>
        /// <remarks>
        /// This implementation of the BuildEventArgs constructor takes no parameters, and sets the TimeStamp()()() property to Now and the ThreadId property to the current thread.
        /// </remarks>
        protected BuildEventArgs()
        {
            this.timestamp = DateTime.Now;
            this.threadId = Thread.CurrentThread.GetHashCode();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEventArgs"/> class
        /// with the specified Message, HelpKeyword, and SenderName values.
        /// </summary>
        /// <param name="message">The text of the event.</param>
        /// <param name="helpKeyword">The Help keyword associated with the event.</param>
        /// <param name="senderName">The source of the event.</param>
        protected BuildEventArgs(string message, string helpKeyword, string senderName)
            : this()
        {
            this.message = message;
            this.helpKeyword = helpKeyword;
            this.senderName = senderName;
        }

        /// <summary>
        /// Gets the Help keyword for the event.
        /// </summary>
        public string HelpKeyword
        {
            get
            {
                return this.helpKeyword;
            }
        }

        /// <summary>
        /// Gets the message for the event.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }
        }

        /// <summary>
        /// Gets the name of the Object raising the event.
        /// </summary>
        /// <value>
        /// The name of the Object raising the event.
        /// </value>
        public string SenderName
        {
            get
            {
                return this.senderName;
            }
        }

        /// <summary>
        /// Gets an integer identifier for the thread that raised the event.
        /// </summary>
        public int ThreadId
        {
            get
            {
                return this.threadId;
            }
        }

        /// <summary>
        /// Gets the time the event was raised as a DateTime.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }
    }

    /// <summary>
    /// Provides data for the MessageRaised event
    /// </summary>
    [Serializable]
    public class BuildMessageEventArgs : BuildEventArgs
    {
        // Fields
        private MessageImportance importance;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildMessageEventArgs"/> class.
        /// </summary>
        protected BuildMessageEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildMessageEventArgs"/> class.
        /// </summary>
        /// <param name="message">The text of the event.</param>
        /// <param name="helpKeyword">The Help keyword associated with the event.</param>
        /// <param name="senderName">The source of the event.</param>
        /// <param name="importance">A MessageImportance value indicating the importance of the event.</param>
        public BuildMessageEventArgs(string message, string helpKeyword, string senderName, MessageImportance importance)
            : base(message, helpKeyword, senderName)
        {
            this.importance = importance;
        }

        /// <summary>
        /// Gets the importance of the event.
        /// </summary>
        /// <value>
        /// A MessageImportance value indicating the importance of the event.
        /// </value>
        public MessageImportance Importance
        {
            get
            {
                return this.importance;
            }
        }
    }

    /// <summary>
    /// Provides data for the MessageRaised event.
    /// </summary>
    [Serializable]
    public class TaskCommandLineEventArgs : BuildMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCommandLineEventArgs"/> class.
        /// </summary>
        protected TaskCommandLineEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCommandLineEventArgs"/> class.
        /// </summary>
        /// <param name="commandLine">The command line used by the task to run the underlying program.</param>
        /// <param name="taskName">The name of the task raising the event.</param>
        /// <param name="importance">A MessageImportance value indicating the importance of the event.</param>
        public TaskCommandLineEventArgs(string commandLine, string taskName, MessageImportance importance)
            : base(commandLine, null, taskName, importance)
        {
        }

        /// <summary>
        /// Gets the command line used by the task to run the underlying program.
        /// </summary>
        public string CommandLine
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        /// Gets the name of the task that raised the event.
        /// </summary>
        /// <value>
        /// The name of the task that raised the event.
        /// </value>
        public string TaskName
        {
            get
            {
                return base.SenderName;
            }
        }
    }

    /// <summary>
    /// Provides data for the ErrorRaised event.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildErrorEventArgs"/> class.
        /// </summary>
        protected BuildErrorEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildErrorEventArgs"/> class
        /// with the specified SubCategory()()(), Code, File, LineNumber, ColumnNumber, 
        /// EndLineNumber, EndColumnNumber, Message, HelpKeyword, and SenderName values.
        /// </summary>
        /// <param name="subcategory">The custom subcategory of the event.</param>
        /// <param name="code">The error code of the event.</param>
        /// <param name="file">The path to the file containing the error.</param>
        /// <param name="lineNumber">The line in the file where the error occurs.</param>
        /// <param name="columnNumber">The column in the file where the error occurs.</param>
        /// <param name="endLineNumber">The end line in the file where the error occurs.</param>
        /// <param name="endColumnNumber">The end column in the file where the error occurs.</param>
        /// <param name="message">The text of the event.</param>
        /// <param name="helpKeyword">The Help keyword to associate with the error.</param>
        /// <param name="senderName">Name of the sender.</param>
        public BuildErrorEventArgs(string subcategory, string code, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, string helpKeyword, string senderName)
            : base(message, helpKeyword, senderName)
        {
            this.subcategory = subcategory;
            this.code = code;
            this.file = file;
            this.lineNumber = lineNumber;
            this.columnNumber = columnNumber;
            this.endLineNumber = endLineNumber;
            this.endColumnNumber = endColumnNumber;
        }

        /// <summary>
        /// Gets the error code of the event.
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }
        }

        /// <summary>
        /// Gets the column number that corresponds to the beginning of the section of code that raised the event.
        /// </summary>
        public int ColumnNumber
        {
            get
            {
                return this.columnNumber;
            }
        }

        /// <summary>
        /// Gets the column number that corresponds to the end of the section of code that raised the event.
        /// </summary>
        public int EndColumnNumber
        {
            get
            {
                return this.endColumnNumber;
            }
        }

        /// <summary>
        /// Gets the line number that corresponds to the end of the section of code that raised the event.
        /// </summary>
        public int EndLineNumber
        {
            get
            {
                return this.endLineNumber;
            }
        }

        /// <summary>
        /// Gets the name of the file that raised the event.
        /// </summary>
        public string File
        {
            get
            {
                return this.file;
            }
        }

        /// <summary>
        /// Gets the line number that corresponds to the beginning of the section of code that raised the event.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        /// <summary>
        /// Gets the custom subtype of the event.
        /// </summary>
        public string Subcategory
        {
            get
            {
                return this.subcategory;
            }
        }
    }

}