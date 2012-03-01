namespace Jedzia.BackBock.Tasks.Shared
{
    using System.Xml;

    internal static class ProjectErrorUtilities
    {
        // Methods
        private static void ThrowInvalidProject(string errorSubCategoryResourceName, XmlNode xmlNode, string resourceName, params object[] args)
        {
            string errorSubcategory = null;
            string str2;
            string str3;
            if (errorSubCategoryResourceName != null)
            {
                errorSubcategory = AssemblyResources.GetString(errorSubCategoryResourceName);
            }
            string message = ResourceUtilities.FormatResourceString(out str2, out str3, resourceName, args);
            throw new InvalidProjectFileException(xmlNode, message, errorSubcategory, str2, str3);
        }

        internal static void VerifyThrowInvalidProject(bool condition, XmlNode xmlNode, string resourceName)
        {
            VerifyThrowInvalidProject(condition, null, xmlNode, resourceName);
        }

        internal static void VerifyThrowInvalidProject(bool condition, string errorSubCategoryResourceName, XmlNode xmlNode, string resourceName)
        {
            if (!condition)
            {
                ThrowInvalidProject(errorSubCategoryResourceName, xmlNode, resourceName, null);
            }
        }

        internal static void VerifyThrowInvalidProject(bool condition, XmlNode xmlNode, string resourceName, object arg0)
        {
            VerifyThrowInvalidProject(condition, null, xmlNode, resourceName, arg0);
        }

        internal static void VerifyThrowInvalidProject(bool condition, string errorSubCategoryResourceName, XmlNode xmlNode, string resourceName, object arg0)
        {
            if (!condition)
            {
                ThrowInvalidProject(errorSubCategoryResourceName, xmlNode, resourceName, new object[] { arg0 });
            }
        }

        internal static void VerifyThrowInvalidProject(bool condition, XmlNode xmlNode, string resourceName, object arg0, object arg1)
        {
            VerifyThrowInvalidProject(condition, null, xmlNode, resourceName, arg0, arg1);
        }

        internal static void VerifyThrowInvalidProject(bool condition, string errorSubCategoryResourceName, XmlNode xmlNode, string resourceName, object arg0, object arg1)
        {
            if (!condition)
            {
                ThrowInvalidProject(errorSubCategoryResourceName, xmlNode, resourceName, new object[] { arg0, arg1 });
            }
        }

        internal static void VerifyThrowInvalidProject(bool condition, XmlNode xmlNode, string resourceName, object arg0, object arg1, object arg2)
        {
            VerifyThrowInvalidProject(condition, null, xmlNode, resourceName, arg0, arg1, arg2);
        }

        internal static void VerifyThrowInvalidProject(bool condition, string errorSubCategoryResourceName, XmlNode xmlNode, string resourceName, object arg0, object arg1, object arg2)
        {
            if (!condition)
            {
                ThrowInvalidProject(errorSubCategoryResourceName, xmlNode, resourceName, new object[] { arg0, arg1, arg2 });
            }
        }

        internal static void VerifyThrowInvalidProject(bool condition, XmlNode xmlNode, string resourceName, object arg0, object arg1, object arg2, object arg3)
        {
            VerifyThrowInvalidProject(condition, null, xmlNode, resourceName, arg0, arg1, arg2, arg3);
        }

        internal static void VerifyThrowInvalidProject(bool condition, string errorSubCategoryResourceName, XmlNode xmlNode, string resourceName, object arg0, object arg1, object arg2, object arg3)
        {
            if (!condition)
            {
                ThrowInvalidProject(errorSubCategoryResourceName, xmlNode, resourceName, new object[] { arg0, arg1, arg2, arg3 });
            }
        }
    }
}