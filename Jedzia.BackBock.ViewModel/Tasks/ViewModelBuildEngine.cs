namespace Jedzia.BackBock.ViewModel.Tasks
{
    using System;
    using System.Collections;
    using Jedzia.BackBock.ViewModel.MVVM.Messaging;
    using Jedzia.BackBock.ViewModel.Util;
    using Microsoft.Build.Framework;

    internal class ViewModelBuildEngine : IBuildEngine
    {
        IMessenger messengerInstance;
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBuildEngine"/> class.
        /// </summary>
        public ViewModelBuildEngine(IMessenger messengerInstance)
        {
            Guard.NotNull(() => messengerInstance, messengerInstance);
            this.messengerInstance = messengerInstance;
            //this.Enabled = true;
        }

        #region IBuildEngine Members

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            if (this.Enabled)
                messengerInstance.Send(e);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            throw new NotImplementedException();
        }

        public int ColumnNumberOfTaskNode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IBuildEngine Members

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            if (this.Enabled)
                messengerInstance.Send(e);
        }

        public bool ContinueOnError
        {
            get { return false; }
        }

        public int LineNumberOfTaskNode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ProjectFileOfTaskNode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}