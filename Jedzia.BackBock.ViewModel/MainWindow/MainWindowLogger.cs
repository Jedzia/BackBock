using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Jedzia.BackBock.ViewModel.Util;
using Jedzia.BackBock.ViewModel.MVVM.Threading;
using Jedzia.BackBock.DataAccess;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    public sealed class MainWindowLogger : ILogger
    {
        //private readonly StringBuilder logsb = new StringBuilder();
        private readonly IMainWindow mainWindow;

        public MainWindowLogger(IMainWindow mainWindow)
        {
            Guard.NotNull(() => mainWindow, mainWindow);
            this.mainWindow = mainWindow;
        }

        public void Reset()
        {
            //this.logsb.Length = 0;
            DispatcherHelper.CheckBeginInvokeOnUI(() => mainWindow.ResetLogText());
        }

        public void LogMessageEvent(string e)
        {
            var logsb = new StringBuilder();
            logsb.Append(DateTime.Now);
            logsb.Append(": ");
            logsb.Append(e);
            logsb.Append(Environment.NewLine);
            DispatcherHelper.CheckBeginInvokeOnUI(() => mainWindow.UpdateLogText(e));
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
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
        }

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

    public class RepositoryLogger : BackupDataFsRepository
    {
        private readonly BackupDataRepository innerRepository;
        private readonly ILogger auditor;
        public RepositoryLogger(BackupDataRepository repository, ILogger auditor)
        {
            Guard.NotNull(() => repository, repository);
            Guard.NotNull(() => auditor, auditor);
            this.innerRepository = repository;
            this.auditor = auditor;
        }

        public override Jedzia.BackBock.DataAccess.DTO.BackupData GetBackupData()
        {
            auditor.LogMessageEvent("GetBackupData()");
            return innerRepository.GetBackupData();
        }

        public override BackupRepositoryType RepositoryType
        {
            get { return innerRepository.RepositoryType; }
        }

        public override Jedzia.BackBock.DataAccess.DTO.BackupData LoadBackupData(string filename, System.Collections.Specialized.StringDictionary parameters)
        {
            auditor.LogMessageEvent("LoadBackupData(" + filename+ ")");
            return ((BackupDataFsRepository)innerRepository).LoadBackupData(filename,  parameters);
        }
    }

    public interface IDings
    {

    }

    public class Dings : IDings
	{
        static int count = 0;
        public int MyCount { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dings"/> class.
        /// </summary>
        public Dings()
        {
            MyCount = count;
            count++;
        }
	}


    public class ProxyDings : IDings
    {
        static int count = 0;
        public int MyCount { get; set; }
        private readonly IDings innerdings;
        public ProxyDings(IDings innerdings)
        {
            Guard.NotNull(() => innerdings, innerdings);
            this.innerdings = innerdings;
            MyCount = count;
            count++;
        }
    }

    public interface IDingsFactory
    {
        IDings Create();
    }

}
