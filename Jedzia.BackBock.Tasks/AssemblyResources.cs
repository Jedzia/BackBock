namespace Jedzia.BackBock.Tasks
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class AssemblyResources
    {
        // Fields
        private static readonly ResourceManager resources = new ResourceManager("Microsoft.Build.Tasks.Strings", Assembly.GetExecutingAssembly());
        private static readonly ResourceManager sharedResources = new ResourceManager("Microsoft.Build.Tasks.Strings.shared", Assembly.GetExecutingAssembly());

        // Methods
        internal static string GetString(string name)
        {
            string str = resources.GetString(name, CultureInfo.CurrentUICulture);
            if (str == null)
            {
                str = sharedResources.GetString(name, CultureInfo.CurrentUICulture);
            }
            return str;
        }

        // Properties
        internal static ResourceManager PrimaryResources
        {
            get
            {
                return resources;
            }
        }

        internal static ResourceManager SharedResources
        {
            get
            {
                return sharedResources;
            }
        }
    }
}