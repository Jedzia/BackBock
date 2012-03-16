namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;

    public class ContainerInjector : IContainerInjector
    {
        #region IContainerInjector Members

        public virtual void AddCustomComponent(ComponentModel model, bool isMetaHandler)
        {
            RegisterSimpleIoc(model);
        }

        /// <exception cref="ArgumentOutOfRangeException"><c></c> is out of range.</exception>
        private void RegisterSimpleIoc(ComponentModel model)
        {
            foreach (var item in model.Services)
            {
                InstanceLifetime lifetime;
                switch (model.LifestyleType)
                {
                    case LifestyleType.Undefined:
                    case LifestyleType.Singleton:
                        lifetime = new SingletonInstance();
                        break;
                    case LifestyleType.Thread:
                        throw new ArgumentOutOfRangeException();
                        break;
                    case LifestyleType.Transient:
                        lifetime = new TransitionLifetime();
                        break;
                    case LifestyleType.Pooled:
                        throw new ArgumentOutOfRangeException();
                        break;
                    case LifestyleType.PerWebRequest:
                        throw new ArgumentOutOfRangeException();
                        break;
                    case LifestyleType.Custom:
                        throw new ArgumentOutOfRangeException();
                        break;
                    case LifestyleType.Scoped:
                        throw new ArgumentOutOfRangeException();
                        break;
                    case LifestyleType.Bound:
                        throw new ArgumentOutOfRangeException();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                SimpleIoc.Default.Register(item, model.Implementation, lifetime);
            }
        }

        #endregion
    }
}
