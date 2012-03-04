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
    using Jedzia.BackBock.Tasks.Shared;

        /// <summary>
    /// A simple backup task.
    /// </summary>
    [DisplayName("Touch Task")]
    public class Touch : TaskExtension
    {
        /// <summary>
        /// Gets the name of the Task.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Touch";
            }
        }

        public override bool Execute()
        {
            throw new NotImplementedException();
            //return false;
        }
    }

    /// <summary>
    /// A simple backup task.
    /// </summary>
    [DisplayName("Backup Task")]
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
            return;
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


        private static ITaskItem[] ExpandWildcards(ITaskItem[] expand)
        {
            if (expand == null)
            {
                return null;
            }
            ArrayList list = new ArrayList();
            foreach (ITaskItem item in expand)
            {
                if (FileMatcher.HasWildcards(item.ItemSpec))
                {
                    foreach (string str in FileMatcher.GetFiles(item.ItemSpec))
                    {
                        TaskItem item2 = new TaskItem(item)
                        {
                            ItemSpec = str
                        };
                        FileMatcher.Result result = FileMatcher.FileMatch(item.ItemSpec, str);
                        if ((result.isLegalFileSpec && result.isMatch) && ((result.wildcardDirectoryPart != null) && (result.wildcardDirectoryPart.Length > 0)))
                        {
                            item2.SetMetadata("RecursiveDir", result.wildcardDirectoryPart);
                        }
                        list.Add(item2);
                    }
                }
                else
                {
                    list.Add(new TaskItem(item));
                }
            }
            return (ITaskItem[])list.ToArray(typeof(ITaskItem));
        }

        // Properties

        #region Properties

        /// <summary>
        /// Gets the items that were successfully copied.
        /// </summary>
        [Output]
        [Browsable(false)]
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
        /// Gets the name of the Task.
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
            /*if (this.SourceFiles == null)
            {
                this.sourceFiles = new TaskItem[0];
                return true;
            }
            this.SourceFiles = ExpandWildcards(this.SourceFiles);*/
            //this.Exclude = ExpandWildcards(this.Exclude);
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
                // Todo: commented out for testing.
                // Directory.CreateDirectory(directoryName);
            }
            if (this.overwriteReadOnlyFiles)
            {
                this.MakeFileWriteable(destinationFile, true);
            }
            Log.LogMessageFromResources(
                MessageImportance.Normal, "Copy.FileComment", new object[] { sourceFile, destinationFile });
            Log.LogMessageFromResources(MessageImportance.Low, "Shared.ExecCommand", new object[0]);
            Log.LogCommandLine(MessageImportance.Low, "copy /y \"" + sourceFile + "\" \"" + destinationFile + "\"");
            // Todo: commented out for testing.
            // File.Copy(sourceFile, destinationFile, true);
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
                        ITaskItem srcFile = this.sourceFiles[i];
                        var destination = ExpandMetadata(srcFile, this.destinationFolder.ItemSpec);
                        str = Path.Combine( destination, Path.GetFileName(this.sourceFiles[i].ItemSpec));
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

        private static string ExpandMetadata(ITaskItem srcFile, string template)
        {
            //var ee2 = this.sourceFiles[i].GetAllCustomEvaluatedMetadata();
            foreach (string item in srcFile.MetadataNames)
            {
                var match = "%(" + item + ")";
                if (template.Contains(match))
                {
                    var meta = srcFile.GetMetadata(item);
                    template = template.Replace(match, meta);
                }
            }
            return template;
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
                // Todo: commented out for testing.
                // File.SetAttributes(file, FileAttributes.Normal);
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