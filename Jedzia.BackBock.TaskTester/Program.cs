using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.TaskTester
{
    using System.Collections;

    class Program
    {
        static void Main(string[] args)
        {
            var service = TaskRegistry.GetInstance();
            var task = (Backup)service["Backup"];
            var srcFiles = new[]
                               {
                                   "C:\\Temp\\NStub\\**", 
                                   //"C:\\Temp\\raabe.jpg", 
                                   //"C:\\Temp\\NStub"
                               };
            var dstFiles = new[]
                               {
                                   "C:\\Temp\\destination", 
                                   //"C:\\Temp\\destination\\raabe.jpg", 
                               };
            //ArrayList sourceFiles = new ArrayList(srcFiles);
            //var cparr = sourceFiles.ToArray(typeof(TaskExtension));
            task.SourceFiles = TaskItems(srcFiles);
            //task.DestinationFolder = TaskItems(dstFiles);
            task.DestinationFolder = new TaskItem("C:\\Temp\\destination");
            var result = task.Execute();
        }

        private static TaskItem[] TaskItems(string[] srcFiles)
        {
            return srcFiles.Select((e) => new TaskItem(e)).ToArray();
        }
    }
}
