namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public class ServicesDescriptor : IComponentModelDescriptor
    {
        private readonly Type[] services;

        public ServicesDescriptor(Type[] services)
        {
            this.services = services;
        }

        public void BuildComponentModel(IKernel kernel, ComponentModel model)
        {
            Array.ForEach(services, model.AddService);
        }

        public void ConfigureComponentModel(IKernel kernel, ComponentModel model)
        {
        }
    }
}