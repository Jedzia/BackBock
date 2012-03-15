namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;

    public class ContainerInjector : IContainerInjector
    {
        #region IContainerInjector Members

        public virtual void AddCustomComponent(ComponentModel model, bool isMetaHandler)
        {
            RegisterSimpleIoc(model);
        }

        private void RegisterSimpleIoc(ComponentModel model)
        {
            foreach (var item in model.Services)
            {
                SimpleIoc.Default.Register(item, model.Implementation, new SingletonInstance());
            }
        }

        #endregion
    }
}
