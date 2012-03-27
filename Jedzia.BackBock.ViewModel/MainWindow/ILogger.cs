using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    public interface ILogger
    {
        //string LogText { get; set; }
        void Reset();
        void LogMessageEvent(string e);
        void LogMessageEvent(BuildMessageEventArgs e);
    }
}
