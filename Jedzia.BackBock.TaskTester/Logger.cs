using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;

namespace Jedzia.BackBock.TaskTester
{
    class Messenger
    {
        public void Send(object sender, BuildErrorEventArgs e)
        {
            var message = string.Format("ERROR {0}:[{1}-{2}] {3}", e.Timestamp, e.ThreadId, e.SenderName, e.Message);
            Console.WriteLine(message);
        }

        internal void Send(object sender, BuildMessageEventArgs e)
        {
            var message = string.Format("{0}:[{1}-{2}] {3}", e.Timestamp, e.ThreadId, e.SenderName, e.Message);
            Console.WriteLine(message);
        }
    }

    class Logger : ILogger
    {
        #region ILogger Members
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Logger"/> class.
        /// </summary>
        public Logger()
        {
            this.Enabled = true;
        }

        private Messenger messengerInstance;
        public Messenger MessengerInstance
        {
            get
            {
                if (messengerInstance == null)
                {
                    this.messengerInstance = new Messenger();
                }
                return this.messengerInstance;
            }
        }

        /// <summary>
        /// Initializes the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        public void Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += Log;
            eventSource.ErrorRaised += Log;
        }

        private void Log(object sender, BuildErrorEventArgs e)
        {
            if (this.Enabled)
                MessengerInstance.Send(sender, e);
        }

        public void Log(object sender, BuildMessageEventArgs buildMessageEventArgs)
        {
            if (this.Enabled)
                MessengerInstance.Send(sender, buildMessageEventArgs);
        }

        public void Shutdown()
        {
        }

        public string Parameters
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public LoggerVerbosity Verbosity
        {
            get
            {
                return LoggerVerbosity.Detailed;
            }
            set
            {
            }
        }

        #endregion
    }
}
