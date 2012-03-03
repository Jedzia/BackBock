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
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml;
    using Jedzia.BackBock.Tasks.Shared;
    using Jedzia.BackBock.Tasks.Utilities;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Text;

    internal class BuildPropertyGroup : List<string>
    {
    }

    internal class Expander
    {
        // Fields
        private Hashtable itemMetadata;
        private Hashtable primaryItemsByName;
        private BuildPropertyGroup properties;
        private Hashtable secondaryItemsByName;

        // Methods
        internal Expander(BuildPropertyGroup properties)
            : this(properties, null, null, null)
        {
        }

        internal Expander(BuildPropertyGroup properties, Hashtable primaryItemsByName)
            : this(properties, primaryItemsByName, null, null)
        {
        }

        internal Expander(BuildPropertyGroup properties, Hashtable primaryItemsByName, Hashtable secondaryItemsByName, Hashtable itemMetadata)
        {
            this.properties = properties;
            this.primaryItemsByName = primaryItemsByName;
            this.secondaryItemsByName = secondaryItemsByName;
            this.itemMetadata = itemMetadata;
        }

        internal string ExpandAllIntoString(XmlAttribute expressionAttribute)
        {
            return this.ExpandAllIntoString(expressionAttribute.Value, expressionAttribute);
        }

        internal string ExpandAllIntoString(string expression, XmlNode expressionNode)
        {
            return EscapingUtilities.UnescapeAll(this.ExpandAllIntoStringLeaveEscaped(expression, expressionNode));
        }

        internal string ExpandAllIntoStringLeaveEscaped(XmlAttribute expressionAttribute)
        {
            return this.ExpandAllIntoStringLeaveEscaped(expressionAttribute.Value, expressionAttribute);
        }

        internal string ExpandAllIntoStringLeaveEscaped(string expression, XmlNode expressionNode)
        {
            ErrorUtilities.VerifyThrow(expression != null, "Must pass in non-null expression.");
            if (expression.Length == 0)
            {
                return expression;
            }
            return this.ExpandItemsIntoStringLeaveEscaped(this.ExpandPropertiesLeaveEscaped(this.ExpandMetadataLeaveEscaped(expression)), expressionNode);
        }

        internal List<string> ExpandAllIntoStringList(XmlAttribute expressionAttribute)
        {
            return this.ExpandAllIntoStringList(expressionAttribute.Value, expressionAttribute);
        }

        internal List<string> ExpandAllIntoStringList(string expression, XmlNode expressionNode)
        {
            List<string> list = SplitSemiColonSeparatedList(this.ExpandAllIntoStringLeaveEscaped(expression, expressionNode));
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = EscapingUtilities.UnescapeAll(list[i]);
            }
            return list;
        }

        internal List<string> ExpandAllIntoStringListLeaveEscaped(XmlAttribute expressionAttribute)
        {
            return this.ExpandAllIntoStringListLeaveEscaped(expressionAttribute.Value, expressionAttribute);
        }

        internal List<string> ExpandAllIntoStringListLeaveEscaped(string expression, XmlNode expressionNode)
        {
            return SplitSemiColonSeparatedList(this.ExpandAllIntoStringLeaveEscaped(expression, expressionNode));
        }

        internal List<TaskItem> ExpandAllIntoTaskItems(string expression, XmlAttribute expressionAttribute)
        {
            List<TaskItem> list = new List<TaskItem>();
            string str = this.ExpandPropertiesLeaveEscaped(this.ExpandMetadataLeaveEscaped(expression));
            if (str.Length > 0)
            {
                foreach (string str2 in SplitSemiColonSeparatedList(str))
                {
                    BuildItemGroup group = this.ExpandSingleItemListExpressionIntoItemsLeaveEscaped(str2, expressionAttribute);
                    if (group != null)
                    {
                        foreach (BuildItem item in group)
                        {
                            list.Add(new TaskItem(item));
                        }
                        continue;
                    }
                    list.Add(new TaskItem(str2));
                }
            }
            return list;
        }

        private string ExpandItemsIntoStringLeaveEscaped(string expression, XmlNode expressionNode)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return expression;
            }
            ErrorUtilities.VerifyThrow((expressionNode != null) || ((this.primaryItemsByName == null) && (this.secondaryItemsByName == null)), "Must pass in non-null node if expression is not empty and item expansion is being performed.");
            if (this.secondaryItemsByName == null)
            {
                if (this.primaryItemsByName != null)
                {
                    return ItemExpander.ExpandEmbeddedItemVectors(expression, expressionNode, this.primaryItemsByName);
                }
                return expression;
            }
            ErrorUtilities.VerifyThrow(this.primaryItemsByName != null, "Should never have secondary items without primary items.");
            return ItemExpander.ExpandEmbeddedItemVectors(ItemExpander.ExpandEmbeddedItemVectors(expression, expressionNode, this.primaryItemsByName, true), expressionNode, this.secondaryItemsByName);
        }

        private string ExpandMetadataLeaveEscaped(string expression)
        {
            if (this.itemMetadata == null)
            {
                return expression;
            }
            if (expression.IndexOf("%(", StringComparison.Ordinal) == -1)
            {
                return expression;
            }
            if (expression.IndexOf("@(", StringComparison.Ordinal) == -1)
            {
                return ItemExpander.itemMetadataPattern.Replace(expression, new MatchEvaluator(this.ExpandSingleMetadata));
            }
            if (ItemExpander.listOfItemVectorsWithoutSeparatorsPattern.IsMatch(expression))
            {
                return expression;
            }
            return ItemExpander.nonTransformItemMetadataPattern.Replace(expression, new MatchEvaluator(this.ExpandSingleMetadata));
        }

        private string ExpandPropertiesLeaveEscaped(string sourceString)
        {
            int num2;
            ErrorUtilities.VerifyThrow(this.properties != null, "Need a property bag.");
            if (sourceString.IndexOf("$(", StringComparison.Ordinal) == -1)
            {
                return sourceString;
            }
            StringBuilder builder = new StringBuilder(null);
            int startIndex = 0;
            while ((num2 = sourceString.IndexOf("$(", startIndex, StringComparison.Ordinal)) != -1)
            {
                builder.Append(sourceString, startIndex, num2 - startIndex);
                startIndex = num2;
                int num3 = sourceString.IndexOf(")", num2, StringComparison.Ordinal);
                if (num3 == -1)
                {
                    builder.Append(sourceString, num2, sourceString.Length - num2);
                    startIndex = sourceString.Length;
                }
                else
                {
                    // Todo: commented out
                    /* string finalValueEscaped;
                    string str = sourceString.Substring(num2 + 2, (num3 - num2) - 2);
                    BuildProperty property = this.properties[str];
                    if (property == null)
                    {
                        finalValueEscaped = string.Empty;
                    }
                    else
                    {
                        finalValueEscaped = property.FinalValueEscaped;
                    }
                    builder.Append(finalValueEscaped);
                    startIndex = num3 + 1;*/
                }
            }
            builder.Append(sourceString, startIndex, sourceString.Length - startIndex);
            return builder.ToString();
        }

        internal BuildItemGroup ExpandSingleItemListExpressionIntoItemsLeaveEscaped(string singleItemVectorExpression, XmlAttribute itemVectorAttribute)
        {
            Match match;
            if (this.primaryItemsByName == null)
            {
                return null;
            }
            return this.ExpandSingleItemListExpressionIntoItemsLeaveEscaped(singleItemVectorExpression, itemVectorAttribute, out match);
        }

        internal BuildItemGroup ExpandSingleItemListExpressionIntoItemsLeaveEscaped(string singleItemVectorExpression, XmlAttribute itemVectorAttribute, out Match itemVectorMatch)
        {
            ErrorUtilities.VerifyThrow(this.primaryItemsByName != null, "Expander has not been properly initialized.");
            return ItemExpander.ItemizeItemVector(singleItemVectorExpression, itemVectorAttribute, this.primaryItemsByName, this.secondaryItemsByName, out itemVectorMatch);
        }

        private string ExpandSingleMetadata(Match itemMetadataMatch)
        {
            ErrorUtilities.VerifyThrow(itemMetadataMatch.Success, "Need a valid item metadata.");
            string str = itemMetadataMatch.Groups["NAME"].Value;
            string str2 = str;
            if (itemMetadataMatch.Groups["ITEM_SPECIFICATION"].Length > 0)
            {
                str2 = itemMetadataMatch.Groups["TYPE"].Value + "." + str;
            }
            string str3 = (string)this.itemMetadata[str2];
            if (str3 == null)
            {
                str3 = string.Empty;
            }
            return str3;
        }

        private static List<string> SplitSemiColonSeparatedList(string expression)
        {
            string str;
            List<string> list = new List<string>();
            int startIndex = 0;
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case '\'':
                        if (flag)
                        {
                            flag2 = !flag2;
                        }
                        break;

                    case ')':
                        if (flag && !flag2)
                        {
                            flag = false;
                        }
                        break;

                    case ';':
                        if (!flag)
                        {
                            str = expression.Substring(startIndex, i - startIndex).Trim();
                            if (str.Length > 0)
                            {
                                list.Add(str);
                            }
                            startIndex = i + 1;
                        }
                        break;

                    case '@':
                        if ((expression.Length > (i + 1)) && (expression[i + 1] == '('))
                        {
                            flag = true;
                        }
                        break;
                }
            }
            str = expression.Substring(startIndex, expression.Length - startIndex).Trim();
            if (str.Length > 0)
            {
                list.Add(str);
            }
            return list;
        }

        internal static List<string> SplitSemiColonSeparatedList_ForUnitTestsOnly(string expression)
        {
            return SplitSemiColonSeparatedList(expression);
        }

        // Properties
        internal Hashtable ItemMetadata
        {
            get
            {
                return this.itemMetadata;
            }
        }

        internal Hashtable PrimaryItemsByName
        {
            get
            {
                return this.primaryItemsByName;
            }
        }

        internal BuildPropertyGroup Properties
        {
            get
            {
                return this.properties;
            }
        }

        internal Hashtable SecondaryItemsByName
        {
            get
            {
                return this.secondaryItemsByName;
            }
        }
    }

}