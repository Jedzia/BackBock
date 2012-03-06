using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.TaskTester
{
    using System.Collections;
    using Microsoft.Build.Utilities;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Summary
    /// </summary>
    public static class Consts
    {
        public const string InputFileA = @"C:\Temp\FolderA";
    }
    /// <summary>
    /// Blah
    /// </summary>
    /// <example>
    /// The following example shows how to write a basic logger that responds to build events.
    /// <para></para>
    /// <code source="..\Jedzia.BackBock.TaskTester\Program.cs" lang="cs" title="The following example shows how to write a basic logger that responds to build events."/>
    /// </example>
    public class Program
    {
        static void Main(string[] args)
        {
            //CheckBackupTask();
            /*var task = new Zip();
            task.TaskAction = "Create";
            task.CompressFiles = new[] { new TaskItem(Consts.InputFileA), };
            task.ZipFileName = new TaskItem(@"D:\TestZip.zip");
            TestB(task);*/

            var paths = new[] { Consts.InputFileA };
            //TestA(typeof(Backup), "SourceFiles", paths);
            TestA(typeof(Zip), "CompressFiles", paths);
        }

        private static void TestB(ITask task)
        {
        }

        private static void TestA(Type taskType, string sourceParameter, IEnumerable<string> paths)
        {
            var engine = new Engine(@"D:\E\Projects\CSharp\BackBock\Jedzia.BackBock.Application\bin\Debug");
            engine.RegisterLogger(new Logger());

            var proj = engine.CreateNewProject();
            proj.DefaultToolsVersion = "3.5";
            var target = proj.Targets.AddNewTarget("mainTarget");

            #region itemgroup
            var big = proj.AddNewItemGroup();

            var inclEle = new StringBuilder();
            foreach (var path in paths)
            {
                inclEle.Append(path);
                inclEle.Append(";");
            }
            inclEle.Remove(inclEle.Length, 1);
            var include = inclEle.ToString();

            BuildItem cr = big.AddNewItem("FilesToZip", include);
            // cr.Exclude = "*.txt";
            #endregion

            //Type btasktype = typeof(Zip);
            proj.AddNewUsingTaskFromAssemblyName(taskType.FullName, taskType.Assembly.FullName);
            var batask = target.AddNewTask(taskType.FullName);
            //batask.SetParameterValue("SourceFiles", @"C:\Temp\company.xmi");
            batask.SetParameterValue(sourceParameter, @"@(FilesToZip)");
            var pars = batask.GetParameterNames();

        }

        private static void CheckBackupTask()
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
