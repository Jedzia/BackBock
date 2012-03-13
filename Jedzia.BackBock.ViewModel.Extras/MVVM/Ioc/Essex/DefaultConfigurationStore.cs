namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public class DefaultConfigurationStore : /*AbstractSubSystem,*/ IConfigurationStore, ISubSystem
    {
        #region ISubSystem Members

        public void Init(IKernelInternal kernel)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}