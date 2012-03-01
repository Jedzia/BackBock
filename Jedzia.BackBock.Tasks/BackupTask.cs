using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Xml;
using System.ComponentModel;
using System.Resources;
using System.IO;
using System.Security;
using System.Reflection;

namespace Jedzia.BackBock.Tasks
{

    internal static class ExceptionHandling
    {
        // Methods
        internal static bool IsCriticalException(Exception e)
        {
            return (((e is StackOverflowException) || (e is OutOfMemoryException)) || ((e is ExecutionEngineException) || (e is AccessViolationException)));
        }

        internal static bool NotExpectedException(Exception e)
        {
            return (((!(e is UnauthorizedAccessException) && !(e is ArgumentNullException)) && (!(e is PathTooLongException) && !(e is DirectoryNotFoundException))) && ((!(e is NotSupportedException) && !(e is ArgumentException)) && (!(e is SecurityException) && !(e is IOException))));
        }

        internal static bool NotExpectedReflectionException(Exception e)
        {
            return ((((!(e is TypeLoadException) && !(e is MethodAccessException)) && (!(e is MissingMethodException) && !(e is MemberAccessException))) && ((!(e is BadImageFormatException) && !(e is ReflectionTypeLoadException)) && (!(e is CustomAttributeFormatException) && !(e is TargetParameterCountException)))) && (((!(e is InvalidCastException) && !(e is AmbiguousMatchException)) && (!(e is InvalidFilterCriteriaException) && !(e is TargetException))) && (!(e is MissingFieldException) && NotExpectedException(e))));
        }
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RequiredAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class OutputAttribute : Attribute
    {
    }


    [DebuggerDisplay("BuildItem (Name = { Name }, Include = { Include }, FinalItemSpec = { FinalItemSpec }, Condition = { Condition })")]
    [DisplayName("BuildItem")]
    public class BuildItem
    {
        private string finalItemSpecEscaped;
        private Hashtable itemSpecModifiers;
        private string recursivePortionOfFinalItemSpecDirectory;

        public BuildItem(string itemName, string itemInclude)
            : this(null, itemName, itemInclude)
        {
        }
        internal BuildItem(XmlDocument ownerDocument, string itemName, string itemInclude)
            : this(ownerDocument, itemName, itemInclude, true)
        {
        }

        private BuildItem(XmlDocument ownerDocument, string itemName, string itemInclude, bool createCustomMetadataCache)
        {
            //this.finalItemSpecEscaped = string.Empty;
            this.finalItemSpecEscaped = itemInclude;
        }

        public string FinalItemSpec
        {
            get
            {
                //return EscapingUtilities.UnescapeAll(this.FinalItemSpecEscaped);
                return this.FinalItemSpecEscaped;
            }
        }
        internal string FinalItemSpecEscaped
        {
            get
            {
                return this.finalItemSpecEscaped;
            }
        }

        internal void SetFinalItemSpecEscaped(string finalItemSpecValueEscaped)
        {
            this.finalItemSpecEscaped = finalItemSpecValueEscaped;
            this.itemSpecModifiers = null;
            this.recursivePortionOfFinalItemSpecDirectory = null;
        }


    }

    [DisplayName("TaskItem")]
    public /*internal*/ sealed class TaskItem : MarshalByRefObject, ITaskItem
    {
        private XmlElement mainProjectElement;
        private XmlDocument mainProjectEntireContents;

        public TaskItem()
        {
            // Todo: after XamlReader testing, make this internal.
            //ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");

            this.mainProjectEntireContents = new XmlDocument();
            this.mainProjectElement = this.mainProjectEntireContents.CreateElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
            this.mainProjectEntireContents.AppendChild(this.mainProjectElement);


            this.item = new BuildItem(null, string.Empty);
        }

        internal TaskItem(string itemSpec)
        {
            //ErrorUtilities.VerifyThrow(itemSpec != null, "Need to specify item-spec.");
            this.item = new BuildItem(null, itemSpec);
        }


        // Fields
        private BuildItem item;

        #region ITaskItem Members

        // Properties
        public string ItemSpec
        {
            get
            {
                return this.item.FinalItemSpec;
            }
            set
            {
                //ErrorUtilities.VerifyThrowArgumentNull(value, "ItemSpec");
                //this.item.SetFinalItemSpecEscaped(EscapingUtilities.Escape(value));
                this.item.SetFinalItemSpecEscaped(value);
            }
        }

        public System.Collections.ICollection MetadataNames
        {
            get { throw new NotImplementedException(); }
        }

        public int MetadataCount
        {
            get { throw new NotImplementedException(); }
        }

        public string GetMetadata(string metadataName)
        {
            throw new NotImplementedException();
        }

        public void SetMetadata(string metadataName, string metadataValue)
        {
            throw new NotImplementedException();
        }

        public void RemoveMetadata(string metadataName)
        {
            throw new NotImplementedException();
        }

        public void CopyMetadataTo(ITaskItem destinationItem)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IDictionary CloneCustomMetadata()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public abstract class Task : ITask
    {
        public abstract bool Execute();


        #region ITask Members

        public abstract string Name
        {
            get;
        }

        #endregion
    }

    public class TaskLoggingHelper : MarshalByRefObject
    {
        public virtual string FormatResourceString(string resourceName, params object[] args)
        {
            return resourceName;
        }
        public void LogErrorWithCodeFromResources(string messageResourceName, params object[] messageArgs)
        {
            //this.LogErrorWithCodeFromResources(null, null, 0, 0, 0, 0, messageResourceName, messageArgs);
        }

        public void LogMessageFromResources(MessageImportance importance, string messageResourceName, params object[] messageArgs)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(messageResourceName, "messageResourceName");
            //this.LogMessage(importance, this.FormatResourceString(messageResourceName, messageArgs), new object[0]);
        }
        public void LogCommandLine(MessageImportance importance, string commandLine)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(commandLine, "commandLine");
            //TaskCommandLineEventArgs e = new TaskCommandLineEventArgs(commandLine, this.TaskName, importance);
            //ErrorUtilities.VerifyThrowInvalidOperation(this.BuildEngine != null, "LoggingBeforeTaskInitialization", e.Message);
            //this.BuildEngine.LogMessageEvent(e);
        }

 

 

    }

    public class TaskLoggingHelperExtension : TaskLoggingHelper
    {
        // Fields
        private ResourceManager taskSharedResources;

        // Methods
        private TaskLoggingHelperExtension()
           // : base(null)
        {
        }

        public TaskLoggingHelperExtension(ITask taskInstance, ResourceManager primaryResources, ResourceManager sharedResources, string helpKeywordPrefix)
            //: base(taskInstance)
        {
            //base.TaskResources = primaryResources;
            //this.TaskSharedResources = sharedResources;
            //base.HelpKeywordPrefix = helpKeywordPrefix;
        }

        public override string FormatResourceString(string resourceName, params object[] args)
        {
            //ErrorUtilities.VerifyThrowArgumentNull(resourceName, "resourceName");
            //ErrorUtilities.VerifyThrowInvalidOperation(base.TaskResources != null, "Shared.TaskResourcesNotRegistered", base.TaskName);
            //ErrorUtilities.VerifyThrowInvalidOperation(this.TaskSharedResources != null, "Shared.TaskResourcesNotRegistered", base.TaskName);
            //string unformatted = base.TaskResources.GetString(resourceName, CultureInfo.CurrentUICulture);
            //if (unformatted == null)
            //{
            //    unformatted = this.TaskSharedResources.GetString(resourceName, CultureInfo.CurrentUICulture);
           // }
            //ErrorUtilities.VerifyThrowArgument(unformatted != null, "Shared.TaskResourceNotFound", resourceName, base.TaskName);
            //return this.FormatString(unformatted, args);
            return resourceName;
        }

        // Properties
        public ResourceManager TaskSharedResources
        {
            get
            {
                return this.taskSharedResources;
            }
            set
            {
                this.taskSharedResources = value;
            }
        }
    }
    [Serializable]
    public enum MessageImportance
    {
        High,
        Normal,
        Low
    }


    public abstract class TaskExtension : Task
    {
        // Fields
        private TaskLoggingHelperExtension logExtension;

        // Methods
        internal TaskExtension()
            //: base(AssemblyResources.PrimaryResources, "MSBuild.")
        {
            //this.logExtension = new TaskLoggingHelperExtension(this, AssemblyResources.PrimaryResources, AssemblyResources.SharedResources, "MSBuild.");
        }

        // Properties
        public TaskLoggingHelper Log
        {
            get
            {
                return this.logExtension;
            }
        }
    }

    internal delegate bool CopyFile(string source, string destination);

    [DisplayName("BackupTask")]
    public class BackupTask : TaskExtension
    {

        // Fields
        private ITaskItem[] copiedFiles;
        private ITaskItem[] destinationFiles;
        private ITaskItem destinationFolder;
        private bool overwriteReadOnlyFiles;
        private bool skipUnchangedFiles;
        private ITaskItem[] sourceFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BackupTask"/> class.
        /// </summary>
        public BackupTask()
        {
            SourceFiles = new TaskItem[5];
            //FilesXX = new List<object>();
            for (int index = 0; index < 5; index++)
            {
                var taskItem = new TaskItem(@"C:\tmp\My0" + index.ToString() + ".txt");
                //taskItem.ItemSpec = @"C:\tmp\txt";
                SourceFiles[index] = taskItem;
                //FilesXX.Add(taskItem);
            }
        }

        private bool ValidateInputs()
        {
            if ((this.destinationFiles == null) && (this.destinationFolder == null))
            {
                base.Log.LogErrorWithCodeFromResources("Copy.NeedsDestination", new object[] { "DestinationFiles", "DestinationDirectory" });
                return false;
            }
            if ((this.destinationFiles != null) && (this.destinationFolder != null))
            {
                base.Log.LogErrorWithCodeFromResources("Copy.ExactlyOneTypeOfDestination", new object[] { "DestinationFiles", "DestinationDirectory" });
                return false;
            }
            if ((this.destinationFiles != null) && (this.destinationFiles.Length != this.sourceFiles.Length))
            {
                base.Log.LogErrorWithCodeFromResources("General.TwoVectorsMustHaveSameLength", new object[] { this.destinationFiles.Length, this.sourceFiles.Length, "DestinationFiles", "SourceFiles" });
                return false;
            }
            return true;
        }

 

 

        // Properties
        [Output]
        public ITaskItem[] CopiedFiles
        {
            get
            {
                return this.copiedFiles;
            }
        }

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
        private void MakeFileWriteable(string file, bool logActivity)
        {
            if (File.Exists(file) && (FileAttributes.ReadOnly == (File.GetAttributes(file) & FileAttributes.ReadOnly)))
            {
                if (logActivity)
                {
                    base.Log.LogMessageFromResources(MessageImportance.Low, "Copy.RemovingReadOnlyAttribute", new object[] { file });
                }
                File.SetAttributes(file, FileAttributes.Normal);
            }
        }


        private bool CopyFileWithLogging(string sourceFile, string destinationFile)
        {
            if (Directory.Exists(destinationFile))
            {
                base.Log.LogErrorWithCodeFromResources("Copy.DestinationIsDirectory", new object[] { sourceFile, destinationFile });
                return false;
            }
            if (Directory.Exists(sourceFile))
            {
                base.Log.LogErrorWithCodeFromResources("Copy.SourceIsDirectory", new object[] { sourceFile });
                return false;
            }
            string directoryName = Path.GetDirectoryName(destinationFile);
            if (((directoryName != null) && (directoryName.Length > 0)) && !Directory.Exists(directoryName))
            {
                base.Log.LogMessageFromResources(MessageImportance.Normal, "Copy.CreatesDirectory", new object[] { directoryName });
                Directory.CreateDirectory(directoryName);
            }
            if (this.overwriteReadOnlyFiles)
            {
                this.MakeFileWriteable(destinationFile, true);
            }
            base.Log.LogMessageFromResources(MessageImportance.Normal, "Copy.FileComment", new object[] { sourceFile, destinationFile });
            base.Log.LogMessageFromResources(MessageImportance.Low, "Shared.ExecCommand", new object[0]);
            base.Log.LogCommandLine(MessageImportance.Low, "copy /y \"" + sourceFile + "\" \"" + destinationFile + "\"");
            File.Copy(sourceFile, destinationFile, true);
            this.MakeFileWriteable(destinationFile, false);
            return true;
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
                        str = Path.Combine(this.destinationFolder.ItemSpec, Path.GetFileName(this.sourceFiles[i].ItemSpec));
                    }
                    catch (ArgumentException exception)
                    {
                        base.Log.LogErrorWithCodeFromResources("Copy.Error", new object[] { this.sourceFiles[i].ItemSpec, this.destinationFolder.ItemSpec, exception.Message });
                        this.destinationFiles = new ITaskItem[0];
                        return false;
                    }
                    this.destinationFiles[i] = new TaskItem(str);
                    this.sourceFiles[i].CopyMetadataTo(this.destinationFiles[i]);
                }
            }
            return true;
        }

        private static bool IsMatchingSizeAndTimeStamp(string sourceFile, string destinationFile)
        {
            FileInfo info = new FileInfo(sourceFile);
            FileInfo info2 = new FileInfo(destinationFile);
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

        private bool PathsAreIdentical(string source, string destination)
        {
            string fullPath = Path.GetFullPath(source);
            string strB = Path.GetFullPath(destination);
            return (0 == string.Compare(fullPath, strB, StringComparison.OrdinalIgnoreCase));
        }

 

 
 

        private bool DoCopyIfNecessary(string sourceFile, string destinationFile, CopyFile copyFile)
        {
            bool flag = true;
            try
            {
                if (this.skipUnchangedFiles && IsMatchingSizeAndTimeStamp(sourceFile, destinationFile))
                {
                    base.Log.LogMessageFromResources(MessageImportance.Low, "Copy.DidNotCopyBecauseOfFileMatch", new object[] { sourceFile, destinationFile, "SkipUnchangedFiles", "true" });
                    return flag;
                }
                if (string.Compare(sourceFile, destinationFile, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    flag = copyFile(sourceFile, destinationFile);
                }
            }
            catch (PathTooLongException exception)
            {
                base.Log.LogErrorWithCodeFromResources("Copy.Error", new object[] { sourceFile, destinationFile, exception.Message });
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
                base.Log.LogErrorWithCodeFromResources("Copy.Error", new object[] { sourceFile, destinationFile, exception2.Message });
                flag = false;
            }
            catch (Exception exception3)
            {
                if (ExceptionHandling.NotExpectedException(exception3))
                {
                    throw;
                }
                base.Log.LogErrorWithCodeFromResources("Copy.Error", new object[] { sourceFile, destinationFile, exception3.Message });
                flag = false;
            }
            return flag;
        }



        #region ITask Members

        public override string Name
        {
            get { return "Backup"; }
        }

        public override bool Execute()
        {
            return this.Execute(new CopyFile(this.CopyFileWithLogging));
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
            ArrayList list = new ArrayList();
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


        #endregion
    }
}
