using Jedzia.BackBock.Tasks.Utilities;
using System.ComponentModel;
using System.Collections;
using Jedzia.BackBock.Tasks.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
namespace Jedzia.BackBock.Tasks
{
    /// <summary>
    /// Contains properties to help extend a task.
    /// </summary>
    public abstract class TaskExtension : Task
    {
        private readonly TaskLoggingHelperExtension logExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExtension"/> class.
        /// </summary>
        internal TaskExtension()
            : base(AssemblyResources.PrimaryResources, "MSBuild.")
        {
            this.logExtension = new TaskLoggingHelperExtension(this, AssemblyResources.PrimaryResources, AssemblyResources.SharedResources, "MSBuild.");
        }

        /// <summary>
        /// Gets an instance of a <see cref="TaskLoggingHelperExtension"/> containing task logging methods.
        /// </summary>
        [Browsable(false)]
        public new TaskLoggingHelper Log
        {
            get
            {
                return this.logExtension;
            }
        }
    }


    internal static class PropertyParser
    {
        // Methods
        internal static bool GetTable(TaskLoggingHelper log, string parameterName, string[] propertyList, out Hashtable propertiesTable)
        {
            propertiesTable = null;
            if (propertyList != null)
            {
                propertiesTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
                foreach (string str in propertyList)
                {
                    string str2 = string.Empty;
                    string str3 = string.Empty;
                    int index = str.IndexOf('=');
                    if (index != -1)
                    {
                        str2 = str.Substring(0, index).Trim();
                        str3 = str.Substring(index + 1).Trim();
                    }
                    if (str2.Length == 0)
                    {
                        if (log != null)
                        {
                            log.LogErrorWithCodeFromResources("General.InvalidPropertyError", new object[] { parameterName, str });
                        }
                        return false;
                    }
                    propertiesTable[str2] = str3;
                }
            }
            return true;
        }

        internal static bool GetTableWithEscaping(TaskLoggingHelper log, string parameterName, string[] propertyNameValueStrings, out Hashtable finalPropertiesTable)
        {
            finalPropertiesTable = null;
            if (propertyNameValueStrings != null)
            {
                finalPropertiesTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
                List<PropertyNameValuePair> list = new List<PropertyNameValuePair>();
                foreach (string str in propertyNameValueStrings)
                {
                    int index = str.IndexOf('=');
                    if (index != -1)
                    {
                        string propertyName = str.Substring(0, index).Trim();
                        string propertyValue = EscapingUtilities.Escape(str.Substring(index + 1).Trim());
                        if (propertyName.Length == 0)
                        {
                            if (log != null)
                            {
                                log.LogErrorWithCodeFromResources("General.InvalidPropertyError", new object[] { parameterName, str });
                            }
                            return false;
                        }
                        list.Add(new PropertyNameValuePair(propertyName, propertyValue));
                    }
                    else if (list.Count > 0)
                    {
                        string str4 = EscapingUtilities.Escape(str.Trim());
                        list[list.Count - 1].Value.Append(';');
                        list[list.Count - 1].Value.Append(str4);
                    }
                    else
                    {
                        if (log != null)
                        {
                            log.LogErrorWithCodeFromResources("General.InvalidPropertyError", new object[] { parameterName, str });
                        }
                        return false;
                    }
                }
                if (log != null)
                {
                    //log.LogMessageFromText(string.Format(CultureInfo.InvariantCulture, "{0}:", new object[] { parameterName }), MessageImportance.Low);
                }
                foreach (PropertyNameValuePair pair in list)
                {
                    finalPropertiesTable[pair.Name] = pair.Value.ToString();
                    if (log != null)
                    {
                        //log.LogMessageFromText(string.Format(CultureInfo.InvariantCulture, "  {0}={1}", new object[] { pair.Name, pair.Value.ToString() }), MessageImportance.Low);
                    }
                }
            }
            return true;
        }

        // Nested Types
        private class PropertyNameValuePair
        {
            // Fields
            private string name;
            private StringBuilder value;

            // Methods
            private PropertyNameValuePair()
            {
            }

            internal PropertyNameValuePair(string propertyName, string propertyValue)
            {
                this.Name = propertyName;
                this.Value = new StringBuilder();
                this.Value.Append(propertyValue);
            }

            // Properties
            internal string Name
            {
                get
                {
                    return this.name;
                }
                set
                {
                    this.name = value;
                }
            }

            internal StringBuilder Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    this.value = value;
                }
            }
        }
    }




    /// <summary>
    /// Populates item collections with the input items. This allows items to be copied from one list to another.
    /// </summary>
    /// <remarks>
    /// This task is deprecated. Starting with .NET Framework 3.5, item groups may be placed within Target elements. For more information, see MSBuild Items. 
    /// <para> </para>
    /// <para>In addition to the parameters listed above, this task inherits parameters from the <see cref="T:Jedzia.BackBock.Tasks.TaskExtension">TaskExtension</see> class, which itself inherits from the Task class. For a list of these additional parameters and their descriptions, see TaskExtension Base Class.</para>
    /// </remarks>
    /// <example>
    /// The following code example creates a new item collection named MySourceItemsWithMetadata from the item collection MySourceItems. The CreateItem task populates the new item collection with the items in the MySourceItems item. It then adds an additional metadata entry named MyMetadata with a value of Hello to each item in the new collection. 
    /// <para> </para>
    /// <para>After the task is executed, the MySourceItemsWithMetadata item collection contains the items file1.resx and file2.resx, both with metadata entries for MyMetadata. The MySourceItems item collection is unchanged.</para>
    /// <para> </para>
    /// <code>&lt;Project xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
    /// 
    ///     &lt;ItemGroup&gt;
    ///         &lt;MySourceItems Include=&quot;file1.resx;file2.resx&quot; /&gt;
    ///     &lt;/ItemGroup&gt;
    /// 
    ///     &lt;Target Name=&quot;NewItems&quot;&gt;
    ///         &lt;CreateItem
    ///             Include=&quot;@(MySourceItems)&quot;
    ///             AdditionalMetadata=&quot;MyMetadata=Hello&quot;&gt;
    ///            &lt;Output
    ///                TaskParameter=&quot;Include&quot;
    ///                ItemName=&quot;MySourceItemsWithMetadata&quot;/&gt;
    ///         &lt;/CreateItem&gt;
    /// 
    ///     &lt;/Target&gt;
    /// 
    /// &lt;/Project&gt;</code>
    /// <para></para>
    /// <para>The following table describes the value of the output item after task execution. Item metadata is shown in parenthesis after the item.</para>
    /// <para></para>
    /// <list type="table">
    /// <listheader>
    /// <term>Item collection</term>
    /// <description>Contents</description></listheader>
    /// <item>
    /// <term>MySourceItemsWithMetadata</term>
    /// <description>file1.resx (MyMetadata=&quot;Hello&quot;)
    /// <para></para>
    /// <para>file2.resx (MyMetadata=&quot;Hello&quot;)</para></description></item></list>
    /// </example>
    public class CreateItem : TaskExtension
    {
        // Fields
        private string[] additionalMetadata;
        private ITaskItem[] exclude;
        private ITaskItem[] include;

        private ArrayList CreateOutputItems(Hashtable metadataTable, Hashtable excludeItems)
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < this.Include.Length; i++)
            {
                if ((excludeItems.Count == 0) || !excludeItems.ContainsKey(this.Include[i].ItemSpec))
                {
                    ITaskItem item = new TaskItem(this.Include[i]);
                    if (metadataTable != null)
                    {
                        foreach (DictionaryEntry entry in metadataTable)
                        {
                            item.SetMetadata((string)entry.Key, (string)entry.Value);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// Executes a task.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the task executed successfully; otherwise, <c>false</c>.
        /// </returns>
        public override bool Execute()
        {
            if (this.Include == null)
            {
                this.include = new TaskItem[0];
                return true;
            }
            this.Include = ExpandWildcards(this.Include);
            this.Exclude = ExpandWildcards(this.Exclude);
            if ((this.AdditionalMetadata != null) || (this.Exclude != null))
            {
                Hashtable hashtable;
                if (!PropertyParser.GetTable(base.Log, "AdditionalMetadata", this.AdditionalMetadata, out hashtable))
                {
                    return false;
                }
                Hashtable uniqueItems = GetUniqueItems(this.Exclude);
                ArrayList list = this.CreateOutputItems(hashtable, uniqueItems);
                this.include = (ITaskItem[])list.ToArray(typeof(ITaskItem));
            }
            return true;
        }

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

        private static Hashtable GetUniqueItems(ITaskItem[] items)
        {
            Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
            if (items != null)
            {
                foreach (ITaskItem item in items)
                {
                    hashtable[item.ItemSpec] = string.Empty;
                }
            }
            return hashtable;
        }

        /// <summary>
        /// Gets or sets the additional metadata.
        /// </summary>
        /// <remarks>
        /// Optional String array parameter.
        /// <para></para>
        /// <para>Specifies additional metadata to attach to the output items. Specify the metadata name and value for the item with the following syntax:</para>
        /// <para></para>
        /// <para><c>MetadataName=MetadataValue</c></para>
        /// <para></para>
        /// <para>Multiple metadata name/value pairs should be separated with a semicolon. If either the name or the value contains a semicolon or any other special characters, they must be escaped. For more information, see How to: Escape Special Characters in MSBuild.</para>
        /// </remarks>
        /// <value>
        /// The additional metadata.
        /// </value>
        public string[] AdditionalMetadata
        {
            get
            {
                return this.additionalMetadata;
            }
            set
            {
                this.additionalMetadata = value;
            }
        }

        /// <summary>
        /// Gets or sets the optional ITaskItem[] output parameters.
        /// </summary>
        /// <remarks>
        /// Optional ITaskItem[] output parameter.
        /// <para></para>
        /// <para>Specifies the items to exclude from the output item collection. This parameter can contain wildcard specifications. For more information, see MSBuild Items and How to: Exclude Files from the Build.</para>
        /// </remarks>
        /// <value>
        /// The optional ITaskItem[] output parameters.
        /// </value>
        public ITaskItem[] Exclude
        {
            get
            {
                return this.exclude;
            }
            set
            {
                this.exclude = value;
            }
        }

        /// <summary>
        /// Gets or sets the Required ITaskItem[] parameter.
        /// </summary>
        /// <remarks>
        /// Specifies the items to include in the output item collection. This parameter can contain wildcard specifications.
        /// </remarks>
        /// <value>
        /// The Required ITaskItem[] parameter.
        /// </value>
        [Output]
        public ITaskItem[] Include
        {
            get
            {
                return this.include;
            }
            set
            {
                this.include = value;
            }
        }

        /// <summary>
        /// Gets the name of the Task.
        /// </summary>
        public override string Name
        {
            get { return "CreateItem"; }
        }
    }


}