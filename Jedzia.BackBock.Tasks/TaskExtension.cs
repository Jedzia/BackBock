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




    public class CreateItem : TaskExtension
    {
        // Fields
        private string[] additionalMetadata;
        private ITaskItem[] exclude;
        private ITaskItem[] include;

        // Methods
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

        // Properties
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

        public override string Name
        {
            get { return "CreateItem"; }
        }
    }


}