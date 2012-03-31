namespace Jedzia.BackBock.ViewModel.MainWindow
{
    /// <summary>
    /// Represents an instance that presents log data to the user.
    /// </summary>
    public interface ILogger
    {
        // string LogText { get; set; }

        /// <summary>
        /// Clears this instances log.
        /// </summary>
        void Reset();

        /// <summary>
        /// Appends message event data to the log.
        /// </summary>
        /// <param name="e">The message text.</param>
        void LogMessageEvent(string e);
        
        // void LogMessageEvent(BuildMessageEventArgs e);
    }
}
