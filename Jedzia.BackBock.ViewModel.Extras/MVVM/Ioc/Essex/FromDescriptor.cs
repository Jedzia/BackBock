namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

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
}