namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex.Conversion
{
    using System;

    public static class ExceptionHelper
    {
        public static Exception SetUp(this Exception exception)
        {
#if !SILVERLIGHT
            // SL doesn't have  that peroperty
            exception.HelpLink = Constants.ExceptionHelpLink;
#endif
            return exception;
        }
    }
}