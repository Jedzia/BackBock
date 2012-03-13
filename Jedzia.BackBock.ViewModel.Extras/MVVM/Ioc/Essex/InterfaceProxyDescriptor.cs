namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public class InterfaceProxyDescriptor : IComponentModelDescriptor
    {
        public void BuildComponentModel(IKernel kernel, ComponentModel model)
        {
        }

        public void ConfigureComponentModel(IKernel kernel, ComponentModel model)
        {
            if (model.HasInterceptors && model.Implementation.IsInterface)
            {
                //var options = model.ObtainProxyOptions();
                //options.OmitTarget = true;
            }
        }
    }
}