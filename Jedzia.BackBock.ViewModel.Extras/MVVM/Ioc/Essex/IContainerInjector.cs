namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public interface IContainerInjector
    {
        void AddCustomComponent(ComponentModel model, bool isMetaHandler);
    }
}