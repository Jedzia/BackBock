namespace Jedzia.BackBock.Tasks.Utilities
{
    using System;
    using System.Resources;

    public class TaskLoggingHelper : MarshalByRefObject
    {
        private ITask taskInstance;
        public ResourceManager TaskResources;
        public string HelpKeywordPrefix;

        public virtual string FormatResourceString(string resourceName, params object[] args)
        {
            return resourceName;
        }
        public void LogErrorWithCodeFromResources(string messageResourceName, params object[] messageArgs)
        {
            //this.LogErrorWithCodeFromResources(null, null, 0, 0, 0, 0, messageResourceName, messageArgs);
        }

        public void LogMessageFromResources(MessageImportance importance, string messageResourceName, params object[] messageArgs)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(messageResourceName, "messageResourceName");
            //this.LogMessage(importance, this.FormatResourceString(messageResourceName, messageArgs), new object[0]);
        }
        public void LogCommandLine(MessageImportance importance, string commandLine)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(commandLine, "commandLine");
            //TaskCommandLineEventArgs e = new TaskCommandLineEventArgs(commandLine, this.TaskName, importance);
            //ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", e.Message);
            //this.BuildEngine.LogMessageEvent(e);
        }
        public TaskLoggingHelper(ITask taskInstance)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(taskInstance, "taskInstance");
            this.taskInstance = taskInstance;
        }

 

 

 

 

    }
}