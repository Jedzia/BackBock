// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks.Shared
{
    using System.Collections;
    using System.Xml;

    internal static class XMakeElements
    {
        // Fields

        #region Fields

        internal const string choose = "Choose";
        internal const string error = "Error";
        internal const string import = "Import";
        internal const string itemGroup = "ItemGroup";
        internal const string message = "Message";
        internal const string onError = "OnError";
        internal const string otherwise = "Otherwise";
        internal const string output = "Output";
        internal const string project = "Project";
        internal const string projectExtensions = "ProjectExtensions";
        internal const string propertyGroup = "PropertyGroup";
        internal const string target = "Target";
        internal const string usingTask = "UsingTask";
        internal const string visualStudioProject = "VisualStudioProject";
        internal const string warning = "Warning";
        internal const string when = "When";

        internal static readonly string[] illegalPropertyOrItemNames = new[]
                                                                           {
                                                                               "VisualStudioProject", "Target",
                                                                               "PropertyGroup", "Output", "ItemGroup",
                                                                               "UsingTask", "ProjectExtensions", "OnError"
                                                                               , "Choose", "When", "Otherwise"
                                                                           };

        internal static readonly char[] illegalTargetNameCharacters = new[] { '$', '@', '(', ')', '%', '*', '?', '.' };
        private static Hashtable illegalItemOrPropertyNamesHashtable;

        #endregion

        // Methods

        // Properties

        #region Properties

        internal static Hashtable IllegalItemPropertyNames
        {
            get
            {
                if (illegalItemOrPropertyNamesHashtable == null)
                {
                    illegalItemOrPropertyNamesHashtable = new Hashtable();
                    foreach(string str in illegalPropertyOrItemNames)
                    {
                        illegalItemOrPropertyNamesHashtable.Add(str, string.Empty);
                    }
                }
                return illegalItemOrPropertyNamesHashtable;
            }
        }

        #endregion

        internal static bool IsValidTaskChildNode(XmlNode childNode)
        {
            if (!(childNode.Name == "Output") && (childNode.NodeType != XmlNodeType.Comment))
            {
                return (childNode.NodeType == XmlNodeType.Whitespace);
            }
            return true;
        }
    }
}