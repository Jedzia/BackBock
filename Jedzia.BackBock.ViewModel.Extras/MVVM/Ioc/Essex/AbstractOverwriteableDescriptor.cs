namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public abstract class AbstractOverwriteableDescriptor<TService> : IComponentModelDescriptor
        where TService : class
    {
        protected bool IsOverWrite
        {
            //get { return Registration.IsOverWrite; }
            get { return false; }
        }

        internal ComponentRegistration<TService> Registration { private get; set; }

        public virtual void BuildComponentModel(IKernel kernel, ComponentModel model)
        {
            ApplyToConfiguration(kernel, model.Configuration);
        }

        public virtual void ConfigureComponentModel(IKernel kernel, ComponentModel model)
        {
        }

        protected virtual void ApplyToConfiguration(IKernel kernel, IConfiguration configuration)
        {
        }
    }
}