using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    /// <summary>
    ///   Factory for creating <see cref = "ComponentRegistration" /> objects. Use static methods on the class to fluently build registration.
    /// </summary>
    public static class Component
    {
        /// <summary>
        ///   Creates a component registration for the service type.
        /// </summary>
        /// <typeparam name = "TService">The service type.</typeparam>
        /// <returns>The component registration.</returns>
        public static ComponentRegistration<TService> For<TService>()
            where TService : class
        {
            return new ComponentRegistration<TService>();
        }

        public static Predicate<Type> IsInNamespace(string @namespace)
        {
            return IsInNamespace(@namespace, false);
        }
        public static Predicate<Type> IsInNamespace(string @namespace, bool includeSubnamespaces)
        {
            Predicate<Type> predicate = null;
            if (!includeSubnamespaces)
            {
                return type => (type.Namespace == @namespace);
            }
            if (predicate == null)
            {
                predicate = type => (type.Namespace == @namespace) || ((type.Namespace != null) && type.Namespace.StartsWith(@namespace + "."));
            }
            return predicate;
        }

        public static Predicate<Type> IsInSameNamespaceAs<T>()
        {
            return IsInSameNamespaceAs(typeof(T));
        }

        public static Predicate<Type> IsInSameNamespaceAs<T>(bool includeSubnamespaces)
        {
            return IsInSameNamespaceAs(typeof(T), includeSubnamespaces);
        }

        public static Predicate<Type> IsInSameNamespaceAs(Type type)
        {
            return IsInNamespace(type.Namespace);
        }

        public static Predicate<Type> IsInSameNamespaceAs(Type type, bool includeSubnamespaces)
        {
            return IsInNamespace(type.Namespace, includeSubnamespaces);
        }



    }

}
