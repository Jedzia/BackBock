// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskSetupEngine.cs" company="EvePanix">
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using Jedzia.BackBock.Model.Data;
    using Jedzia.BackBock.Tasks;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Tasks;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Helper class to setup <see cref="ITask"/>'s from model data.
    /// </summary>
    internal sealed class TaskSetupEngine : IDisposable
    {
        #region Fields

        private const string allFilesPattern = "*.*";
        private const string fullrecursivePattern = @"\" + recursivePattern + @"\" + allFilesPattern;
        private const string recursivePattern = "**";
        private readonly IBuildLogger buildLogger;

        /// <summary>
        /// The default build engine.
        /// </summary>
        private Engine buildEngine;
        private ITaskComposerFactory defaultTaskComposerBuilder;

        private ITask taskInWork;
        private ITaskService taskService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSetupEngine"/> class.
        /// </summary>
        /// <param name="paths">The paths to setup the task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="paths" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="TaskContext"/>.Default.TaskService delivered no valid ITaskService.</exception>
        public TaskSetupEngine(IEnumerable<PathDataType> paths)
            : this((ILogger)null, paths)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSetupEngine"/> class.
        /// </summary>
        /// <param name="logger">The logger used by this instance. Can be <c>null</c>.</param>
        /// <param name="paths">The paths to setup the task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="paths" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="TaskContext"/>.Default.TaskService delivered no valid ITaskService.</exception>
        public TaskSetupEngine(ILogger logger, IEnumerable<PathDataType> paths)
            : this(new BuildLogger(logger), paths)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSetupEngine"/> class.
        /// </summary>
        /// <param name="buildLogger">The build logger.</param>
        /// <param name="paths">The paths to setup the task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="paths"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="TaskContext"/>.Default.TaskService delivered no valid ITaskService.</exception>
        public TaskSetupEngine(
            IBuildLogger buildLogger,
            IEnumerable<PathDataType> paths)
        {
            if (buildLogger == null)
            {
                throw new ArgumentNullException("buildLogger");
            }
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            // this.taskService = taskService;
            this.taskService = TaskContext.Default.TaskService;
            if (this.taskService == null)
            {
                throw new InvalidOperationException("The TaskContext.Default.TaskService delivered no valid ITaskService.");
            }

            this.Paths = paths;
            this.buildLogger = buildLogger;

            this.buildLogger.LogBuildMessage(this, GetType().Name, "Initialized");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default build engine.
        /// </summary>
        /// <value>The default build engine.</value>
        public Engine DefaultBuildEngine
        {
            get
            {
                if (this.buildEngine == null)
                {
                    return new Engine(@"D:\E\Projects\CSharp\BackBock\Jedzia.BackBock.Application\bin\Debug");
                }

                return this.buildEngine;
            }

            set
            {
                this.buildEngine = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the default task composer builder used by this instance.
        /// </summary>
        /// <value>The default task composer builder used by this instance.</value>
        public ITaskComposerFactory DefaultTaskComposerBuilder
        {
            get
            {
                if (this.defaultTaskComposerBuilder == null)
                {
                    return new TaskComposerFactory();
                }

                return this.defaultTaskComposerBuilder;
            }

            set
            {
                this.defaultTaskComposerBuilder = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets or sets the paths that are processed.
        /// </summary>
        /// <value>
        /// The paths that are processed.
        /// </value>
        private IEnumerable<PathDataType> Paths { get; set; }

        #endregion

        /// <summary>
        /// Work after the task was modified.
        /// </summary>
        /// <param name="task">The task, that was modified.</param>
        /// <param name="taskAttributes">The list of task attributes to serialize the task data to.</param>
        public void AfterTask(ITask task, ICollection<XmlAttribute> taskAttributes)
        {
            // var task = taskInWork;
            if (task == null)
            {
                return;
            }

            if (task is Backup)
            {
                // var btask = (Backup)task;
                // btask.SourceFiles = new[] { new TaskItem(@"C:\Fuck\der.txt") };
            }

            try
            {
                /*var prj = new Project(Engine.GlobalEngine, "3.5");
                Target target = prj.Targets.AddNewTarget("target");
                var te = new TaskEngine(prj);
                var parameters = new Dictionary<string, string>();
                te.Prepare(task, target.TargetElement, parameters, task.GetType());
                te.PublishOutput();*/
                var properties = task.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                    | BindingFlags.IgnoreCase);
                foreach (PropertyInfo pi in properties)
                {
                    bool isRequired = pi.IsDefined(typeof(RequiredAttribute), false);
                    bool isOutput = pi.IsDefined(typeof(OutputAttribute), false);
                    Type propType = pi.PropertyType;
                    if (propType.IsArray)
                    {
                        propType = propType.GetElementType();
                    }

                    if (!propType.IsPrimitive && propType != typeof(string) && propType != typeof(ITaskItem))
                    {
                        continue;
                    }

                    if (!isOutput)
                    {
                        var val = pi.GetValue(task, null);
                        if (val != null)
                        {
                            if (pi.PropertyType.IsArray)
                            {
                                continue;
                            }

                            var converter = TypeDescriptor.GetConverter(val.GetType());
                            var lattr = SelectMatchingAttribute(taskAttributes, pi.Name);
                            if (lattr != null)
                            {
                                // TaskItem x = new TaskItem();
                                var xxxy = val.GetType();
                                if (val == null)
                                {
                                    // Todo: default value exclusion.
                                }

                                lattr.Value = (string)converter.ConvertTo(val, typeof(string));
                            }
                            else
                            {
                                var xdoc = new XmlDocument();
                                var attr = xdoc.CreateAttribute(pi.Name);
                                if (pi.PropertyType.Name == "ITaskItem")
                                {
                                }

                                attr.Value = (string)converter.ConvertTo(val, typeof(string));
                                taskAttributes.Add(attr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.buildLogger.LogBuildMessage(this, GetType().Name, ex.Message);
            }
        }

        /*private void LogError(string message)
        {
            eventSource.FireErrorRaised(this,
                new BuildErrorEventArgs(
                    message,
                    "",
                    this.GetType().Name,
                    MessageImportance.Low
                    ));
        }*/

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="InvalidOperationException">A <see cref="TaskSetupEngine"/> cannot dispose twice.</exception>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                throw new InvalidOperationException("A TaskSetupEngine cannot dispose twice.");
            }

            this.taskService = null;
            this.Paths = null;
            this.taskInWork = null;
            this.IsDisposed = true;
        }

        /// <summary>
        /// Executes a task specified by a string with the specified parameters.
        /// </summary>
        /// <param name="taskTypeName">Name of the task type.</param>
        /// <param name="taskAttributes">The list of Xml attributes with the parameters of the task.</param>
        /// <returns><c>true</c> if the operation succeeds.</returns>
        public bool ExecuteTask(string taskTypeName, IEnumerable<XmlAttribute> taskAttributes)
        {
            var task = this.taskService[taskTypeName];

            var engine = this.DefaultBuildEngine;
            // Todo: check if this is detached, later.
            buildLogger.RegisterLogger(engine);

            var sourceParameter = string.Empty;

            // Todo: put this task generation extra.);
            if (task is Backup)
            {
                // var btask = (Backup)task;
                /*for (int index = 0; index < this.Paths.Count; index++)
                {
                    var item = this.Paths[index];
                }*/
                sourceParameter = "SourceFiles";
            }
            else if (task is Zip)
            {
                sourceParameter = "CompressFiles";
            }

            var proj = engine.CreateNewProject();
            proj.DefaultToolsVersion = "3.5";
            var target = proj.Targets.AddNewTarget("mainTarget");

            var sourceParaIdent = "FilesToZip";
            //var big = proj.AddNewItemGroup();
            var igComp = DefaultTaskComposerBuilder.CreateItemGroupComposer();
            var big = igComp.GenerateBuildItemGroup(proj, this.Paths, sourceParaIdent);

            var taskComp = DefaultTaskComposerBuilder.CreateTaskComposer(proj, task.GetType(), this.buildLogger);
            var batask = taskComp.CreateNewTaskOnTarget(target, sourceParaIdent, sourceParameter);
            taskComp.SetParametersOnCreatedTask(taskAttributes);

            var result = proj.Build("mainTarget");
            var str = PrettyPrintXml(proj.Xml);

            return result;
        }


        // private IBuildEngine buildEngine;

        /*public IBuildEngine BuildEngine
        {
            get
            {
                if (this.buildEngine == null)
                {
                    //this.buildEngine = new SimpleBuildEngine(LogMessageEvent);
                    var vmb = new ViewModelBuildEngine(this.MessengerInstance);
                    vmb.Enabled = true;
                    this.buildEngine = vmb;
                }
                return buildEngine;
            }
        }*/

        /// <summary>
        /// Prepares a task for display with an editor.
        /// </summary>
        /// <param name="taskTypeName">Name of the task type to instantiate.</param>
        /// <param name="taskAttributes">A list with the task attributes data.</param>
        /// <returns>A new task of the specified type, initialized with the data from the
        /// <paramref name="taskAttributes"/> list of Xml attributes.</returns>
        public ITask InitTaskEditor(string taskTypeName, IEnumerable<XmlAttribute> taskAttributes)
        {
            // var taskService = SimpleIoc.Default.GetInstance<ITaskService>();
            var task = this.taskService[taskTypeName];
            if (task == null)
            {
                return null;
            }

            foreach (var item in taskAttributes)
            {
                var taskType = task.GetType();
                var property = taskType.GetProperty(item.Name);
                if (property == null)
                {
                    // Log property not found.
                    continue;
                }

                if (property.PropertyType.IsArray)
                {
                    // Log skipping array type.
                    continue;
                }

                if (property != null)
                {
                    object val = item.Value;
                    if (property.PropertyType.Name == "ITaskItem")
                    {
                        val = new TaskItem(item.Value);
                    }
                    else
                    {
                        // try
                        // {
                        val = Convert.ChangeType(item.Value, property.PropertyType);

                        // }
                        // catch (Exception ex)
                        // {
                        // }
                    }

                    property.SetValue(task, val, null);
                }
            }

            // var dst = this.Task.data.AnyAttr.Where((e) => e.Name == "DestinationFolder").FirstOrDefault();
            /*if (dst != null)
            {
                var val = dst.Value;
            }*/
            this.PrepareTask(task);
            this.taskInWork = task;
            return task;

            // var str = XamlSerializer.Save(task);
            // SerializeTest(task);
        }

        /// <summary>
        /// Pretty print XML data.
        /// </summary>
        /// <param name="xml">The string containing valid XML data.</param>
        /// <returns>The xml data in idented and justified form.</returns>
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


        /// <summary>
        /// Selects the first matching Xml attribute of the specified list.
        /// </summary>
        /// <param name="attributes">The list of Xml attribute.</param>
        /// <param name="name">The name of the Xml attribute to select.</param>
        /// <returns>The first matching Xml attribute of the specified name.</returns>
        private static XmlAttribute SelectMatchingAttribute(IEnumerable<XmlAttribute> attributes, string name)
        {
            var lattr = attributes.Where(e => e.Name == name).FirstOrDefault();
            return lattr;
        }


        /// <summary>
        /// Prepares the 'SourceFiles', etc., <see cref="TaskItem"/> data.
        /// </summary>
        /// <param name="task">The task to prepare.</param>
        /// <remarks>
        /// This is experimental. Uncommenting the "cr.Execute()" line below, fills the
        /// SourceFiles property of a Backup-Task in the editor with the evaluated path data. 
        /// A <see cref="CreateItem"/> task is used to mimic the work of a project build engine.
        /// </remarks>
        private void PrepareTask(ITask task)
        {
            // Todo: put this task generation extra.
            /*for (int index = 0; index < this.Paths.Count; index++)
            {
                var item = this.Paths[index];
            }*/
            // int xasd = 0;
            var result = this.Paths.Select(
                e =>
                {
                    var cr = new CreateItem();
                    if (e.Inclusion.Count > 0)
                    {
                        cr.Include = e.Inclusion.Select(
                            t =>
                            {
                                if (e.Path.EndsWith("\\"))
                                {
                                    return new TaskItem(e.Path + t.Pattern);
                                }
                                else
                                {
                                    return new TaskItem(e.Path + "\\" + t.Pattern);
                                }
                            }

                            ).ToArray();
                    }
                    else
                    {
                        ITaskItem taskItem;
                        var finfo = new FileInfo(e.Path);
                        if (Directory.Exists(e.Path))
                        {
                            // a directory
                            taskItem = new TaskItem(e.Path + "\\" + recursivePattern);
                        }
                        else
                        {
                            // a file
                            taskItem = new TaskItem(e.Path);
                        }

                        cr.Include = new[] { taskItem };
                    }

                    cr.Exclude =
                        e.Exclusion.Select((t) => { return new TaskItem(e.Path + "\\" + t.Pattern); }).ToArray();

                    // at the moment, do not execute the items.
                    // cr.Execute();
                    return cr;
                }

                );

            if (task is Backup)
            {
                var btask = (Backup)task;

                // var res = result.ToArray();
                var includes = result.SelectMany((e) => e.Include);
                btask.SourceFiles = includes.ToArray();

                // btask.DestinationFolder = new TaskItem(@"C:\tmp\%(RecursiveDir)");
                // var itemsByType = new Hashtable();
                // foreach (var item in btask.SourceFiles)
                // {
                // itemsByType.Add(
                // }
                // var bla = ItemExpander.ItemizeItemVector(@"@(File)", null, itemsByType);
                // Todo: do i need the build engine ?
                // btask.BuildEngine = this.BuildEngine;
            }
        }
    }
}