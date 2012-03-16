namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Uses the ConfigurationStore registered in the kernel to obtain
    ///   an <see cref = "IConfiguration" /> associated with the component.
    /// </summary>
    [Serializable]
    public class ConfigurationModelInspector : IContributeComponentModelConstruction
    {
        /// <summary>
        ///   Queries the kernel's ConfigurationStore for a configuration
        ///   associated with the component name.
        /// </summary>
        /// <param name = "kernel"></param>
        /// <param name = "model"></param>
        public virtual void ProcessModel(IKernel kernel, ComponentModel model)
        {
            var config = kernel.ConfigurationStore.GetComponentConfiguration(model.Name);
            model.Configuration = config;
        }
    }
}