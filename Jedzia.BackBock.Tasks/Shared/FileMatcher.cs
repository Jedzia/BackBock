// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks.Shared
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class FileMatcher
    {
        // Fields

        #region Fields

        private const string dotdot = "..";
        private const string recursiveDirectoryMatch = "**";

        internal static readonly char[] directorySeparatorCharacters = new[]
                                                                           {
                                                                               Path.DirectorySeparatorChar,
                                                                               Path.AltDirectorySeparatorChar
                                                                           };

        private static readonly string altDirectorySeparator = new string(Path.AltDirectorySeparatorChar, 1);
        private static readonly DirectoryExists defaultDirectoryExists = Directory.Exists;
        private static readonly GetFileSystemEntries defaultGetFileSystemEntries = GetAccessibleFileSystemEntries;
        private static readonly string directorySeparator = new string(Path.DirectorySeparatorChar, 1);

        private static readonly char[] wildcardCharacters = new[] { '*', '?' };

        #endregion

        #region Delegates

        internal delegate bool DirectoryExists(string path);

        internal delegate string[] GetFileSystemEntries(FileSystemEntity entityType, string path, string pattern);

        #endregion

        // Methods
        internal static Result FileMatch(string filespec, string fileToMatch)
        {
            return FileMatch(filespec, fileToMatch, defaultGetFileSystemEntries);
        }

        internal static Result FileMatch(string filespec, string fileToMatch, GetFileSystemEntries getFileSystemEntries)
        {
            Regex regex;
            Result result = new Result();
            fileToMatch = GetLongPathName(fileToMatch, getFileSystemEntries);
            GetFileSpecInfo(
                filespec, out regex, out result.isFileSpecRecursive, out result.isLegalFileSpec, getFileSystemEntries);
            if (result.isLegalFileSpec)
            {
                Match match = regex.Match(fileToMatch);
                result.isMatch = match.Success;
                if (result.isMatch)
                {
                    result.fixedDirectoryPart = match.Groups["FIXEDDIR"].Value;
                    result.wildcardDirectoryPart = match.Groups["WILDCARDDIR"].Value;
                    result.filenamePart = match.Groups["FILENAME"].Value;
                }
            }
            return result;
        }

        internal static void GetFileSpecInfo(
            string filespec,
            out Regex regexFileMatch,
            out bool needsRecursion,
            out bool isLegalFileSpec,
            GetFileSystemEntries getFileSystemEntries)
        {
            string str;
            string str2;
            string str3;
            string str4;
            GetFileSpecInfo(
                filespec,
                out str,
                out str2,
                out str3,
                out str4,
                out needsRecursion,
                out isLegalFileSpec,
                getFileSystemEntries);
            if (isLegalFileSpec)
            {
                regexFileMatch = new Regex(str4, RegexOptions.IgnoreCase);
            }
            else
            {
                regexFileMatch = null;
            }
        }

        internal static string[] GetFiles(string filespec)
        {
            return GetFiles(filespec, defaultGetFileSystemEntries, defaultDirectoryExists);
        }

        internal static string[] GetFiles(
            string filespec, GetFileSystemEntries getFileSystemEntries, DirectoryExists directoryExists)
        {
            string str;
            string str2;
            string str3;
            string str4;
            bool flag;
            bool flag2;
            if (!HasWildcards(filespec))
            {
                return new[] { filespec };
            }
            ArrayList list = new ArrayList();
            IList listOfFiles = list;
            GetFileSpecInfo(filespec, out str, out str2, out str3, out str4, out flag, out flag2, getFileSystemEntries);
            if (!flag2)
            {
                return new[] { filespec };
            }
            if ((str.Length > 0) && !directoryExists(str))
            {
                return new string[0];
            }
            bool flag3 = (str2.Length > 0) && (str2 != ("**" + directorySeparator));
            string str5 = flag3 ? null : Path.GetExtension(str3);
            bool flag4 = ((str5 != null) && (str5.IndexOf('*') == -1)) &&
                         (str5.EndsWith("?", StringComparison.Ordinal) ||
                          ((str5.Length == 4) && (str3.IndexOf('*') != -1)));
            GetFilesRecursive(
                listOfFiles,
                str,
                str2,
                flag3 ? null : str3,
                flag4 ? str5.Length : 0,
                flag3 ? new Regex(str4, RegexOptions.IgnoreCase) : null,
                flag,
                getFileSystemEntries);
            return (string[])list.ToArray(typeof(string));
        }

        internal static string GetLongPathName(string path, GetFileSystemEntries getFileSystemEntries)
        {
            string str;
            if (path.IndexOf("~", StringComparison.Ordinal) == -1)
            {
                return path;
            }
            ErrorUtilities.VerifyThrow(
                !HasWildcards(path), "GetLongPathName does not handle wildcards and was passed '{0}'.", path);
            string[] strArray = path.Split(directorySeparatorCharacters);
            int num = 0;
            if (path.StartsWith(directorySeparator + directorySeparator, StringComparison.Ordinal))
            {
                str = ((directorySeparator + directorySeparator) + strArray[2] + directorySeparator) + strArray[3] +
                      directorySeparator;
                num = 4;
            }
            else if ((path.Length > 2) && (path[1] == ':'))
            {
                str = strArray[0] + directorySeparator;
                num = 1;
            }
            else
            {
                str = string.Empty;
                num = 0;
            }
            string[] strArray2 = new string[strArray.Length - num];
            string str2 = str;
            for (int i = num; i < strArray.Length; i++)
            {
                if (strArray[i].Length == 0)
                {
                    strArray2[i - num] = string.Empty;
                }
                else if (strArray[i].IndexOf("~", StringComparison.Ordinal) == -1)
                {
                    strArray2[i - num] = strArray[i];
                    str2 = Path.Combine(str2, strArray[i]);
                }
                else
                {
                    string[] strArray3 = getFileSystemEntries(FileSystemEntity.FilesAndDirectories, str2, strArray[i]);
                    if (strArray3.Length == 0)
                    {
                        for (int j = i; j < strArray.Length; j++)
                        {
                            strArray2[j - num] = strArray[j];
                        }
                        break;
                    }
                    ErrorUtilities.VerifyThrow(
                        strArray3.Length == 1,
                        "Unexpected number of entries ({3}) found when enumerating '{0}' under '{1}'. Original path was '{2}'",
                        strArray[i],
                        str2,
                        path,
                        strArray3.Length);
                    str2 = strArray3[0];
                    strArray2[i - num] = Path.GetFileName(str2);
                }
            }
            return (str + string.Join(directorySeparator, strArray2));
        }

        internal static bool HasWildcards(string filespec)
        {
            return (-1 != filespec.IndexOfAny(wildcardCharacters));
        }

        internal static void SplitFileSpec(
            string filespec,
            out string fixedDirectoryPart,
            out string wildcardDirectoryPart,
            out string filenamePart,
            GetFileSystemEntries getFileSystemEntries)
        {
            PreprocessFileSpecForSplitting(
                filespec, out fixedDirectoryPart, out wildcardDirectoryPart, out filenamePart);
            if ("**" == filenamePart)
            {
                wildcardDirectoryPart = wildcardDirectoryPart + "**";
                wildcardDirectoryPart = wildcardDirectoryPart + directorySeparator;
                filenamePart = "*.*";
            }
            fixedDirectoryPart = GetLongPathName(fixedDirectoryPart, getFileSystemEntries);
        }

        private static string[] GetAccessibleDirectories(string path, string pattern)
        {
            try
            {
                string[] paths = null;
                if (pattern == null)
                {
                    paths = Directory.GetDirectories((path.Length == 0) ? @".\" : path);
                }
                else
                {
                    paths = Directory.GetDirectories((path.Length == 0) ? @".\" : path, pattern);
                }
                if (!path.StartsWith(@".\", StringComparison.Ordinal))
                {
                    RemoveInitialDotSlash(paths);
                }
                return paths;
            }
            catch (SecurityException)
            {
                return new string[0];
            }
            catch (UnauthorizedAccessException)
            {
                return new string[0];
            }
        }

        private static string[] GetAccessibleFileSystemEntries(FileSystemEntity entityType, string path, string pattern)
        {
            switch (entityType)
            {
                case FileSystemEntity.Files:
                    return GetAccessibleFiles(path, pattern);

                case FileSystemEntity.Directories:
                    return GetAccessibleDirectories(path, pattern);

                case FileSystemEntity.FilesAndDirectories:
                    return GetAccessibleFilesAndDirectories(path, pattern);
            }
            ErrorUtilities.VerifyThrow(false, "Unexpected filesystem entity type.");
            return null;
        }

        private static string[] GetAccessibleFiles(string path, string filespec)
        {
            try
            {
                string str = (path.Length == 0) ? @".\" : path;
                string[] paths = (filespec == null) ? Directory.GetFiles(str) : Directory.GetFiles(str, filespec);
                if (!path.StartsWith(@".\", StringComparison.Ordinal))
                {
                    RemoveInitialDotSlash(paths);
                }
                return paths;
            }
            catch (SecurityException)
            {
                return new string[0];
            }
            catch (UnauthorizedAccessException)
            {
                return new string[0];
            }
        }

        private static string[] GetAccessibleFilesAndDirectories(string path, string pattern)
        {
            string[] fileSystemEntries = null;
            if (Directory.Exists(path))
            {
                try
                {
                    fileSystemEntries = Directory.GetFileSystemEntries(path, pattern);
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (SecurityException)
                {
                }
            }
            if (fileSystemEntries == null)
            {
                fileSystemEntries = new string[0];
            }
            return fileSystemEntries;
        }

        private static void GetFileSpecInfo(
            string filespec,
            out string fixedDirectoryPart,
            out string wildcardDirectoryPart,
            out string filenamePart,
            out string matchFileExpression,
            out bool needsRecursion,
            out bool isLegalFileSpec,
            GetFileSystemEntries getFileSystemEntries)
        {
            isLegalFileSpec = true;
            needsRecursion = false;
            fixedDirectoryPart = string.Empty;
            wildcardDirectoryPart = string.Empty;
            filenamePart = string.Empty;
            matchFileExpression = null;
            if (-1 != filespec.IndexOfAny(Path.GetInvalidPathChars()))
            {
                isLegalFileSpec = false;
            }
            else if (-1 != filespec.IndexOf("...", StringComparison.Ordinal))
            {
                isLegalFileSpec = false;
            }
            else
            {
                int num = filespec.LastIndexOf(":", StringComparison.Ordinal);
                if ((-1 != num) && (1 != num))
                {
                    isLegalFileSpec = false;
                }
                else
                {
                    SplitFileSpec(
                        filespec,
                        out fixedDirectoryPart,
                        out wildcardDirectoryPart,
                        out filenamePart,
                        getFileSystemEntries);
                    matchFileExpression = RegularExpressionFromFileSpec(
                        fixedDirectoryPart, wildcardDirectoryPart, filenamePart, out isLegalFileSpec);
                    if (isLegalFileSpec)
                    {
                        needsRecursion = wildcardDirectoryPart.Length != 0;
                    }
                }
            }
        }

        private static void GetFilesRecursive(
            IList listOfFiles,
            string baseDirectory,
            string remainingWildcardDirectory,
            string filespec,
            int extensionLengthToEnforce,
            Regex regexFileMatch,
            bool needsRecursion,
            GetFileSystemEntries getFileSystemEntries)
        {
            ErrorUtilities.VerifyThrow(
                (filespec != null) || (regexFileMatch != null),
                "Need either a file-spec or a regular expression to match files.");
            ErrorUtilities.VerifyThrow(
                remainingWildcardDirectory != null, "Expected non-null remaning wildcard directory.");
            bool flag = false;
            if (remainingWildcardDirectory.Length == 0)
            {
                flag = true;
            }
            else if (remainingWildcardDirectory.IndexOf("**", StringComparison.Ordinal) == 0)
            {
                flag = true;
            }
            if (flag)
            {
                foreach(string str in getFileSystemEntries(FileSystemEntity.Files, baseDirectory, filespec))
                {
                    if (((filespec != null) || regexFileMatch.IsMatch(str)) &&
                        (((filespec == null) || (extensionLengthToEnforce == 0)) ||
                         (Path.GetExtension(str).Length == extensionLengthToEnforce)))
                    {
                        listOfFiles.Add(str);
                    }
                }
            }
            if (needsRecursion && (remainingWildcardDirectory.Length > 0))
            {
                string pattern = null;
                if (remainingWildcardDirectory != "**")
                {
                    int length = remainingWildcardDirectory.IndexOfAny(directorySeparatorCharacters);
                    ErrorUtilities.VerifyThrow(length != -1, "Slash should be guaranteed.");
                    pattern = remainingWildcardDirectory.Substring(0, length);
                    remainingWildcardDirectory = remainingWildcardDirectory.Substring(length + 1);
                    if (pattern == "**")
                    {
                        pattern = null;
                        remainingWildcardDirectory = "**";
                    }
                }
                foreach(string str3 in getFileSystemEntries(FileSystemEntity.Directories, baseDirectory, pattern))
                {
                    GetFilesRecursive(
                        listOfFiles,
                        str3,
                        remainingWildcardDirectory,
                        filespec,
                        extensionLengthToEnforce,
                        regexFileMatch,
                        true,
                        getFileSystemEntries);
                }
            }
        }

        private static void PreprocessFileSpecForSplitting(
            string filespec, out string fixedDirectoryPart, out string wildcardDirectoryPart, out string filenamePart)
        {
            int num = filespec.LastIndexOfAny(directorySeparatorCharacters);
            if (-1 == num)
            {
                fixedDirectoryPart = string.Empty;
                wildcardDirectoryPart = string.Empty;
                filenamePart = filespec;
            }
            else
            {
                int length = filespec.IndexOfAny(wildcardCharacters);
                if ((-1 == length) || (length > num))
                {
                    fixedDirectoryPart = filespec.Substring(0, num + 1);
                    wildcardDirectoryPart = string.Empty;
                    filenamePart = filespec.Substring(num + 1);
                }
                else
                {
                    int num3 = filespec.Substring(0, length).LastIndexOfAny(directorySeparatorCharacters);
                    if (-1 == num3)
                    {
                        fixedDirectoryPart = string.Empty;
                        wildcardDirectoryPart = filespec.Substring(0, num + 1);
                        filenamePart = filespec.Substring(num + 1);
                    }
                    else
                    {
                        fixedDirectoryPart = filespec.Substring(0, num3 + 1);
                        wildcardDirectoryPart = filespec.Substring(num3 + 1, num - num3);
                        filenamePart = filespec.Substring(num + 1);
                    }
                }
            }
        }

        private static string RegularExpressionFromFileSpec(
            string fixedDirectoryPart, string wildcardDirectoryPart, string filenamePart, out bool isLegalFileSpec)
        {
            int length;
            isLegalFileSpec = true;
            if ((((fixedDirectoryPart.IndexOf("<:", StringComparison.Ordinal) != -1) ||
                  (fixedDirectoryPart.IndexOf(":>", StringComparison.Ordinal) != -1)) ||
                 ((wildcardDirectoryPart.IndexOf("<:", StringComparison.Ordinal) != -1) ||
                  (wildcardDirectoryPart.IndexOf(":>", StringComparison.Ordinal) != -1))) ||
                ((filenamePart.IndexOf("<:", StringComparison.Ordinal) != -1) ||
                 (filenamePart.IndexOf(":>", StringComparison.Ordinal) != -1)))
            {
                isLegalFileSpec = false;
                return string.Empty;
            }
            if (wildcardDirectoryPart.Contains(".."))
            {
                isLegalFileSpec = false;
                return string.Empty;
            }
            if (filenamePart.EndsWith(".", StringComparison.Ordinal))
            {
                filenamePart = filenamePart.Replace("*", "<:anythingbutdot:>");
                filenamePart = filenamePart.Replace("?", "<:anysinglecharacterbutdot:>");
                filenamePart = filenamePart.Substring(0, filenamePart.Length - 1);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("<:bol:>");
            builder.Append("<:fixeddir:>").Append(fixedDirectoryPart).Append("<:endfixeddir:>");
            builder.Append("<:wildcarddir:>").Append(wildcardDirectoryPart).Append("<:endwildcarddir:>");
            builder.Append("<:filename:>").Append(filenamePart).Append("<:endfilename:>");
            builder.Append("<:eol:>");
            builder.Replace(directorySeparator, "<:dirseparator:>");
            builder.Replace(altDirectorySeparator, "<:dirseparator:>");
            builder.Replace("<:fixeddir:><:dirseparator:><:dirseparator:>", "<:fixeddir:><:uncslashslash:>");
            do
            {
                length = builder.Length;
                builder.Replace("<:dirseparator:>.<:dirseparator:>", "<:dirseparator:>");
                builder.Replace("<:dirseparator:><:dirseparator:>", "<:dirseparator:>");
                builder.Replace("<:fixeddir:>.<:dirseparator:>.<:dirseparator:>", "<:fixeddir:>.<:dirseparator:>");
                builder.Replace("<:dirseparator:>.<:endfilename:>", "<:endfilename:>");
                builder.Replace("<:filename:>.<:endfilename:>", "<:filename:><:endfilename:>");
                ErrorUtilities.VerifyThrow(
                    builder.Length <= length, "Expression reductions cannot increase the length of the expression.");
            } while (builder.Length < length);
            do
            {
                length = builder.Length;
                builder.Replace("**<:dirseparator:>**", "**");
                ErrorUtilities.VerifyThrow(
                    builder.Length <= length, "Expression reductions cannot increase the length of the expression.");
            } while (builder.Length < length);
            do
            {
                length = builder.Length;
                builder.Replace("<:dirseparator:>**<:dirseparator:>", "<:middledirs:>");
                builder.Replace("<:wildcarddir:>**<:dirseparator:>", "<:wildcarddir:><:leftdirs:>");
                ErrorUtilities.VerifyThrow(
                    builder.Length <= length, "Expression reductions cannot increase the length of the expression.");
            } while (builder.Length < length);
            if (builder.Length > builder.Replace("**", null).Length)
            {
                isLegalFileSpec = false;
                return string.Empty;
            }
            builder.Replace("*.*", "<:anynonseparator:>");
            builder.Replace("*", "<:anynonseparator:>");
            builder.Replace("?", "<:singlecharacter:>");
            builder.Replace(@"\", @"\\");
            builder.Replace("$", @"\$");
            builder.Replace("(", @"\(");
            builder.Replace(")", @"\)");
            builder.Replace("*", @"\*");
            builder.Replace("+", @"\+");
            builder.Replace(".", @"\.");
            builder.Replace("[", @"\[");
            builder.Replace("?", @"\?");
            builder.Replace("^", @"\^");
            builder.Replace("{", @"\{");
            builder.Replace("|", @"\|");
            builder.Replace("<:middledirs:>", @"((/)|(\\)|(/.*/)|(/.*\\)|(\\.*\\)|(\\.*/))");
            builder.Replace("<:leftdirs:>", @"((.*/)|(.*\\)|())");
            builder.Replace("<:rightdirs:>", ".*");
            builder.Replace("<:anything:>", ".*");
            builder.Replace("<:anythingbutdot:>", @"[^\.]*");
            builder.Replace("<:anysinglecharacterbutdot:>", @"[^\.].");
            builder.Replace("<:anynonseparator:>", @"[^/\\]*");
            builder.Replace("<:singlecharacter:>", ".");
            builder.Replace("<:dirseparator:>", @"[/\\]");
            builder.Replace("<:uncslashslash:>", @"\\\\");
            builder.Replace("<:bol:>", "^");
            builder.Replace("<:eol:>", "$");
            builder.Replace("<:fixeddir:>", "(?<FIXEDDIR>");
            builder.Replace("<:endfixeddir:>", ")");
            builder.Replace("<:wildcarddir:>", "(?<WILDCARDDIR>");
            builder.Replace("<:endwildcarddir:>", ")");
            builder.Replace("<:filename:>", "(?<FILENAME>");
            builder.Replace("<:endfilename:>", ")");
            return builder.ToString();
        }

        private static void RemoveInitialDotSlash(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].StartsWith(@".\", StringComparison.Ordinal))
                {
                    paths[i] = paths[i].Substring(2);
                }
            }
        }

        #region Nested type: FileSystemEntity

        internal enum FileSystemEntity
        {
            Files,
            Directories,
            FilesAndDirectories
        }

        #endregion

        #region Nested type: Result

        internal sealed class Result
        {
            // Fields

            #region Fields

            internal string filenamePart = string.Empty;
            internal string fixedDirectoryPart = string.Empty;
            internal bool isFileSpecRecursive;
            internal bool isLegalFileSpec;
            internal bool isMatch;
            internal string wildcardDirectoryPart = string.Empty;

            #endregion

            // Methods
        }

        #endregion
    }
}