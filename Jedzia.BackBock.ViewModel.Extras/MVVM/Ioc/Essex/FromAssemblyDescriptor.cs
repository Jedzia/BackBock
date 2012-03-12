using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{

    public abstract class FromDescriptor : IRegistration
    {
        // Fields
        private readonly Predicate<Type> additionalFilters;
        private bool allowMultipleMatches;
        private readonly IList<BasedOnDescriptor> criterias;

        // Methods
        internal FromDescriptor(Predicate<Type> additionalFilters)
        {
            this.additionalFilters = additionalFilters;
            this.allowMultipleMatches = false;
            this.criterias = new List<BasedOnDescriptor>();
        }

        public FromDescriptor AllowMultipleMatches()
        {
            this.allowMultipleMatches = true;
            return this;
        }

        public BasedOnDescriptor BasedOn<T>()
        {
            return this.BasedOn(typeof(T));
        }

        public BasedOnDescriptor BasedOn(Type basedOn)
        {
            BasedOnDescriptor item = new BasedOnDescriptor(basedOn, this, this.additionalFilters);
            this.criterias.Add(item);
            return item;
        }

        void IRegistration.Register(IKernelInternal kernel)
        {
            if (this.criterias.Count != 0)
            {
                foreach (Type type in this.SelectedTypes(kernel))
                {
                    foreach (BasedOnDescriptor descriptor in this.criterias)
                    {
                        if (descriptor.TryRegister(type, kernel) && !this.allowMultipleMatches)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public BasedOnDescriptor InNamespace(string @namespace)
        {
            return this.Where(Component.IsInNamespace(@namespace, false));
        }

        public BasedOnDescriptor InNamespace(string @namespace, bool includeSubnamespaces)
        {
            return this.Where(Component.IsInNamespace(@namespace, includeSubnamespaces));
        }

        public BasedOnDescriptor InSameNamespaceAs<T>()
        {
            return this.Where(Component.IsInSameNamespaceAs<T>());
        }

        public BasedOnDescriptor InSameNamespaceAs<T>(bool includeSubnamespaces) where T : class
        {
            return this.Where(Component.IsInSameNamespaceAs<T>(includeSubnamespaces));
        }

        public BasedOnDescriptor InSameNamespaceAs(Type type)
        {
            return this.Where(Component.IsInSameNamespaceAs(type));
        }

        public BasedOnDescriptor InSameNamespaceAs(Type type, bool includeSubnamespaces)
        {
            return this.Where(Component.IsInSameNamespaceAs(type, includeSubnamespaces));
        }

        public BasedOnDescriptor Pick()
        {
            return this.BasedOn<object>();
        }

        protected abstract IEnumerable<Type> SelectedTypes(IKernel kernel);
        public BasedOnDescriptor Where(Predicate<Type> accepted)
        {
            BasedOnDescriptor item = new BasedOnDescriptor(typeof(object), this, this.additionalFilters).If(accepted);
            this.criterias.Add(item);
            return item;
        }
    }


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
            return assemblies.SelectMany(a => a.GetAvailableTypes(nonPublicTypes));
        }
    }

}
