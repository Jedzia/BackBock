namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Exception threw when there is a problem
    ///   registering a component
    /// </summary>
    [Serializable]
    public class ComponentRegistrationException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ComponentRegistrationException" /> class.
        /// </summary>
        /// <param name = "message">The message.</param>
        public ComponentRegistrationException(string message)
            : base(message)
        {
            //ExceptionHelper.SetUp(this);
        }

        public ComponentRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
            //ExceptionHelper.SetUp(this);
        }
    }
}