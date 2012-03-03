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




    public class ItemExpander
    {
        // Fields
        private bool ignoreUnknownItemTypes;
        private Hashtable itemGroupsByType;
        public static readonly Regex itemMetadataPattern = new Regex("%\\(\\s*\r\n                (?<ITEM_SPECIFICATION>(?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\\s*\\.\\s*)?\r\n                (?<NAME>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n            \\s*\\)", RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        public const string itemMetadataPrefix = "%(";
        private const string itemMetadataSpecification = "%\\(\\s*\r\n                (?<ITEM_SPECIFICATION>(?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\\s*\\.\\s*)?\r\n                (?<NAME>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n            \\s*\\)";
        private BuildItem itemUnderTransformation;
        public static readonly Regex itemVectorPattern = new Regex("@\\(\\s*\r\n                (?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n                (?<TRANSFORM_SPECIFICATION>\\s*->\\s*'(?<TRANSFORM>[^']*)')?\r\n                (?<SEPARATOR_SPECIFICATION>\\s*,\\s*'(?<SEPARATOR>[^']*)')?\r\n            \\s*\\)", RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        public const string itemVectorPrefix = "@(";
        private const string itemVectorSpecification = "@\\(\\s*\r\n                (?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n                (?<TRANSFORM_SPECIFICATION>\\s*->\\s*'(?<TRANSFORM>[^']*)')?\r\n                (?<SEPARATOR_SPECIFICATION>\\s*,\\s*'(?<SEPARATOR>[^']*)')?\r\n            \\s*\\)";
        private const string itemVectorWithoutSeparatorSpecification = "@\\(\\s*\r\n                (?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n                (?<TRANSFORM_SPECIFICATION>\\s*->\\s*'(?<TRANSFORM>[^']*)')?\r\n            \\s*\\)";
        private const string itemVectorWithTransformLHS = @"@\(\s*[A-Za-z_][A-Za-z_0-9\-]*\s*->\s*'[^']*";
        private const string itemVectorWithTransformRHS = @"[^']*'(\s*,\s*'[^']*')?\s*\)";
        public static readonly Regex listOfItemVectorsWithoutSeparatorsPattern = new Regex("^\\s*(;\\s*)*(@\\(\\s*\r\n                (?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n                (?<TRANSFORM_SPECIFICATION>\\s*->\\s*'(?<TRANSFORM>[^']*)')?\r\n            \\s*\\)\\s*(;\\s*)*)+$", RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        public static readonly Regex nonTransformItemMetadataPattern = new Regex("((?<=@\\(\\s*[A-Za-z_][A-Za-z_0-9\\-]*\\s*->\\s*'[^']*)%\\(\\s*\r\n                (?<ITEM_SPECIFICATION>(?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\\s*\\.\\s*)?\r\n                (?<NAME>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n            \\s*\\)(?![^']*'(\\s*,\\s*'[^']*')?\\s*\\))) |\r\n                        ((?<!@\\(\\s*[A-Za-z_][A-Za-z_0-9\\-]*\\s*->\\s*'[^']*)%\\(\\s*\r\n                (?<ITEM_SPECIFICATION>(?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\\s*\\.\\s*)?\r\n                (?<NAME>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n            \\s*\\)(?=[^']*'(\\s*,\\s*'[^']*')?\\s*\\))) |\r\n                        ((?<!@\\(\\s*[A-Za-z_][A-Za-z_0-9\\-]*\\s*->\\s*'[^']*)%\\(\\s*\r\n                (?<ITEM_SPECIFICATION>(?<TYPE>[A-Za-z_][A-Za-z_0-9\\-]*)\\s*\\.\\s*)?\r\n                (?<NAME>[A-Za-z_][A-Za-z_0-9\\-]*)\r\n            \\s*\\)(?![^']*'(\\s*,\\s*'[^']*')?\\s*\\)))", RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        private XmlNode parentNode;

        // Methods
        private ItemExpander()
        {
        }

        private ItemExpander(XmlNode parentNode, Hashtable itemGroupsByType)
            : this(parentNode, itemGroupsByType, false)
        {
        }

        private ItemExpander(XmlNode parentNode, Hashtable itemGroupsByType, bool ignoreUnknownItemTypes)
        {
            this.parentNode = parentNode;
            this.itemGroupsByType = itemGroupsByType;
            this.ignoreUnknownItemTypes = ignoreUnknownItemTypes;
        }

        public static string ExpandEmbeddedItemVectors(string s, XmlNode parentNode, Hashtable itemsByType)
        {
            return ExpandEmbeddedItemVectors(s, parentNode, itemsByType, false);
        }

        public static string ExpandEmbeddedItemVectors(string s, XmlNode parentNode, Hashtable itemsByType, bool ignoreUnknownItemTypes)
        {
            if (s.IndexOf('@') != -1)
            {
                ItemExpander expander = new ItemExpander(parentNode, itemsByType, ignoreUnknownItemTypes);
                return itemVectorPattern.Replace(s, new MatchEvaluator(expander.ExpandItemVectorPossiblyIgnoringUnknownItemTypes));
            }
            return s;
        }

        private string ExpandItemMetadata(Match itemMetadataMatch)
        {
            ErrorUtilities.VerifyThrow(this.itemUnderTransformation != null, "Need item to get metadata value from.");
            string str = itemMetadataMatch.Groups["NAME"].Value;
            ProjectErrorUtilities.VerifyThrowInvalidProject(itemMetadataMatch.Groups["ITEM_SPECIFICATION"].Length == 0, this.parentNode, "QualifiedAttributeInTransformNotAllowed", itemMetadataMatch.Value, str);
            string evaluatedMetadataEscaped = null;
            try
            {
                evaluatedMetadataEscaped = this.itemUnderTransformation.GetEvaluatedMetadataEscaped(str);
            }
            catch (InvalidOperationException exception)
            {
                ProjectErrorUtilities.VerifyThrowInvalidProject(false, this.parentNode, "CannotEvaluateItemAttribute", str, exception.Message);
            }
            return evaluatedMetadataEscaped;
        }

        private string ExpandItemVector(Match itemVector, out bool isUnknownItemType)
        {
            ErrorUtilities.VerifyThrow(itemVector.Success, "Need a valid item vector.");
            string str = (itemVector.Groups["SEPARATOR_SPECIFICATION"].Length != 0) ? itemVector.Groups["SEPARATOR"].Value : ";";
            BuildItemGroup group = this.ItemizeItemVector(itemVector, out isUnknownItemType);
            if (group.Count <= 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < group.Count; i++)
            {
                builder.Append(group[i].FinalItemSpecEscaped);
                if (i < (group.Count - 1))
                {
                    builder.Append(str);
                }
            }
            return builder.ToString();
        }

        private string ExpandItemVectorPossiblyIgnoringUnknownItemTypes(Match itemVector)
        {
            bool flag;
            string str = this.ExpandItemVector(itemVector, out flag);
            if (this.ignoreUnknownItemTypes && flag)
            {
                return itemVector.Value;
            }
            return str;
        }

        private BuildItemGroup ItemizeItemVector(Match itemVector, out bool isUnknownItemType)
        {
            ErrorUtilities.VerifyThrow(itemVector.Success, "Need a valid item vector.");
            ErrorUtilities.VerifyThrow(this.itemGroupsByType != null, "Received a null itemGroupsByType!");
            string str = itemVector.Groups["TYPE"].Value;
            string input = (itemVector.Groups["TRANSFORM_SPECIFICATION"].Length > 0) ? itemVector.Groups["TRANSFORM"].Value : null;
            BuildItemGroup group = (BuildItemGroup)this.itemGroupsByType[str];
            if (group == null)
            {
                isUnknownItemType = true;
                group = new BuildItemGroup();
            }
            else
            {
                isUnknownItemType = false;
                group = group.Clone(input != null);
            }
            if (input != null)
            {
                foreach (BuildItem item in group)
                {
                    this.itemUnderTransformation = item;
                    item.SetFinalItemSpecEscaped(itemMetadataPattern.Replace(input, new MatchEvaluator(this.ExpandItemMetadata)));
                }
            }
            return group;
        }

        public static BuildItemGroup ItemizeItemVector(string itemVectorExpression, XmlNode parentNode, Hashtable itemsByType)
        {
            Match match;
            return ItemizeItemVector(itemVectorExpression, parentNode, itemsByType, null, out match);
        }

        public static BuildItemGroup ItemizeItemVector(string itemVectorExpression, XmlNode parentNode, Hashtable bucketedItemsByType, Hashtable fullProjectItemsByType, out Match itemVectorMatch)
        {
            ErrorUtilities.VerifyThrow((bucketedItemsByType != null) || (fullProjectItemsByType != null), "We have to have a set of items somewhere.");
            itemVectorMatch = null;
            BuildItemGroup group = null;
            if (itemVectorExpression.IndexOf('@') != -1)
            {
                bool flag;
                itemVectorMatch = itemVectorPattern.Match(itemVectorExpression);
                if (!itemVectorMatch.Success)
                {
                    return group;
                }
                ProjectErrorUtilities.VerifyThrowInvalidProject(itemVectorMatch.Value == itemVectorExpression, parentNode, "EmbeddedItemVectorCannotBeItemized", itemVectorExpression);
                ItemExpander expander = new ItemExpander(parentNode, bucketedItemsByType);
                if (itemVectorMatch.Groups["SEPARATOR_SPECIFICATION"].Length > 0)
                {
                    string itemInclude = expander.ExpandItemVector(itemVectorMatch, out flag);
                    if (flag && (fullProjectItemsByType != null))
                    {
                        expander = new ItemExpander(parentNode, fullProjectItemsByType);
                        itemInclude = expander.ExpandItemVector(itemVectorMatch, out flag);
                    }
                    string itemName = itemVectorMatch.Groups["TYPE"].Value;
                    group = new BuildItemGroup();
                    if (itemInclude.Length > 0)
                    {
                        group.AddNewItem(itemName, itemInclude);
                    }
                }
                else
                {
                    group = expander.ItemizeItemVector(itemVectorMatch, out flag);
                    if (flag && (fullProjectItemsByType != null))
                    {
                        group = new ItemExpander(parentNode, fullProjectItemsByType).ItemizeItemVector(itemVectorMatch, out flag);
                    }
                }
                ErrorUtilities.VerifyThrow(group != null, "ItemizeItemVector shouldn't give us null.");
            }
            return group;
        }
    }



}