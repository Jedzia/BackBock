namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public abstract class Constants
    {
        private const string defaultComponentForServiceFilter = "castle.default-component-for-service-filter";
        private const string helpLink = @"groups.google.com/group/castle-project-users";

        private const string propertyFilters = "castle.property-filters";
        private const string scopeAccessorType = "castle.scope-accessor-type";
        private const string scopeRootSelector = "castle.scope-root";

        public static object DefaultComponentForServiceFilter
        {
            get { return defaultComponentForServiceFilter; }
        }

        public static string ExceptionHelpLink
        {
            get { return helpLink; }
        }

        public static object PropertyFilters
        {
            get { return propertyFilters; }
        }

        public static object ScopeAccessorType
        {
            get { return scopeAccessorType; }
        }

        public static object ScopeRootSelector
        {
            get { return scopeRootSelector; }
        }
    }
}