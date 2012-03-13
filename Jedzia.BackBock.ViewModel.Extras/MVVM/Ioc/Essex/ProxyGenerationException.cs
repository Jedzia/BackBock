namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public class ProxyGenerationException : Exception
    {
        public ProxyGenerationException(string message)
            : base(message)
        {
        }

        public ProxyGenerationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}