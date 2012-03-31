namespace Jedzia.BackBock.Model.Data
{
    using System;
    using System.ComponentModel;
    using Jedzia.BackBock.Model.Tasks;
    using Jedzia.BackBock.Model.Util;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Backup item Domain Object. Represents a single backup job.
    /// </summary>
    public partial class BackupItemType
    {
        // private ITaskService taskProvider;

        // private LogHelper logger;
        #region Fields

        /// <summary>
        /// Last <see cref="TaskSetupEngine"/> executed.
        /// </summary>
        private TaskSetupEngine tse;

        #endregion

        /*public ITaskService TaskProvider
        {
            get
            {
                if (this.taskProvider == null)
                {
                    try
                    {
                        //taskProvider = ApplicationContext.TaskService;
                        //taskProvider = new TaskProvider;
                        throw new NotImplementedException("implement a TaskProvider");
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("TaskProvider fucking: " + ex.ToString());
                        throw;
                    }
                }
                return this.taskProvider;
            }

            internal set
            {
                this.taskProvider = value;
            }
        }*/

        /// <summary>
        /// Modifies the task data.
        /// </summary>
        /// <param name="log">The logging callback.</param>
        /// <param name="taskAction">The action to perform with the new task. A
        /// return value of <c>false</c> indicates that the modification shouldn't
        /// persisted.</param>
        public void ModifyTaskData(Action<string> log, Func<ITask, bool?> taskAction)
        {
            // ITaskService taskProvider = TaskContext.Default;
            var logger = new LogHelper(log);
            using (this.tse = new TaskSetupEngine(logger, this.Path))
            {
                var task = this.tse.InitTaskEditor(this.Task.TypeName, this.Task.AnyAttr);

                // var task = InitTaskEditor(this.taskProvider);
                if (taskAction != null)
                {
                    var result = taskAction(task);
                    if ((!result.HasValue || !result.Value) && log != null)
                    {
                        log("Task modification canceled");
                    }
                }

                // this.Task.TaskInstance = task;
                // var wnd = ControlRegistrator.GetInstanceOfType<Window>(WindowTypes.TaskEditor);
                // var wnd = ApplicationContext.TaskWizardProvider.GetTaskEditor();
                // wnd.DataContext = this;
                // wnd.DataContext = task;
                // this.Task.PropertyChanged += Task_PropertyChanged;
                // var result = wnd.ShowDialog();
                // this.Task.PropertyChanged -= Task_PropertyChanged;
                this.tse.AfterTask(task, this.Task.AnyAttr);
            }
        }

        /// <summary>
        /// Runs the task represented by this instance.
        /// </summary>
        /// <param name="log">The logging callback.</param>
        public void RunTask(Action<string> log)
        {
            // ITaskService taskProvider = TaskContext.Default;
            if (!this.IsEnabled)
            {
                return;
            }

            try
            {
                var logger = new LogHelper(log);
                this.tse = new TaskSetupEngine(logger, this.Path);
                {
                    // using (var tse = new TaskSetupEngine(taskProvider, logger, this.Path, this.Task))
                    var bg = new BackgroundWorker();
                    bg.DoWork += (args, e) =>
                                     {
                                         var success = this.tse.ExecuteTask(this.Task.TypeName, this.Task.AnyAttr);
                                         if (log != null)
                                         {
                                             log("Finished Task: " + success);
                                         }
                                     };
                    bg.RunWorkerCompleted += (e, x) => this.tse.Dispose();
                    bg.RunWorkerAsync();
                }
            }
            catch (Exception e)
            {
                if (log != null)
                {
                    log("Exception: " + e);
                }
            }
        }

        /// <summary>
        /// Updates the task data during a call to <see cref="RunTask"/>.
        /// </summary>
        /// <param name="log">The logging callback.</param>
        /// <returns>An updated task from the changed data.</returns>
        /// <exception cref="InvalidOperationException"><see cref="TaskSetupEngine"/> is disposed.</exception>
        public ITask UpdateTaskData(Action<string> log)
        {
            if (this.tse == null || this.tse.IsDisposed)
            {
                throw new InvalidOperationException("TaskSetupEngine is disposed.");
            }

            return this.tse.InitTaskEditor(this.Task.TypeName, this.Task.AnyAttr);
        }
    }
}