namespace Jedzia.BackBock.Tasks.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Schema;
    using Jedzia.BackBock.Tasks.BuildEngine;
    using Jedzia.BackBock.Tasks.Shared;

    internal static class EscapingUtilities
    {
        // Fields
        private static char[] charsToEscape = new char[] { '%', '*', '?', '@', '$', '(', ')', ';', '\'' };

        // Methods
        internal static bool ContainsEscapedWildcards(string escapedString)
        {
            if ((-1 == escapedString.IndexOf('%')) || ((-1 == escapedString.IndexOf("%2", StringComparison.Ordinal)) && (-1 == escapedString.IndexOf("%3", StringComparison.Ordinal))))
            {
                return false;
            }
            if (((-1 == escapedString.IndexOf("%2a", StringComparison.Ordinal)) && (-1 == escapedString.IndexOf("%2A", StringComparison.Ordinal))) && (-1 == escapedString.IndexOf("%3f", StringComparison.Ordinal)))
            {
                return (-1 != escapedString.IndexOf("%3F", StringComparison.Ordinal));
            }
            return true;
        }

        private static bool ContainsReservedCharacters(string unescapedString)
        {
            return (-1 != unescapedString.IndexOfAny(charsToEscape));
        }

        internal static string Escape(string unescapedString)
        {
            //ErrorUtilities.VerifyThrow(unescapedString != null, "Null strings not allowed.");
            if (!ContainsReservedCharacters(unescapedString))
            {
                return unescapedString;
            }
            StringBuilder builder = new StringBuilder(unescapedString);
            foreach (char ch in charsToEscape)
            {
                int num = Convert.ToInt32(ch);
                string newValue = string.Format(CultureInfo.InvariantCulture, "%{0:x00}", new object[] { num });
                builder.Replace(ch.ToString(CultureInfo.InvariantCulture), newValue);
            }
            return builder.ToString();
        }

        internal static string UnescapeAll(string escapedString)
        {
            bool flag;
            return UnescapeAll(escapedString, out flag);
        }

        internal static string UnescapeAll(string escapedString, out bool escapingWasNecessary)
        {
            //ErrorUtilities.VerifyThrow(escapedString != null, "Null strings not allowed.");
            escapingWasNecessary = false;
            int index = escapedString.IndexOf('%');
            if (index == -1)
            {
                return escapedString;
            }
            StringBuilder builder = new StringBuilder();
            int startIndex = 0;
            while (index != -1)
            {
                if (((index <= (escapedString.Length - 3)) && Uri.IsHexDigit(escapedString[index + 1])) && Uri.IsHexDigit(escapedString[index + 2]))
                {
                    builder.Append(escapedString, startIndex, index - startIndex);
                    char ch = (char)int.Parse(escapedString.Substring(index + 1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    builder.Append(ch);
                    startIndex = index + 3;
                    escapingWasNecessary = true;
                }
                index = escapedString.IndexOf('%', index + 1);
            }
            builder.Append(escapedString, startIndex, escapedString.Length - startIndex);
            return builder.ToString();
        }
    }


    /// <summary>
    /// Contains utility methods used by MSBuild. This class cannot be inherited.
    /// </summary>
    public static class Utilities
    {
        // Fields
        private static readonly Regex singlePropertyRegex = new Regex(@"^\$\(([^\$\(\)]*)\)$");
        private static readonly Regex xmlnsPattern = new Regex("xmlns=\"[^\"]*\"\\s*");

        // Methods
        internal static BuildEventFileInfo CreateBuildEventFileInfo(XmlNode xmlNode, string defaultFile)
        {
            ErrorUtilities.VerifyThrow(xmlNode != null, "Need Xml node.");
            int foundLineNumber = 0;
            int foundColumnNumber = 0;
            string xmlNodeFile = XmlUtilities.GetXmlNodeFile(xmlNode, string.Empty);
            if (xmlNodeFile.Length == 0)
            {
                xmlNodeFile = defaultFile;
            }
            else
            {
                XmlSearcher.GetLineColumnByNode(xmlNode, out foundLineNumber, out foundColumnNumber);
            }
            return new BuildEventFileInfo(xmlNodeFile, foundLineNumber, foundColumnNumber);
        }

        /// <summary>
        /// Converts the specified string into a syntax that allows the MSBuild engine to interpret 
        /// the character literally.
        /// </summary>
        /// <param name="unescapedExpression">The string to convert.</param>
        /// <returns>The converted value of the specified string.</returns>
        /// <remarks>Certain characters have special meaning in MSBuild project files. Examples of the 
        /// characters include semicolons (;) and asterisks (*). In order to use these special characters 
        /// as literals, they must be specified with the syntax %nn, where nn represents the ASCII 
        /// hexadecimal value of the character. This method performs that conversion.
        /// </remarks>
        public static string Escape(string unescapedExpression)
        {
            return EscapingUtilities.Escape(unescapedExpression);
        }

        /*internal static bool EvaluateCondition(string condition, XmlAttribute conditionAttribute, Expander expander, Hashtable conditionedPropertiesTable, ParserOptions itemListOptions)
        {
            bool flag;
            ErrorUtilities.VerifyThrow((conditionAttribute != null) || (condition.Length == 0), "If condition is non-empty, you must provide the XML node representing the condition.");
            if ((condition == null) || (condition.Length == 0))
            {
                return true;
            }
            Parser parser = new Parser();
            if (!parser.Parse(condition, conditionAttribute, itemListOptions).Evaluate(out flag, expander, conditionedPropertiesTable))
            {
                ProjectErrorUtilities.VerifyThrowInvalidProject(false, conditionAttribute, "ConditionNotBoolean", condition);
            }
            return flag;
        }

        internal static void GatherReferencedPropertyNames(string condition, XmlAttribute conditionAttribute, Expander expander, Hashtable conditionedPropertiesTable)
        {
            EvaluateCondition(condition, conditionAttribute, expander, conditionedPropertiesTable, ParserOptions.AllowItemLists | ParserOptions.AllowProperties);
        }

        internal static string GetDescriptiveErrorMessage(Exception e, Engine engine)
        {
            return e.Message;
        }*/

        internal static string GetXmlNodeInnerContents(XmlNode node)
        {
            string innerXml = node.InnerXml;
            if ((innerXml.IndexOf('<') != -1) && (innerXml.IndexOf("<![CDATA[", StringComparison.Ordinal) != 0))
            {
                return innerXml;
            }
            return node.InnerText;
        }

        internal static string RemoveXmlNamespace(string xml)
        {
            return xmlnsPattern.Replace(xml, string.Empty);
        }

        internal static void SetXmlNodeInnerContents(XmlNode node, string s)
        {
            ErrorUtilities.VerifyThrow(s != null, "Need value to set.");
            if (s.IndexOf('<') != -1)
            {
                try
                {
                    node.InnerXml = s;
                }
                catch (XmlException)
                {
                }
            }
            else
            {
                node.InnerText = s;
            }
        }

        internal static void UpdateConditionedPropertiesTable(Hashtable conditionedPropertiesTable, string leftValue, string rightValueExpanded)
        {
            if ((conditionedPropertiesTable != null) && (rightValueExpanded.Length > 0))
            {
                string[] strArray = leftValue.Split(new char[] { '|' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    Match match = singlePropertyRegex.Match(strArray[i]);
                    if (match.Success)
                    {
                        string str;
                        int index = rightValueExpanded.IndexOf('|');
                        if ((index == -1) || (i == (strArray.Length - 1)))
                        {
                            str = rightValueExpanded;
                            i = strArray.Length;
                        }
                        else
                        {
                            str = rightValueExpanded.Substring(0, index);
                            rightValueExpanded = rightValueExpanded.Substring(index + 1);
                        }
                        string str2 = match.Groups[1].ToString();
                        StringCollection strings = (StringCollection)conditionedPropertiesTable[str2];
                        if (strings == null)
                        {
                            strings = new StringCollection();
                            conditionedPropertiesTable[str2] = strings;
                        }
                        if (!strings.Contains(str))
                        {
                            strings.Add(str);
                        }
                    }
                }
            }
        }
    }
}