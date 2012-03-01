// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks.BuildEngine
{
    using System;
    using System.IO;
    using System.Xml;
    using Jedzia.BackBock.Tasks.Shared;

    internal static class XmlSearcher
    {
        // Methods
        internal static bool GetElementAndAttributeNumber(
            XmlNode xmlNodeToFind, out int elementNumber, out int attributeNumber)
        {
            XmlNode ownerElement;
            ErrorUtilities.VerifyThrow(xmlNodeToFind != null, "No Xml node!");
            elementNumber = 0;
            attributeNumber = 0;
            if ((xmlNodeToFind.NodeType == XmlNodeType.Element) || (xmlNodeToFind.NodeType == XmlNodeType.Text))
            {
                ownerElement = xmlNodeToFind;
            }
            else if (xmlNodeToFind.NodeType == XmlNodeType.Attribute)
            {
                ownerElement = ((XmlAttribute)xmlNodeToFind).OwnerElement;
                ErrorUtilities.VerifyThrow(ownerElement != null, "How can an xml attribute not have a parent?");
            }
            else
            {
                return false;
            }
            XmlNode ownerDocument = xmlNodeToFind.OwnerDocument;
            Label_005E:
            if ((ownerDocument.NodeType == XmlNodeType.Element) || (ownerDocument.NodeType == XmlNodeType.Text))
            {
                elementNumber++;
                if (ownerDocument == ownerElement)
                {
                    goto Label_00A9;
                }
            }
            if (!ownerDocument.HasChildNodes)
            {
                while ((ownerDocument != null) && (ownerDocument.NextSibling == null))
                {
                    ownerDocument = ownerDocument.ParentNode;
                }
                if (ownerDocument != null)
                {
                    ownerDocument = ownerDocument.NextSibling;
                    goto Label_005E;
                }
            }
            else
            {
                ownerDocument = ownerDocument.FirstChild;
                goto Label_005E;
            }
            Label_00A9:
            if (ownerDocument == null)
            {
                elementNumber = 0;
                return false;
            }
            if (xmlNodeToFind.NodeType == XmlNodeType.Attribute)
            {
                bool flag = false;
                foreach(XmlAttribute attribute in (ownerElement).Attributes)
                {
                    attributeNumber++;
                    if (attribute == (xmlNodeToFind))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool GetLineColumnByNode(
            XmlNode xmlNodeToFind, out int foundLineNumber, out int foundColumnNumber)
        {
            int num;
            int num2;
            foundLineNumber = 0;
            foundColumnNumber = 0;
            if (xmlNodeToFind == null)
            {
                return false;
            }
            string xmlNodeFile = XmlUtilities.GetXmlNodeFile(xmlNodeToFind, string.Empty);
            if ((xmlNodeFile.Length == 0) || !File.Exists(xmlNodeFile))
            {
                return false;
            }
            if (!GetElementAndAttributeNumber(xmlNodeToFind, out num, out num2))
            {
                return false;
            }
            return GetLineColumnByNodeNumber(xmlNodeFile, num, num2, out foundLineNumber, out foundColumnNumber);
        }

        internal static bool GetLineColumnByNodeNumber(
            string projectFile,
            int xmlElementNumberToSearchFor,
            int xmlAttributeNumberToSearchFor,
            out int foundLineNumber,
            out int foundColumnNumber)
        {
            ErrorUtilities.VerifyThrow(xmlElementNumberToSearchFor != 0, "No element to search for!");
            ErrorUtilities.VerifyThrow((projectFile != null) && (projectFile.Length != 0), "No project file!");
            foundLineNumber = 0;
            foundColumnNumber = 0;
            try
            {
                using (XmlTextReader reader = new XmlTextReader(projectFile))
                {
                    int num = 0;
                    while ((reader.Read() && (foundColumnNumber == 0)) && (foundLineNumber == 0))
                    {
                        if ((reader.NodeType == XmlNodeType.Element) || (reader.NodeType == XmlNodeType.Text))
                        {
                            num++;
                            if (num == xmlElementNumberToSearchFor)
                            {
                                if (xmlAttributeNumberToSearchFor == 0)
                                {
                                    foundLineNumber = reader.LineNumber;
                                    foundColumnNumber = reader.LinePosition;
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        foundColumnNumber--;
                                    }
                                }
                                else if (reader.MoveToFirstAttribute())
                                {
                                    int num2 = 0;
                                    do
                                    {
                                        num2++;
                                        if (num2 == xmlAttributeNumberToSearchFor)
                                        {
                                            foundLineNumber = reader.LineNumber;
                                            foundColumnNumber = reader.LinePosition;
                                        }
                                    } while ((reader.MoveToNextAttribute() && (foundColumnNumber == 0)) &&
                                             (foundLineNumber == 0));
                                }
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            return ((foundColumnNumber != 0) && (foundLineNumber != 0));
        }
    }
}