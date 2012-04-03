using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using Microsoft.Build.BuildEngine;

namespace Jedzia.BackBock.Model.Tasks
{
    public abstract class ItemGroupComposer
    {
        /// <summary>
        /// Generate a build item group for a given project and from the specified paths.
        /// </summary>
        /// <param name="proj">The project that hosts the build item.</param>
        /// <param name="paths">The paths to create the BuildItemGroup from.</param>
        /// <param name="sourceParameterIdentifier">The source parameter identifier.</param>
        /// <returns>A new BuildItemGroup with the name from <paramref name="sourceParameterIdentifier"/>
        /// and based on the specified paths.</returns>
        public abstract BuildItemGroup GenerateBuildItemGroup(Project proj, IEnumerable<PathDataType> paths, string sourceParameterIdentifier);
    }

    internal class FileItemGroupComposer : ItemGroupComposer
    {
        private const string allFilesPattern = "*.*";
        private const string fullrecursivePattern = @"\" + recursivePattern + @"\" + allFilesPattern;
        private const string recursivePattern = "**";

        /// <summary>
        /// Generate a build item group for a given project and from the specified paths.
        /// </summary>
        /// <param name="proj">The project that hosts the build item.</param>
        /// <param name="paths">The paths to create the BuildItemGroup from.</param>
        /// <param name="sourceParameterIdentifier">The source parameter identifier.</param>
        /// <returns>A new BuildItemGroup with the name from <paramref name="sourceParameterIdentifier"/>
        /// and based on the specified paths.</returns>
        public override BuildItemGroup GenerateBuildItemGroup(Project proj, IEnumerable<PathDataType> paths, string sourceParameterIdentifier)
        {
            return GenerateBuildItemGroupInternal(proj, paths, sourceParameterIdentifier);
        }

        private BuildItemGroup GenerateBuildItemGroupInternal(
        Project proj,
        IEnumerable<PathDataType> paths,
        string sourceParameterIdentifier)
        {
            var big = proj.AddNewItemGroup();
            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path.Path))
                {
                    // No valid path, move to the next one.
                    continue;
                }

                BuildItem cr;
                if (path.Inclusion.Count > 0)
                {
                    var itemInclude = ComposePathFromWildcards(path.Path, path.Inclusion);
                    cr = big.AddNewItem(sourceParameterIdentifier, itemInclude);
                    cr.Exclude = ComposePathFromWildcards(path.Path, path.Exclusion);
                }
                else
                {
                    var strit = string.Empty;
                    if (path.Path.EndsWith("\\"))
                    {
                        strit = path.Path + fullrecursivePattern;
                    }
                    else
                    {
                        strit = path.Path;
                    }

                    cr = big.AddNewItem(sourceParameterIdentifier, strit);
                    cr.Exclude = ComposePathFromWildcards(path.Path, path.Exclusion);
                }
            }
            return big;
        }







        private static string ComposePathFromWildcards(string fullPath, List<Wildcard> wildCards)
        {
            var inclEle = new StringBuilder();
            for (int index = 0; index < wildCards.Count; index++)
            {
                var incl = wildCards[index];
                inclEle.Append(fullPath);
                inclEle.Append(incl.Pattern);
                if (index != wildCards.Count - 1)
                {
                    inclEle.Append(";");
                }
            }

            var itemInclude = inclEle.ToString();
            return itemInclude;
        }





    }

}
