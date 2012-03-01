namespace Jedzia.BackBock.Tasks.Shared
{
    using System;

    internal static class ErrorUtilities
    {
        // Methods
        private static void ThrowArgument(Exception innerException, string resourceName, params object[] args)
        {
            throw new ArgumentException(resourceName, innerException);
            //throw new ArgumentException(ResourceUtilities.FormatResourceString(resourceName, args), innerException);
        }

        private static void ThrowInternalError(bool showAssert, string unformattedMessage, params object[] args)
        {
            throw new NotImplementedException(unformattedMessage);
            //throw new InternalErrorException(ResourceUtilities.FormatString(unformattedMessage, args), showAssert);
        }

        private static void ThrowInvalidOperation(string resourceName, params object[] args)
        {
            throw new InvalidOperationException(resourceName);
            //throw new InvalidOperationException(ResourceUtilities.FormatResourceString(resourceName, args));
        }

        internal static void VerifyThrow(bool condition, string unformattedMessage)
        {
            if (!condition)
            {
                ThrowInternalError(true, unformattedMessage, null);
            }
        }

        internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0)
        {
            if (!condition)
            {
                ThrowInternalError(true, unformattedMessage, new object[] { arg0 });
            }
        }

        internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0, object arg1)
        {
            if (!condition)
            {
                ThrowInternalError(true, unformattedMessage, new object[] { arg0, arg1 });
            }
        }

        internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0, object arg1, object arg2)
        {
            if (!condition)
            {
                ThrowInternalError(true, unformattedMessage, new object[] { arg0, arg1, arg2 });
            }
        }

        internal static void VerifyThrow(bool condition, string unformattedMessage, object arg0, object arg1, object arg2, object arg3)
        {
            if (!condition)
            {
                ThrowInternalError(true, unformattedMessage, new object[] { arg0, arg1, arg2, arg3 });
            }
        }

        internal static void VerifyThrowArgument(bool condition, string resourceName)
        {
            VerifyThrowArgument(condition, null, resourceName);
        }

        internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName)
        {
            if (!condition)
            {
                ThrowArgument(innerException, resourceName, null);
            }
        }

        internal static void VerifyThrowArgument(bool condition, string resourceName, object arg0)
        {
            VerifyThrowArgument(condition, null, resourceName, arg0);
        }

        internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName, object arg0)
        {
            if (!condition)
            {
                ThrowArgument(innerException, resourceName, new object[] { arg0 });
            }
        }

        internal static void VerifyThrowArgument(bool condition, string resourceName, object arg0, object arg1)
        {
            VerifyThrowArgument(condition, null, resourceName, arg0, arg1);
        }

        internal static void VerifyThrowArgument(bool condition, Exception innerException, string resourceName, object arg0, object arg1)
        {
            if (!condition)
            {
                ThrowArgument(innerException, resourceName, new object[] { arg0, arg1 });
            }
        }

        internal static void VerifyThrowArgumentLength(string parameter, string parameterName)
        {
            VerifyThrowArgumentNull(parameter, parameterName);
            if (parameter.Length == 0)
            {
                throw new ArgumentException("Shared.ParameterCannotHaveZeroLength");
                //throw new ArgumentException(ResourceUtilities.FormatResourceString("Shared.ParameterCannotHaveZeroLength", new object[] { parameterName }));
            }
        }

        internal static void VerifyThrowArgumentNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("Shared.ParameterCannotBeNull");
                //throw new ArgumentNullException(ResourceUtilities.FormatResourceString("Shared.ParameterCannotBeNull", new object[] { parameterName }), null);
            }
        }

        internal static void VerifyThrowArgumentOutOfRange(bool condition, string parameterName)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        internal static void VerifyThrowInvalidOperation(bool condition, string resourceName)
        {
            if (!condition)
            {
                ThrowInvalidOperation(resourceName, null);
            }
        }

        internal static void VerifyThrowInvalidOperation(bool condition, string resourceName, object arg0)
        {
            if (!condition)
            {
                ThrowInvalidOperation(resourceName, new object[] { arg0 });
            }
        }

        internal static void VerifyThrowInvalidOperation(bool condition, string resourceName, object arg0, object arg1)
        {
            if (!condition)
            {
                ThrowInvalidOperation(resourceName, new object[] { arg0, arg1 });
            }
        }

        internal static void VerifyThrowInvalidOperation(bool condition, string resourceName, object arg0, object arg1, object arg2)
        {
            if (!condition)
            {
                ThrowInvalidOperation(resourceName, new object[] { arg0, arg1, arg2 });
            }
        }

        internal static void VerifyThrowNoAssert(bool condition, string unformattedMessage)
        {
            if (!condition)
            {
                ThrowInternalError(false, unformattedMessage, null);
            }
        }
    }
}