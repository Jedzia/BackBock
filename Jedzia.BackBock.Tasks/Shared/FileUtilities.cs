namespace Jedzia.BackBock.Tasks.Shared
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using Jedzia.BackBock.Tasks.Utilities;

    internal static class FileUtilities
    {
        // Fields
        internal const string FileTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss'.'fffffff";

        // Methods
        internal static bool EndsWithSlash(string fileSpec)
        {
            if (fileSpec.Length <= 0)
            {
                return false;
            }
            return IsSlash(fileSpec[fileSpec.Length - 1]);
        }

        internal static string EnsureTrailingSlash(string fileSpec)
        {
            if (!EndsWithSlash(fileSpec))
            {
                fileSpec = fileSpec + Path.DirectorySeparatorChar;
            }
            return fileSpec;
        }

        internal static string GetDirectory(string fileSpec)
        {
            string directoryName = Path.GetDirectoryName(fileSpec);
            if (directoryName == null)
            {
                return fileSpec;
            }
            if ((directoryName.Length > 0) && !EndsWithSlash(directoryName))
            {
                directoryName = directoryName + Path.DirectorySeparatorChar;
            }
            return directoryName;
        }

        private static string GetFullPath(string fileSpec)
        {
            fileSpec = EscapingUtilities.UnescapeAll(fileSpec);
            string str = EscapingUtilities.Escape(Path.GetFullPath(fileSpec));
            if (EndsWithSlash(str))
            {
                return str;
            }
            Match match = FileUtilitiesRegex.DrivePattern.Match(fileSpec);
            Match match2 = FileUtilitiesRegex.UNCPattern.Match(str);
            if ((!match.Success || (match.Length != fileSpec.Length)) && (!match2.Success || (match2.Length != str.Length)))
            {
                return str;
            }
            return (str + Path.DirectorySeparatorChar);
        }

        internal static string GetItemSpecModifier(string itemSpec, string modifier, ref Hashtable cachedModifiers)
        {
            ErrorUtilities.VerifyThrow(itemSpec != null, "Need item-spec to modify.");
            ErrorUtilities.VerifyThrow(modifier != null, "Need modifier to apply to item-spec.");
            string fileSpec = null;
            if (cachedModifiers != null)
            {
                ErrorUtilities.VerifyThrow(((string) cachedModifiers[string.Empty]) == itemSpec, "The cache of modifiers is only valid for one item-spec. If the item-spec changes, the cache must be nulled out, or a different cache passed in.");
                fileSpec = (string) cachedModifiers[modifier];
            }
            if (fileSpec == null)
            {
                bool flag = false;
                try
                {
                    if (string.Compare(modifier, "FullPath", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        fileSpec = GetFullPath(itemSpec);
                    }
                    else if (string.Compare(modifier, "RootDir", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        fileSpec = Path.GetPathRoot(Path.GetFullPath(itemSpec));
                        if (!EndsWithSlash(fileSpec))
                        {
                            fileSpec = fileSpec + Path.DirectorySeparatorChar;
                        }
                    }
                    else if (string.Compare(modifier, "Filename", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (Path.GetDirectoryName(itemSpec) == null)
                        {
                            fileSpec = string.Empty;
                        }
                        else
                        {
                            fileSpec = Path.GetFileNameWithoutExtension(itemSpec);
                        }
                    }
                    else if (string.Compare(modifier, "Extension", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (Path.GetDirectoryName(itemSpec) == null)
                        {
                            fileSpec = string.Empty;
                        }
                        else
                        {
                            fileSpec = Path.GetExtension(itemSpec);
                        }
                    }
                    else if (string.Compare(modifier, "RelativeDir", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        fileSpec = GetDirectory(itemSpec);
                    }
                    else if (string.Compare(modifier, "Directory", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        fileSpec = GetDirectory(GetFullPath(itemSpec));
                        Match match = FileUtilitiesRegex.DrivePattern.Match(fileSpec);
                        if (!match.Success)
                        {
                            match = FileUtilitiesRegex.UNCPattern.Match(fileSpec);
                        }
                        if (match.Success)
                        {
                            ErrorUtilities.VerifyThrow((fileSpec.Length > match.Length) && IsSlash(fileSpec[match.Length]), "Root directory must have a trailing slash.");
                            fileSpec = fileSpec.Substring(match.Length + 1);
                        }
                    }
                    else if (string.Compare(modifier, "RecursiveDir", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        fileSpec = string.Empty;
                    }
                    else if (string.Compare(modifier, "Identity", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        fileSpec = itemSpec;
                    }
                    else if (string.Compare(modifier, "ModifiedTime", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        flag = true;
                        string path = EscapingUtilities.UnescapeAll(itemSpec);
                        if (File.Exists(path))
                        {
                            fileSpec = File.GetLastWriteTime(path).ToString("yyyy'-'MM'-'dd HH':'mm':'ss'.'fffffff", null);
                        }
                        else
                        {
                            fileSpec = string.Empty;
                        }
                    }
                    else if (string.Compare(modifier, "CreatedTime", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        flag = true;
                        string str3 = EscapingUtilities.UnescapeAll(itemSpec);
                        if (File.Exists(str3))
                        {
                            fileSpec = File.GetCreationTime(str3).ToString("yyyy'-'MM'-'dd HH':'mm':'ss'.'fffffff", null);
                        }
                        else
                        {
                            fileSpec = string.Empty;
                        }
                    }
                    else if (string.Compare(modifier, "AccessedTime", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        flag = true;
                        string str4 = EscapingUtilities.UnescapeAll(itemSpec);
                        if (File.Exists(str4))
                        {
                            fileSpec = File.GetLastAccessTime(str4).ToString("yyyy'-'MM'-'dd HH':'mm':'ss'.'fffffff", null);
                        }
                        else
                        {
                            fileSpec = string.Empty;
                        }
                    }
                    else
                    {
                        ErrorUtilities.VerifyThrow(false, "\"{0}\" is not a valid item-spec modifier.", modifier);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandling.RethrowUnlessFileIO(exception);
                    ErrorUtilities.VerifyThrowInvalidOperation(false, "Shared.InvalidFilespecForTransform", modifier, itemSpec, exception.Message);
                }
                ErrorUtilities.VerifyThrow(fileSpec != null, "The item-spec modifier \"{0}\" was not evaluated.", modifier);
                if (!flag)
                {
                    if (cachedModifiers == null)
                    {
                        cachedModifiers = new Hashtable(StringComparer.OrdinalIgnoreCase);
                        cachedModifiers[string.Empty] = itemSpec;
                    }
                    cachedModifiers[modifier] = fileSpec;
                }
            }
            return fileSpec;
        }

        internal static bool HasExtension(string fileName, string[] allowedExtensions)
        {
            string extension = Path.GetExtension(fileName);
            foreach (string str2 in allowedExtensions)
            {
                if (string.Compare(extension, str2, true, CultureInfo.CurrentCulture) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool IsDerivableItemSpecModifier(string name)
        {
            bool flag = IsItemSpecModifier(name);
            return (((!flag || (name.Length != 12)) || ((name[0] != 'R') && (name[0] != 'r'))) && flag);
        }

        internal static bool IsItemSpecModifier(string name)
        {
            if (name != null)
            {
                switch (name.Length)
                {
                    case 7:
                        switch (name[0])
                        {
                            case 'R':
                                if (!(name == "RootDir"))
                                {
                                    goto Label_01C0;
                                }
                                return true;

                            case 'r':
                                goto Label_01C0;
                        }
                        return false;

                    case 8:
                        {
                            char ch2 = name[0];
                            if (ch2 > 'I')
                            {
                                switch (ch2)
                                {
                                    case 'f':
                                    case 'i':
                                        goto Label_01C0;
                                }
                                break;
                            }
                            switch (ch2)
                            {
                                case 'F':
                                    if ((name != "FullPath") && !(name == "Filename"))
                                    {
                                        goto Label_01C0;
                                    }
                                    return true;

                                case 'I':
                                    if (!(name == "Identity"))
                                    {
                                        goto Label_01C0;
                                    }
                                    return true;
                            }
                            break;
                        }
                    case 9:
                        switch (name[0])
                        {
                            case 'D':
                                if (!(name == "Directory"))
                                {
                                    goto Label_01C0;
                                }
                                return true;

                            case 'E':
                                if (!(name == "Extension"))
                                {
                                    goto Label_01C0;
                                }
                                return true;

                            case 'd':
                            case 'e':
                                goto Label_01C0;
                        }
                        return false;

                    case 10:
                        goto Label_01BE;

                    case 11:
                        {
                            char ch4 = name[0];
                            if (ch4 > 'R')
                            {
                                switch (ch4)
                                {
                                    case 'c':
                                    case 'r':
                                        goto Label_01C0;
                                }
                            }
                            else
                            {
                                switch (ch4)
                                {
                                    case 'C':
                                        if (!(name == "CreatedTime"))
                                        {
                                            goto Label_01C0;
                                        }
                                        return true;

                                    case 'R':
                                        if (!(name == "RelativeDir"))
                                        {
                                            goto Label_01C0;
                                        }
                                        return true;
                                }
                            }
                            return false;
                        }
                    case 12:
                        {
                            char ch5 = name[0];
                            if (ch5 > 'R')
                            {
                                switch (ch5)
                                {
                                    case 'a':
                                    case 'm':
                                    case 'r':
                                        goto Label_01C0;
                                }
                            }
                            else
                            {
                                switch (ch5)
                                {
                                    case 'A':
                                        if (!(name == "AccessedTime"))
                                        {
                                            goto Label_01C0;
                                        }
                                        return true;

                                    case 'M':
                                        if (!(name == "ModifiedTime"))
                                        {
                                            goto Label_01C0;
                                        }
                                        return true;

                                    case 'R':
                                        if (!(name == "RecursiveDir"))
                                        {
                                            goto Label_01C0;
                                        }
                                        return true;
                                }
                            }
                            return false;
                        }
                    default:
                        goto Label_01BE;
                }
            }
            return false;
            Label_01BE:
            return false;
            Label_01C0:
            return ItemSpecModifiers.TableOfItemSpecModifiers.ContainsKey(name);
        }

        internal static bool IsSlash(char c)
        {
            if (c != Path.DirectorySeparatorChar)
            {
                return (c == Path.AltDirectorySeparatorChar);
            }
            return true;
        }

        // Nested Types
        internal static class ItemSpecModifiers
        {
            // Fields
            internal const string AccessedTime = "AccessedTime";
            internal static readonly string[] All = new string[] { "FullPath", "RootDir", "Filename", "Extension", "RelativeDir", "Directory", "RecursiveDir", "Identity", "ModifiedTime", "CreatedTime", "AccessedTime" };
            internal const string CreatedTime = "CreatedTime";
            internal const string Directory = "Directory";
            internal const string Extension = "Extension";
            internal const string Filename = "Filename";
            internal const string FullPath = "FullPath";
            internal const string Identity = "Identity";
            internal const string ModifiedTime = "ModifiedTime";
            internal const string RecursiveDir = "RecursiveDir";
            internal const string RelativeDir = "RelativeDir";
            internal const string RootDir = "RootDir";
            private static Hashtable tableOfItemSpecModifiers = null;

            // Properties
            internal static Hashtable TableOfItemSpecModifiers
            {
                get
                {
                    if (tableOfItemSpecModifiers == null)
                    {
                        tableOfItemSpecModifiers = new Hashtable(StringComparer.OrdinalIgnoreCase);
                        foreach (string str in All)
                        {
                            tableOfItemSpecModifiers[str] = string.Empty;
                        }
                    }
                    return tableOfItemSpecModifiers;
                }
            }
        }
    }
}