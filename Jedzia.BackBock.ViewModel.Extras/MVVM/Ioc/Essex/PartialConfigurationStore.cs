namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    internal class PartialConfigurationStore : IConfigurationStore, ISubSystem, IDisposable
    {
        // Fields
        private readonly IConfigurationStore inner;
        private readonly IConfigurationStore partial;

        public PartialConfigurationStore(IKernelInternal kernel)
        {
            //this.inner = kernel.ConfigurationStore;
            this.partial = new DefaultConfigurationStore();
            //this.partial.Init(kernel);
        }

        #region ISubSystem Members

        public void Init(IKernelInternal kernel)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            // Todo: Implement
            //throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Terminate();
        }

        #endregion
    }
}