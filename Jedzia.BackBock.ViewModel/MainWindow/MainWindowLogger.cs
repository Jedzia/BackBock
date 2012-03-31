// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowLogger.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System;
    using System.Text;
    using Jedzia.BackBock.ViewModel.MVVM.Threading;
    using Jedzia.BackBock.ViewModel.Util;

    /// <summary>
    /// Main window logger.
    /// </summary>
    public sealed class MainWindowLogger : ILogger
    {
        // private readonly StringBuilder logsb = new StringBuilder();
        #region Fields

        /// <summary>
        /// Reference to the main window.
        /// </summary>
        private readonly IMainWindow mainWindow;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowLogger"/> class.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        public MainWindowLogger(IMainWindow mainWindow)
        {
            Guard.NotNull(() => mainWindow, mainWindow);
            this.mainWindow = mainWindow;
        }

        #endregion

        /// <summary>
        /// Appends message event data to the log.
        /// </summary>
        /// <param name="e">The message text.</param>
        public void LogMessageEvent(string e)
        {
            var logsb = new StringBuilder();
            logsb.Append(DateTime.Now);
            logsb.Append(": ");
            logsb.Append(e);
            logsb.Append(Environment.NewLine);
            DispatcherHelper.CheckBeginInvokeOnUI(() => this.mainWindow.UpdateLogText(e));
        }

        /// <summary>
        /// Clears this instances log.
        /// </summary>
        public void Reset()
        {
            // this.logsb.Length = 0;
            DispatcherHelper.CheckBeginInvokeOnUI(() => this.mainWindow.ClearLogText());
        }

        /*public void LogMessageEvent(BuildMessageEventArgs e)
        {
            //var text = e.Timestamp + ":[" + e.ThreadId + "." + e.SenderName + "]" + e.Message + e.HelpKeyword;

            var logsb = new StringBuilder();
            logsb.Append(e.Timestamp);
            logsb.Append(":[");
            logsb.Append(e.ThreadId);
            logsb.Append(".");
            logsb.Append(e.SenderName);
            logsb.Append("]");
            logsb.Append(" ");
            logsb.Append(e.Message);
            logsb.Append("    (");
            logsb.Append(e.HelpKeyword);
            logsb.Append(")");
            logsb.Append(Environment.NewLine);
            var str = logsb.ToString();

            DispatcherHelper.CheckBeginInvokeOnUI(() => mainWindow.UpdateLogText(str));
            //mainWindow.UpdateLogText();
            //this.LogText += text + Environment.NewLine;
        }*/

        /*/// <summary>
        /// Sets and gets the LogText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LogText
        {
            get
            {
                return logsb.ToString();
            }
            set
            {
            }
        }*/
    }

    /// <summary>
    /// For testing, remove me.
    /// </summary>
    public interface IDings
    {
    }

    /// <summary>
    /// For testing, remove me.
    /// </summary>
    public class Dings : IDings
    {
        #region Fields

        private static int count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dings"/> class.
        /// </summary>
        public Dings()
        {
            this.MyCount = count;
            count++;
        }

        #endregion

        #region Properties

        public int MyCount { get; set; }

        #endregion
    }

    /// <summary>
    /// For testing, remove me.
    /// </summary>
    public class ProxyDings : IDings
    {
        #region Fields

        private static int count;
        private readonly IDings innerdings;

        #endregion

        #region Constructors

        public ProxyDings(IDings innerdings)
        {
            Guard.NotNull(() => innerdings, innerdings);
            this.innerdings = innerdings;
            this.MyCount = count;
            count++;
        }

        #endregion

        #region Properties

        public int MyCount { get; set; }

        #endregion
    }

    /// <summary>
    /// For testing, remove me.
    /// </summary>
    public interface IDingsFactory
    {
        IDings Create();
    }
}