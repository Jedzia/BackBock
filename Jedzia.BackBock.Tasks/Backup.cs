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

    /// <summary>
    /// A simple backup task.
    /// </summary>
    [DisplayName("BackupTask")]
    public class Backup : TaskExtension
    {
        // Fields

        #region Fields

        private ITaskItem[] copiedFiles;
        private ITaskItem[] destinationFiles;
        private ITaskItem destinationFolder;
        private bool overwriteReadOnlyFiles;
        private bool skipUnchangedFiles;
        private ITaskItem[] sourceFiles;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Backup"/> class.
        /// </summary>
        public Backup()
        {
            this.SourceFiles = new TaskItem[5];
            //FilesXX = new List<object>();
            for (int index = 0; index < 5; index++)
            {
                var taskItem = new TaskItem("C:\\tmp\\My0" + index + ".txt");
                //taskItem.ItemSpec = @"C:\tmp\txt";
                this.SourceFiles[index] = taskItem;
                //FilesXX.Add(taskItem);
            }
        }

        #endregion

        // Properties

        #region Properties

        /// <summary>
        /// Gets the items that were successfully copied.
        /// </summary>
        [Output]
        public ITaskItem[] CopiedFiles
        {
            get
            {
                return this.copiedFiles;
            }
        }

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
        /// Gets the name of the Task.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Backup";
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether to overwrite files even if they are marked as read only files.
        /// </summary>
        /// <value>
        /// <c>true</c> if the <see cref="Backup"/> task should overwrite files even if they are marked as read only files; otherwise, <c>false</c>.
        /// </value>
        public bool OverwriteReadOnlyFiles
        {
            get
            {
                return this.overwriteReadOnlyFiles;
            }
            set
            {
                this.overwriteReadOnlyFiles = value;
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the Copy task should skip 
        /// the copying of files that are unchanged between the source and destination.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Backup"/> task should skip the copying of files that
        ///  are unchanged between the source and destination; otherwise, <c>false</c>.
        /// </value>
        public bool SkipUnchangedFiles
        {
            get
            {
                return this.skipUnchangedFiles;
            }
            set
            {
                this.skipUnchangedFiles = value;
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
            return this.Execute(this.CopyFileWithLogging);
        }

        internal bool Execute(CopyFile copyFile)
        {
            if ((this.sourceFiles == null) || (this.sourceFiles.Length == 0))
            {
                this.destinationFiles = new TaskItem[0];
                this.copiedFiles = new TaskItem[0];
                return true;
            }
            if (!this.ValidateInputs() || !this.InitializeDestinationFiles())
            {
                return false;
            }
            bool flag = true;
            var list = new ArrayList();
            for (int i = 0; i < this.sourceFiles.Length; i++)
            {
                if (this.DoCopyIfNecessary(this.sourceFiles[i].ItemSpec, this.destinationFiles[i].ItemSpec, copyFile))
                {
                    this.sourceFiles[i].CopyMetadataTo(this.destinationFiles[i]);
                    list.Add(this.destinationFiles[i]);
                }
                else
                {
                    flag = false;
                }
            }
            this.copiedFiles = (ITaskItem[])list.ToArray(typeof(ITaskItem));
            return flag;
        }

        private static bool IsMatchingSizeAndTimeStamp(string sourceFile, string destinationFile)
        {
            var info = new FileInfo(sourceFile);
            var info2 = new FileInfo(destinationFile);
            if (!info2.Exists)
            {
                return false;
            }
            if (info.LastWriteTime != info2.LastWriteTime)
            {
                return false;
            }
            if (info.Length != info2.Length)
            {
                return false;
            }
            return true;
        }


        private bool CopyFileWithLogging(string sourceFile, string destinationFile)
        {
            if (Directory.Exists(destinationFile))
            {
                Log.LogErrorWithCodeFromResources(
                    "Copy.DestinationIsDirectory", new object[] { sourceFile, destinationFile });
                return false;
            }
            if (Directory.Exists(sourceFile))
            {
                Log.LogErrorWithCodeFromResources("Copy.SourceIsDirectory", new object[] { sourceFile });
                return false;
            }
            string directoryName = Path.GetDirectoryName(destinationFile);
            if (((!string.IsNullOrEmpty(directoryName))) && !Directory.Exists(directoryName))
            {
                Log.LogMessageFromResources(
                    MessageImportance.Normal, "Copy.CreatesDirectory", new object[] { directoryName });
                Directory.CreateDirectory(directoryName);
            }
            if (this.overwriteReadOnlyFiles)
            {
                this.MakeFileWriteable(destinationFile, true);
            }
            Log.LogMessageFromResources(
                MessageImportance.Normal, "Copy.FileComment", new object[] { sourceFile, destinationFile });
            Log.LogMessageFromResources(MessageImportance.Low, "Shared.ExecCommand", new object[0]);
            Log.LogCommandLine(MessageImportance.Low, "copy /y \"" + sourceFile + "\" \"" + destinationFile + "\"");
            File.Copy(sourceFile, destinationFile, true);
            this.MakeFileWriteable(destinationFile, false);
            return true;
        }

        private bool DoCopyIfNecessary(string sourceFile, string destinationFile, CopyFile copyFile)
        {
            bool flag = true;
            try
            {
                if (this.skipUnchangedFiles && IsMatchingSizeAndTimeStamp(sourceFile, destinationFile))
                {
                    Log.LogMessageFromResources(
                        MessageImportance.Low,
                        "Copy.DidNotCopyBecauseOfFileMatch",
                        new object[] { sourceFile, destinationFile, "SkipUnchangedFiles", "true" });
                    return flag;
                }
                if (string.Compare(sourceFile, destinationFile, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    flag = copyFile(sourceFile, destinationFile);
                }
            }
            catch (PathTooLongException exception)
            {
                Log.LogErrorWithCodeFromResources(
                    "Copy.Error", new object[] { sourceFile, destinationFile, exception.Message });
                flag = false;
            }
            catch (IOException exception2)
            {
                if (this.PathsAreIdentical(sourceFile, destinationFile))
                {
                    return flag;
                }
                if (ExceptionHandling.NotExpectedException(exception2))
                {
                    throw;
                }
                Log.LogErrorWithCodeFromResources(
                    "Copy.Error", new object[] { sourceFile, destinationFile, exception2.Message });
                flag = false;
            }
            catch (Exception exception3)
            {
                if (ExceptionHandling.NotExpectedException(exception3))
                {
                    throw;
                }
                Log.LogErrorWithCodeFromResources(
                    "Copy.Error", new object[] { sourceFile, destinationFile, exception3.Message });
                flag = false;
            }
            return flag;
        }

        private bool InitializeDestinationFiles()
        {
            if (this.destinationFiles == null)
            {
                this.destinationFiles = new ITaskItem[this.sourceFiles.Length];
                for (int i = 0; i < this.sourceFiles.Length; i++)
                {
                    string str;
                    try
                    {
                        str = Path.Combine(
                            this.destinationFolder.ItemSpec, Path.GetFileName(this.sourceFiles[i].ItemSpec));
                    }
                    catch (ArgumentException exception)
                    {
                        Log.LogErrorWithCodeFromResources(
                            "Copy.Error",
                            new object[]
                                { this.sourceFiles[i].ItemSpec, this.destinationFolder.ItemSpec, exception.Message });
                        this.destinationFiles = new ITaskItem[0];
                        return false;
                    }
                    this.destinationFiles[i] = new TaskItem(str);
                    this.sourceFiles[i].CopyMetadataTo(this.destinationFiles[i]);
                }
            }
            return true;
        }

        private void MakeFileWriteable(string file, bool logActivity)
        {
            if (File.Exists(file) && (FileAttributes.ReadOnly == (File.GetAttributes(file) & FileAttributes.ReadOnly)))
            {
                if (logActivity)
                {
                    Log.LogMessageFromResources(
                        MessageImportance.Low, "Copy.RemovingReadOnlyAttribute", new object[] { file });
                }
                File.SetAttributes(file, FileAttributes.Normal);
            }
        }

        private bool PathsAreIdentical(string source, string destination)
        {
            string fullPath = Path.GetFullPath(source);
            string strB = Path.GetFullPath(destination);
            return (0 == string.Compare(fullPath, strB, StringComparison.OrdinalIgnoreCase));
        }

        private bool ValidateInputs()
        {
            if ((this.destinationFiles == null) && (this.destinationFolder == null))
            {
                Log.LogErrorWithCodeFromResources(
                    "Copy.NeedsDestination", new object[] { "DestinationFiles", "DestinationDirectory" });
                return false;
            }
            if ((this.destinationFiles != null) && (this.destinationFolder != null))
            {
                Log.LogErrorWithCodeFromResources(
                    "Copy.ExactlyOneTypeOfDestination", new object[] { "DestinationFiles", "DestinationDirectory" });
                return false;
            }
            if ((this.destinationFiles != null) && (this.destinationFiles.Length != this.sourceFiles.Length))
            {
                Log.LogErrorWithCodeFromResources(
                    "General.TwoVectorsMustHaveSameLength",
                    new object[]
                        { this.destinationFiles.Length, this.sourceFiles.Length, "DestinationFiles", "SourceFiles" });
                return false;
            }
            return true;
        }
    }
}