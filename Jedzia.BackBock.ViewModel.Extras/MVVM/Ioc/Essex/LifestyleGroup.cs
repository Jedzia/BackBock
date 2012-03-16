namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public class LifestyleGroup<TService> : RegistrationGroup<TService>
        where TService : class
    {
        public LifestyleGroup(ComponentRegistration<TService> registration)
            : base(registration)
        {
        }
        
        public ComponentRegistration<TService> Transient
        {
            get
            {
                return base.AddDescriptor(new LifestyleDescriptor<TService>(LifestyleType.Transient));
            }
        }
    }
}