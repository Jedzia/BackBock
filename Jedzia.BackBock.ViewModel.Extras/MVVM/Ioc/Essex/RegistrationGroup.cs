namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public abstract class RegistrationGroup<S> where S : class
    {
        // Fields
        private readonly ComponentRegistration<S> registration;

        // Methods
        public RegistrationGroup(ComponentRegistration<S> registration)
        {
            this.registration = registration;
        }

        protected ComponentRegistration<S> AddAttributeDescriptor(string name, string value)
        {
            return this.registration.AddDescriptor(new AttributeDescriptor<S>(name, value));
        }

        protected ComponentRegistration<S> AddDescriptor(IComponentModelDescriptor descriptor)
        {
            return this.registration.AddDescriptor(descriptor);
        }

        // Properties
        public ComponentRegistration<S> Registration
        {
            get
            {
                return this.registration;
            }
        }
    }
}