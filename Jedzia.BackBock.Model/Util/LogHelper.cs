// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Util
{
    using System;
    using Microsoft.Build.Framework;
    using System.Globalization;

    /// <summary>
    /// Helper-Wrapper for Logging Microsoft.Build.Framework <see cref="BuildEventArgs"/> events.
    /// </summary>
    internal class LogHelper : ILogger
    {
        #region Fields

        /// <summary>
        /// For testing purposes. Switch logging of with <c>false</c>.
        /// </summary>
        private bool ENABLELOGGING = true;

        /// <summary>
        /// Holds a reference to the logging method.
        /// </summary>
        private readonly Action<string> log;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogHelper"/> class.
        /// </summary>
        /// <param name="log">The logging method to forward the messages, to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="log" /> is <c>null</c>.</exception>
        public LogHelper(Action<string> log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            this.log = log;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
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

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>
        /// The verbosity.
        /// </value>
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

        /// <summary>
        /// Initializes the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        public void Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += Log;
            eventSource.WarningRaised += Log;
            eventSource.ErrorRaised += Log;
        }

        /// <summary>
        /// Shut down this instance.
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="e">The <see cref="Microsoft.Build.Framework.BuildErrorEventArgs"/> instance containing the event data.</param>
        private void Log(object sender, BuildErrorEventArgs e)
        {
            if (ENABLELOGGING)
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}:{3}:{0} [{7}]   Error: {4}({5}{6}):{2}",
                    e.Timestamp,
                    e.ThreadId,
                    e.Message,
                    e.SenderName,
                    e.File,
                    e.LineNumber,
                    e.ColumnNumber,
                    e.Code);
                this.log(msg);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="e">The <see cref="Microsoft.Build.Framework.BuildWarningEventArgs"/> instance containing the event data.</param>
        private void Log(object sender, BuildWarningEventArgs e)
        {
            if (ENABLELOGGING)
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}:{3}:{0} [{7}] Warning: {4}({5}{6}):{2}",
                    e.Timestamp,
                    e.ThreadId,
                    e.Message,
                    e.SenderName,
                    e.File,
                    e.LineNumber,
                    e.ColumnNumber,
                    e.Code);
                this.log(msg);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="e">The <see cref="Microsoft.Build.Framework.BuildMessageEventArgs"/> instance containing the event data.</param>
        private void Log(object sender, BuildMessageEventArgs e)
        {
            if (ENABLELOGGING)
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}:{3}:{0} {2}",
                    e.Timestamp, 
                    e.ThreadId,
                    e.Message,
                    e.SenderName);
                this.log(msg);
            }
        }
    }
}