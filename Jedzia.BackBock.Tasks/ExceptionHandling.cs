namespace Jedzia.BackBock.Tasks
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security;

    internal static class ExceptionHandling
    {
        // Methods
        internal static bool IsCriticalException(Exception e)
        {
            return (((e is StackOverflowException) || (e is OutOfMemoryException)) || ((e is ExecutionEngineException) || (e is AccessViolationException)));
        }

        internal static bool NotExpectedException(Exception e)
        {
            return (((!(e is UnauthorizedAccessException) && !(e is ArgumentNullException)) && (!(e is PathTooLongException) && !(e is DirectoryNotFoundException))) && ((!(e is NotSupportedException) && !(e is ArgumentException)) && (!(e is SecurityException) && !(e is IOException))));
        }

        internal static bool NotExpectedReflectionException(Exception e)
        {
            return ((((!(e is TypeLoadException) && !(e is MethodAccessException)) && (!(e is MissingMethodException) && !(e is MemberAccessException))) && ((!(e is BadImageFormatException) && !(e is ReflectionTypeLoadException)) && (!(e is CustomAttributeFormatException) && !(e is TargetParameterCountException)))) && (((!(e is InvalidCastException) && !(e is AmbiguousMatchException)) && (!(e is InvalidFilterCriteriaException) && !(e is TargetException))) && (!(e is MissingFieldException) && NotExpectedException(e))));
        }

        internal static void RethrowUnlessFileIO(Exception e)
        {
            if (((!(e is UnauthorizedAccessException) && !(e is ArgumentNullException)) && (!(e is PathTooLongException) && !(e is DirectoryNotFoundException))) && ((!(e is NotSupportedException) && !(e is ArgumentException)) && (!(e is SecurityException) && !(e is IOException))))
            {
                throw e;
            }
        }
    }
}