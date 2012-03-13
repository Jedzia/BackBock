namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public interface ISubSystem
    {
        // Methods
        void Init(IKernelInternal kernel);
        void Terminate();
    }
}