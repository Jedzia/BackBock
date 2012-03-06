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
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Summary
    /// </summary>
    public static class Consts
    {
        public const string InputFolderA = @"C:\Temp\FolderA\**\*.*";
        public const string InputFolderRoot = @"C:\Temp\";
        public const string InputFolderRootAll = @"C:\Temp\**\*.*";
        public const string InputFileA = @"C:\Temp\raabe2.jpg";

        static string[] reservedMetadataNames = new string[] {
				"FullPath", "RootDir", "Filename", "Extension", "RelativeDir", "Directory",
				"RecursiveDir", "Identity", "ModifiedTime", "CreatedTime", "AccessedTime"};

    }

    public enum Metanames
    {
        FullPath = 0, RootDir, Filename, Extension, RelativeDir, Directory,
        RecursiveDir, Identity, ModifiedTime, CreatedTime, AccessedTime
    }

    public static class RMetadata
    {
        public const string FullPath = "FullPath";
        public const string RootDir = "RootDir";
        public const string Filename = "Filename";
        public const string Extension = "Extension";
        public const string RelativeDir = "RelativeDir";
        public const string Directory = "Directory";
        public const string RecursiveDir = "RecursiveDir";
        public const string Identity = "Identity";
        public const string ModifiedTime = "ModifiedTime";
        public const string CreatedTime = "CreatedTime";
        public const string AccessedTime = "AccessedTime";

        private static readonly string[] reservedMetadataNames = new string[] {
				FullPath, RootDir, Filename, Extension, RelativeDir, Directory,
				RecursiveDir, Identity, ModifiedTime, CreatedTime, AccessedTime};

        public static IList<string> ReservedMetadataNames
        {
            get
            {
                return reservedMetadataNames;
            }
        }
        
        /*/// <summary>
        /// Gets or sets the <see cref="T:System.Object"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <value>The <see cref="T:System.Object"/> at the specified index.</value>
        public static object this[Metanames index]
        {
            get
            {
                return Meta(index);
            }
        }*/

        public static string Meta(Metanames name)
        {
            //var l = new List<string>(reservedMetadataNames);
            //var dic = new Dictionary<string, string>();
            //var rd = new ReadOnlyCollection<string>(l);
            //var lu = l.ToLookup(e => e);
            return ReservedMetadataNames[(int)name];
        }

    }

    /// <summary>
    /// Parameter.
    /// </summary>
    [Serializable]
    internal class Parameter : IEquatable<Parameter>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>The Name.</value>
        internal string Name { get; set; }
        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>The Value.</value>
        internal string Value { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        internal Parameter() { }
        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="Name">The Name</param>
        /// <param name="Value">The Value</param>
        internal Parameter(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            Parameter other = obj as Parameter;
            if (other != null)
                return Equals(other);
            return false;
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Parameter other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
              Name == other.Name &&
              Value == other.Value;
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = " + Name + ";");
            sb.Append("Value = " + Value);
            return sb.ToString();
        }
        #endregion
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

        private static string StartupPath
        {
            get
            {
                var basedir = AppDomain.CurrentDomain.BaseDirectory;
                return basedir;
            }
        }

        private static string EngineTargetsFile
        {
            get
            {
                var file = Path.Combine(StartupPath, "Microsoft.Common.Tasks");
                return file;
            }
        }

        static void Main(string[] args)
        {
            //CheckBackupTask();
            /*var task = new Zip();
            task.TaskAction = "Create";
            task.CompressFiles = new[] { new TaskItem(Consts.InputFileA), };
            task.ZipFileName = new TaskItem(@"D:\TestZip.zip");
            TestB(task);*/

            var paths = new[] { Consts.InputFolderA };
            //TestA(typeof(Backup), "SourceFiles", paths);
            var parameters = new[] { 
                new Parameter("TaskAction", "Create"),
                new Parameter("ZipFileName", @"D:\TestZip.zip"),
            };
            //TestA(typeof(Zip), "CompressFiles", paths, parameters);

            paths = new[] { Consts.InputFolderA };
            //TestA(typeof(Backup), "SourceFiles", paths);
            parameters = new Parameter[] { 
                //new Parameter("TaskAction", "Create"),
                //new Parameter("ZipFileName", @"D:\TestZip.zip"),
            };
            TestB("CreateItem", "SourceFiles", paths, parameters);

        }

        /// <summary>
        /// The logger of this instance.
        /// </summary>
        private static Logger myLogger;

        /// <summary>
        /// Gets or sets the logger of this instance.
        /// </summary>
        /// <value>The logger of this instance.</value>
        private static Logger MyLogger
        {
            get
            {
                if (myLogger == null)
                {
                    myLogger = new Logger();
                }
                return myLogger;
            }

        }

        private static void TestA(Type taskType, string sourceParameter,
            IEnumerable<string> paths, IEnumerable<Parameter> parameters)
        {

            //var proj2 = new Project();


            var engine = new Engine(StartupPath);
            engine.RegisterLogger(MyLogger);

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
            inclEle.Remove(inclEle.Length - 1, 1);
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

            // Set additional parameters.
            foreach (var item in parameters)
            {
                batask.SetParameterValue(item.Name, item.Value);
                MyLogger.Log(null, new BuildMessageEventArgs("Setting parameter " + item.Name +
                    " to " + item.Value, "", taskType.FullName, MessageImportance.Low));
            }

            var res = proj.Build("mainTarget");
            var str = PrettyPrintXml(proj.Xml);

        }

        private static void TestB(string taskType, string sourceParameter,
    IEnumerable<string> paths, IEnumerable<Parameter> parameters)
        {

            //var proj2 = new Project();


            var engine = new Engine(StartupPath);
            engine.RegisterLogger(MyLogger);

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
            inclEle.Remove(inclEle.Length - 1, 1);
            var include = inclEle.ToString();

            BuildItem cr = big.AddNewItem("FilesToZip", include);
            // cr.Exclude = "*.txt";
            #endregion

            var subtask = target.AddNewTask(taskType);
            subtask.SetParameterValue("Include", Consts.InputFolderRoot + @"**\*.txt");
            subtask.AddOutputItem("Include", "SubtaskOutput");


            Type btasktype = typeof(Info);
            proj.AddNewUsingTaskFromAssemblyName(btasktype.FullName, btasktype.Assembly.FullName);
            var batask = target.AddNewTask(btasktype.Name);
            //batask.SetParameterValue("SourceFiles", @"C:\Temp\company.xmi");
            batask.SetParameterValue(sourceParameter, @"@(FilesToZip)");
            var pars = batask.GetParameterNames();

            // Set additional parameters.
            foreach (var item in parameters)
            {
                batask.SetParameterValue(item.Name, item.Value);
                MyLogger.Log(null, new BuildMessageEventArgs("Setting parameter " + item.Name +
                    " to " + item.Value, "", taskType, MessageImportance.Low));
            }

            var batask2 = target.AddNewTask(btasktype.Name);
            //batask.SetParameterValue("SourceFiles", @"C:\Temp\company.xmi");
            batask2.SetParameterValue(sourceParameter,
                "%(SubtaskOutput." + RMetadata.Filename + "): " +
                "%(SubtaskOutput." + RMetadata.CreatedTime + ")");


            var res = proj.Build("mainTarget");
            var str = PrettyPrintXml(proj.Xml);

            NewMethod(proj);
        }

        private static void NewMethod(Project proj)
        {
            //IEnumerable<string> p;
            
            var props = proj.EvaluatedProperties.OfType<BuildProperty>();
            var lst = props.ToDictionary(e => e.Name);
            //props.OfType
            //var ee = proj.EvaluatedProperties.GetEnumerator().Where(ee => true == true);
        }

        private static string PrettyPrintXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Normalize();

            TextWriter wr = new StringWriter();
            doc.Save(wr);
            var str = wr.ToString();
            return str;
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
