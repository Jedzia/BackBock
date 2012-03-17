namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;


    /// <summary>
    ///   Selects a set of types from an assembly.
    /// </summary>
    public class FromAssemblyDescriptor : FromDescriptor
    {
        private readonly IEnumerable<Assembly> assemblies;
        private bool nonPublicTypes;

        internal FromAssemblyDescriptor(Assembly assembly, Predicate<Type> additionalFilters)
            : base(additionalFilters)
        {
            assemblies = new[] { assembly };
        }

        internal FromAssemblyDescriptor(IEnumerable<Assembly> assemblies, Predicate<Type> additionalFilters)
            : base(additionalFilters)
        {
            this.assemblies = assemblies;
        }

        /// <summary>
        ///   When called also non-public types will be scanned.
        /// </summary>
        /// <remarks>
        ///   Usually it is not recommended to register non-public types in the container so think twice before using this option.
        /// </remarks>
        public FromAssemblyDescriptor IncludeNonPublicTypes()
        {
            nonPublicTypes = true;
            return this;
        }

        protected override IEnumerable<Type> SelectedTypes(IKernel kernel)
        {
            if (this.nonPublicTypes)
            {
                return Enumerable.SelectMany<Assembly, Type>(this.assemblies, delegate(Assembly assembly)
                {
                    return assembly.GetTypes();
                });
            }
            return Enumerable.SelectMany<Assembly, Type>(this.assemblies, delegate(Assembly assembly)
            {
                return assembly.GetExportedTypes();
            });
        }

        /*protected override IEnumerable<Type> SelectedTypes(IKernel kernel)
        {
            return assemblies.SelectMany(a => a.GetAvailableTypes(nonPublicTypes));
        }*/
    }

}
