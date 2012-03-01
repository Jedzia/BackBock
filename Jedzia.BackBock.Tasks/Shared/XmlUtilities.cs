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
    using System.Xml;

    internal static class XmlUtilities
    {
        // Methods
        internal static string GetXmlNodeFile(XmlNode node, string defaultFile)
        {
            ErrorUtilities.VerifyThrow(node != null, "Need XML node.");
            ErrorUtilities.VerifyThrow(defaultFile != null, "Must specify the default file to use.");
            string localPath = defaultFile;
            if ((node.OwnerDocument.BaseURI != null) && (node.OwnerDocument.BaseURI.Length > 0))
            {
                localPath = new Uri(node.OwnerDocument.BaseURI).LocalPath;
            }
            return localPath;
        }

        internal static bool IsXmlRootElement(XmlNode node)
        {
            return ((((node.NodeType != XmlNodeType.Comment) && (node.NodeType != XmlNodeType.Whitespace)) &&
                     ((node.NodeType != XmlNodeType.XmlDeclaration) &&
                      (node.NodeType != XmlNodeType.ProcessingInstruction))) &&
                    (node.NodeType != XmlNodeType.DocumentType));
        }

        internal static int LocateFirstInvalidElementNameCharacter(string name)
        {
            if ((((name[0] < 'A') || (name[0] > 'Z')) && ((name[0] < 'a') || (name[0] > 'z'))) && (name[0] != '_'))
            {
                return 0;
            }
            for (int i = 1; i < name.Length; i++)
            {
                if ((((name[i] < 'a') || (name[i] > 'z')) && ((name[i] < 'A') || (name[i] > 'Z'))) &&
                    (((name[i] < '0') || (name[i] > '9')) && ((name[i] != '_') && (name[i] != '-'))))
                {
                    return i;
                }
            }
            return -1;
        }

        internal static XmlElement RenameXmlElement(XmlElement oldElement, string newElementName, string xmlNamespace)
        {
            XmlElement newChild = (xmlNamespace == null)
                                      ? oldElement.OwnerDocument.CreateElement(newElementName)
                                      : oldElement.OwnerDocument.CreateElement(newElementName, xmlNamespace);
            foreach(XmlAttribute attribute in oldElement.Attributes)
            {
                XmlAttribute newAttr = (XmlAttribute)attribute.CloneNode(true);
                newChild.SetAttributeNode(newAttr);
            }
            foreach(XmlNode node in oldElement.ChildNodes)
            {
                XmlNode node2 = node.CloneNode(true);
                newChild.AppendChild(node2);
            }
            if (oldElement.ParentNode != null)
            {
                oldElement.ParentNode.ReplaceChild(newChild, oldElement);
            }
            return newChild;
        }

        internal static void VerifyThrowValidElementName(string name)
        {
            int num = LocateFirstInvalidElementNameCharacter(name);
            if (-1 != num)
            {
                ErrorUtilities.VerifyThrowArgument(false, "NameInvalid", name, name[num]);
            }
        }
    }
}