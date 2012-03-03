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
            return resourceName;
        }

        /// <summary>
        /// Logs the command line for an underlying tool, executable file, or shell command of a task using the specified importance level.
        /// </summary>
        /// <param name="importance">One of the values of <see cref="MessageImportance"/> that indicates the importance level of the command line.</param>
        /// <param name="commandLine">The command line string.</param>
        public void LogCommandLine(MessageImportance importance, string commandLine)
        {
            ErrorUtilities.VerifyThrowArgumentNull(commandLine, "commandLine");
            //TaskCommandLineEventArgs e = new TaskCommandLineEventArgs(commandLine, this.TaskName, importance);
            //ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", e.Message);
            //this.BuildEngine.LogMessageEvent(e);
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
            //this.LogErrorWithCodeFromResources(null, null, 0, 0, 0, 0, messageResourceName, messageArgs);
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


}