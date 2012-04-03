namespace Jedzia.BackBock.Model.Tasks
{
    using System;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Represents a logging facility used by the <see cref="TaskSetupEngine"/>.
    /// </summary>
    public interface IBuildLogger
    {
        /// <summary>
        /// Attach the specified <see cref="ILogger"/> to this instance.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger" /> is <c>null</c>.</exception>
        void Initialize(ILogger logger);

        /// <summary>
        /// Posts a message to the logger.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="message">The message for the logger.</param>
        void LogBuildMessage(object sender, string senderName, string message);

        /// <summary>
        /// Registers the logger with the specified engine.
        /// </summary>
        /// <param name="engine">The engine to register the logger of this instance, with.</param>
        void RegisterLogger(Engine engine);
    }
}