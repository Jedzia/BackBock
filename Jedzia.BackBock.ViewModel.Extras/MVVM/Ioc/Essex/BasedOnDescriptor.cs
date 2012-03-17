namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

    public class BasedOnDescriptor : IRegistration
    {

        // Fields
        private readonly Type basedOn;
        //private Action<ComponentRegistration> configuration;
        private readonly FromDescriptor from;
        private Predicate<Type> ifFilter;
        //private readonly ServiceDescriptor service;
        private Predicate<Type> unlessFilter;

        internal BasedOnDescriptor(Type basedOn, FromDescriptor from, Predicate<Type> additionalFilters)
        {
            this.basedOn = basedOn;
            this.from = from;
            //this.service = new ServiceDescriptor(this);
            this.If(additionalFilters);
        }

        public BasedOnDescriptor If(Predicate<Type> ifFilter)
        {
            this.ifFilter = (Predicate<Type>)Delegate.Combine(this.ifFilter, ifFilter);
            return this;
        }

        protected virtual bool Accepts(Type type, out Type[] baseTypes)
        {
            return IsBasedOn(type, out baseTypes)
                   && ExecuteIfCondition(type)
                   && ExecuteUnlessCondition(type) == false;
        }

        protected bool ExecuteIfCondition(Type type)
        {
            if (ifFilter == null)
            {
                return true;
            }

            foreach (Predicate<Type> filter in ifFilter.GetInvocationList())
            {
                if (filter(type) == false)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool ExecuteUnlessCondition(Type type)
        {
            if (unlessFilter == null)
            {
                return false;
            }
            foreach (Predicate<Type> filter in unlessFilter.GetInvocationList())
            {
                if (filter(type))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsBasedOn(Type type, out Type[] baseTypes)
        {
            if (basedOn.IsAssignableFrom(type))
            {
                baseTypes = new[] { basedOn };
                return true;
            }
            if (basedOn.IsGenericTypeDefinition)
            {
                if (basedOn.IsInterface)
                {
                    return IsBasedOnGenericInterface(type, out baseTypes);
                }
                return IsBasedOnGenericClass(type, out baseTypes);
            }
            baseTypes = new[] { basedOn };
            return false;
        }

        private bool IsBasedOnGenericClass(Type type, out Type[] baseTypes)
        {
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == basedOn)
                {
                    baseTypes = new[] { type };
                    return true;
                }

                type = type.BaseType;
            }
            baseTypes = null;
            return false;
        }

        private bool IsBasedOnGenericInterface(Type type, out Type[] baseTypes)
        {
            var types = new List<Type>(4);
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType &&
                    @interface.GetGenericTypeDefinition() == basedOn)
                {
                    if (@interface.ReflectedType == null &&
                        @interface.ContainsGenericParameters)
                    {
                        types.Add(@interface.GetGenericTypeDefinition());
                    }
                    else
                    {
                        types.Add(@interface);
                    }
                }
            }
            baseTypes = types.ToArray();
            return baseTypes.Length > 0;
        }


        internal bool TryRegister(Type type, IKernel kernel)
        {
            Type[] typeArray;
            if (!this.Accepts(type, out typeArray))
            {
                return false;
            }
            // Todo: implement BasedOn
            int x = 1;
            /*
            CastleComponentAttribute defaultsFor = CastleComponentAttribute.GetDefaultsFor(type);
            ICollection<Type> services = this.service.GetServices(type, typeArray);
            if ((services.Count == 0) && (defaultsFor.Services.Length > 0))
            {
                services = defaultsFor.Services;
            }
            ComponentRegistration registration = Component.For(services);
            registration.ImplementedBy(type);
            if (this.configuration != null)
            {
                this.configuration(registration);
            }
            if (string.IsNullOrEmpty(registration.Name) && !string.IsNullOrEmpty(defaultsFor.Name))
            {
                registration.Named(defaultsFor.Name);
            }
            else
            {
                registration.RegisterOptionally();
            }
            kernel.Register(new IRegistration[] { registration });*/
            return true;
        }

        void IRegistration.Register(IKernelInternal kernel)
        {
            ((IRegistration)this.from).Register(kernel);
        }
    }
}
