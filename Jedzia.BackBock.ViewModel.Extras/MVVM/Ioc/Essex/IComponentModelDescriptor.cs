namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public interface IComponentModelDescriptor
    {
        // Methods
        void BuildComponentModel(IKernel kernel, ComponentModel model);
        void ConfigureComponentModel(IKernel kernel, ComponentModel model);
    }
}