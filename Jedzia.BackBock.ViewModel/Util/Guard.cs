using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

namespace Jedzia.BackBock.ViewModel.Util
{
    [DebuggerStepThrough]
    internal static class Guard
    {
        private const string ArgumentCannotBeEmpty = "Argument cannot be null or empty";
        private const string TypeNotImplementInterface = "{0} does not implement {1}";
        private const string TypeNotInheritFromType = "{0} does not inherite from {1}";
        private const string CannotEqual = "Value cannot equal the default";

        public static void NotDefault<T>(Expression<Func<T>> reference, T value)
        {


            if (value.Equals(default(T)))
                throw new ArgumentException(CannotEqual, GetParameterName(reference));
        }

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not null.
        /// Throws <see cref="ArgumentNullException"/> otherwise.
        /// </summary>
        public static void NotNull<T>(Expression<Func<T>> reference, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(GetParameterName(reference));
            }
        }

        /// <summary>
        /// Ensures the given string <paramref name="value"/> is not null or empty.
        /// Throws <see cref="ArgumentNullException"/> in the first case, or 
        /// <see cref="ArgumentException"/> in the latter.
        /// </summary>
        public static void NotNullOrEmpty(Expression<Func<string>> reference, string value)
        {
            NotNull<string>(reference, value);
            if (value.Length == 0)
            {
                throw new ArgumentException(ArgumentCannotBeEmpty, GetParameterName(reference));
            }
        }

        /// <summary>
        /// Checks an argument to ensure it is in the specified range including the edges.
        /// </summary>
        /// <typeparam name="T">Type of the argument to check, it must be an <see cref="IComparable"/> type.
        /// </typeparam>
        /// <param name="reference">The expression containing the name of the argument.</param>
        /// <param name="value">The argument value to check.</param>
        /// <param name="from">The minimun allowed value for the argument.</param>
        /// <param name="to">The maximun allowed value for the argument.</param>
        public static void NotOutOfRangeInclusive<T>(Expression<Func<T>> reference, T value, T from, T to)
                        where T : IComparable
        {
            if (value != null && (value.CompareTo(from) < 0 || value.CompareTo(to) > 0))
            {
                throw new ArgumentOutOfRangeException(GetParameterName(reference));
            }
        }

        /// <summary>
        /// Checks an argument to ensure it is in the specified range excluding the edges.
        /// </summary>
        /// <typeparam name="T">Type of the argument to check, it must be an <see cref="IComparable"/> type.
        /// </typeparam>
        /// <param name="reference">The expression containing the name of the argument.</param>
        /// <param name="value">The argument value to check.</param>
        /// <param name="from">The minimun allowed value for the argument.</param>
        /// <param name="to">The maximun allowed value for the argument.</param>
        public static void NotOutOfRangeExclusive<T>(Expression<Func<T>> reference, T value, T from, T to)
                        where T : IComparable
        {
            if (value != null && (value.CompareTo(from) <= 0 || value.CompareTo(to) >= 0))
            {
                throw new ArgumentOutOfRangeException(GetParameterName(reference));
            }
        }

        public static void CanBeAssigned(Expression<Func<object>> reference, Type typeToAssign, Type targetType)
        {
            if (!targetType.IsAssignableFrom(typeToAssign))
            {
                if (targetType.IsInterface)
                {
                    throw new ArgumentException(string.Format(
                            CultureInfo.CurrentCulture,
                            TypeNotImplementInterface,
                            typeToAssign,
                            targetType), GetParameterName(reference));
                }

                throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        TypeNotInheritFromType,
                        typeToAssign,
                        targetType), GetParameterName(reference));
            }
        }

        private static string GetParameterName(LambdaExpression reference)
        {
            var member = (MemberExpression)reference.Body;
            return member.Member.Name;
        }
    }
}