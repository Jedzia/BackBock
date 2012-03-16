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
            this.inner = kernel.ConfigurationStore;
            this.partial = new DefaultConfigurationStore();
            this.partial.Init(kernel);
        }
        public void AddComponentConfiguration(String key, IConfiguration config)
        {
            inner.AddComponentConfiguration(key, config);
            partial.AddComponentConfiguration(key, config);
        }

        public void Init(IKernelInternal kernel)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            // Todo: Implement
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Terminate();
        }

        public IConfiguration GetComponentConfiguration(string key)
        {
            throw new NotImplementedException();
        }
    }
}