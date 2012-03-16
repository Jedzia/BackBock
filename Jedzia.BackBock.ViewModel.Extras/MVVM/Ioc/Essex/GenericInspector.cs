namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Linq;

    [Serializable]
    public class GenericInspector : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            model.RequiresGenericArguments = model.Services.Any(s => s.IsGenericTypeDefinition);
        }
    }
}