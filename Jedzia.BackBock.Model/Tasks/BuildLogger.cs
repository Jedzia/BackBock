// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildLogger.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Tasks
{
    using System;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;

    /// <summary>
    /// A logging facility helper for the <see cref="TaskSetupEngine"/>.
    /// </summary>
    public class BuildLogger : IBuildLogger
    {
        private readonly ILogger logger;

        #region Fields

        private readonly EventSource eventSource;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildLogger"/> class
        /// and creates new logging infrastructure.
        /// </summary>
        /// <param name="logger">The logger used by this instance. Can be <c>null</c>.</param>
        public BuildLogger(ILogger logger)
            : this(logger, new EventSource())
        {
            this.logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger used by this instance. Can be <c>null</c>.</param>
        /// <param name="eventSource">The event source.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventSource"/> is <c>null</c>.</exception>
        internal BuildLogger(ILogger logger, EventSource eventSource)
        {
            if (eventSource == null)
            {
                throw new ArgumentNullException("eventSource");
            }

            this.eventSource = eventSource;
            if (logger != null)
            {
                Initialize(logger);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the event source used for logging of this instance.
        /// </summary>
        internal EventSource EventSource
        {
            get
            {
                return this.eventSource;
            }
        }

        #endregion

        /// <summary>
        /// Attach the specified <see cref="ILogger"/> to this instance.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger" /> is <c>null</c>.</exception>
        public void Initialize(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Initialize(this.EventSource);
        }

        /// <summary>
        /// Registers the logger with the specified engine.
        /// </summary>
        /// <param name="engine">The engine to register the logger of this instance, with.</param>
        public void RegisterLogger(Engine engine)
        {
            engine.RegisterLogger(this.logger);
        }

        /// <summary>
        /// Posts a message to the logger.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="message">The message for the logger.</param>
        public void LogBuildMessage(object sender, string senderName, string message)
        {
            this.EventSource.FireMessageRaised(
                sender, 
                new BuildMessageEventArgs(message, string.Empty, senderName, MessageImportance.Low));
        }
    }
}