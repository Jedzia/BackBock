namespace Jedzia.BackBock.Model.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Jedzia.BackBock.Model.Data;
    using Microsoft.Build.BuildEngine;

    /// <summary>
    /// Creates item groups from <see cref="PathDataType"/>'s.
    /// </summary>
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

    /// <summary>
    /// Creates file or directory based item groups from <see cref="PathDataType"/>'s.
    /// </summary>
    internal class FileItemGroupComposer : ItemGroupComposer
    {
        private const string AllFilesPattern = "*.*";
        private const string FullRecursivePattern = @"\" + RecursivePattern + @"\" + AllFilesPattern;
        private const string RecursivePattern = "**";

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

        private static string BuildPathFromWildcards(string fullPath, List<Wildcard> wildCards)
        {
            var itemInclude = new StringBuilder();
            for (int index = 0; index < wildCards.Count; index++)
            {
                var incl = wildCards[index];
                itemInclude.Append(fullPath);
                itemInclude.Append(incl.Pattern);
                if (index != wildCards.Count - 1)
                {
                    itemInclude.Append(";");
                }
            }

            return itemInclude.ToString();
        }

        /// <summary>
        /// Generate a build item group for a given project and from the specified paths.
        /// </summary>
        /// <param name="proj">The project that hosts the build item.</param>
        /// <param name="paths">The paths to create the BuildItemGroup from.</param>
        /// <param name="sourceParameterIdentifier">The source parameter identifier.</param>
        /// <returns>
        /// A new BuildItemGroup with the name from <paramref name="sourceParameterIdentifier"/>
        /// and based on the specified paths.
        /// </returns>
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
                    var itemInclude = BuildPathFromWildcards(path.Path, path.Inclusion);
                    cr = big.AddNewItem(sourceParameterIdentifier, itemInclude);
                    cr.Exclude = BuildPathFromWildcards(path.Path, path.Exclusion);
                }
                else
                {
                    var strit = string.Empty;
                    if (path.Path.EndsWith("\\"))
                    {
                        strit = path.Path + FullRecursivePattern;
                    }
                    else
                    {
                        strit = path.Path;
                    }

                    cr = big.AddNewItem(sourceParameterIdentifier, strit);
                    cr.Exclude = BuildPathFromWildcards(path.Path, path.Exclusion);
                }
            }

            return big;
        }
    }
}
