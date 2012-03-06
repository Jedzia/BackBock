// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Tasks;
    using Microsoft.Build.Utilities;

    //using Jedzia.BackBock.Tasks.Shared;

    /// <summary>
    /// A simple backup task.
    /// </summary>
    [DisplayName("Info Task")]
    public class Info : TaskExtension
    {
        // Fields

        #region Fields

        private ITaskItem[] destinationFiles;
        private ITaskItem destinationFolder;
        private ITaskItem[] sourceFiles;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Backup"/> class.
        /// </summary>
        public Info()
        {
            //this.SourceFiles = new TaskItem[0];
            //this.DestinationFolder = new TaskItem();
        }

        #endregion


        // Properties

        #region Properties



        /// <summary>
        /// Gets or sets a list of files to copy the source files to.
        /// </summary>
        /// <value>
        /// The list of files to copy the source files to..
        /// </value>
        /// <remarks>The list of destination files is expected to be a one-to-one mapping with the list 
        /// specified in the <see cref="SourceFiles"/> property. That is, the first file specified in <see cref="SourceFiles"/> will
        ///  be copied to the first location specified in DestinationFiles, and so forth.</remarks>
        [Output]
        public ITaskItem[] DestinationFiles
        {
            get
            {
                return this.destinationFiles;
            }
            set
            {
                this.destinationFiles = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory to which you want to copy the files.
        /// </summary>
        /// <value>
        /// The directory to which you want to copy the files.
        /// </value>
        /// <remarks>The destination folder must be a directory, not a file. If the directory does not exist, it is created automatically.</remarks>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ITaskItem DestinationFolder
        {
            get
            {
                return this.destinationFolder;
            }
            set
            {
                this.destinationFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the files to copy.
        /// </summary>
        /// <value>
        /// The files to copy.
        /// </value>
        [Required]
        public ITaskItem[] SourceFiles
        {
            get
            {
                return this.sourceFiles;
            }
            set
            {
                this.sourceFiles = value;
            }
        }

        #endregion

        /// <summary>
        /// Executes a task.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the task executed successfully; otherwise, <c>false</c>.
        /// </returns>
        public override bool Execute()
        {
            /*if (this.SourceFiles == null)
            {
                this.sourceFiles = new TaskItem[0];
                return true;
            }
           
            if (this.DestinationFolder == null)
            {
                this.destinationFolder = new TaskItem();
            }*/
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs(
    "*** Running: " + this.GetType() + " ***", "", this.GetType().Name, MessageImportance.High));

            string dstFolder = "is empty";
            if (DestinationFolder != null)
            {
                dstFolder = DestinationFolder.ItemSpec;
            }

            BuildEngine.LogMessageEvent(new BuildMessageEventArgs(
                "DestinationFolder: " + dstFolder, "", this.GetType().Name, MessageImportance.High));

            if (SourceFiles != null)
            {
                foreach (var sourceFile in SourceFiles)
                {
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs(
                       "SourceFile: " + sourceFile.ItemSpec, "", this.GetType().Name, MessageImportance.High));
                }
            }
            else
            {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs(
                   "SourceFiles are null", "", this.GetType().Name, MessageImportance.High));
            }


            if (DestinationFiles != null)
            {
                foreach (var sourceFile in DestinationFiles)
                {
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs(
                       "DestinationFile: " + sourceFile.ItemSpec, "", this.GetType().Name, MessageImportance.High));
                }
            }
            else
            {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs(
                   "DestinationFiles are null", "", this.GetType().Name, MessageImportance.High));
            }

            string add = string.Empty;
            if (this.SourceFiles != null)
                add += " Total:" + this.SourceFiles.Length + " Files.";
            var msg = this.GetType().Name + " Finished: " + add;
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs(msg, "", "", MessageImportance.High));
            return true;
        }

    }
}