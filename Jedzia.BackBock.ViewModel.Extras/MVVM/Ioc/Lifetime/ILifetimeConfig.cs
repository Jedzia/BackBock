namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    using System;

    /// <summary>
    /// Provides the fluent configuration interface for the <see cref="LifetimeManager:T"/> 
    /// of <see cref="SimpleIoc"/> container objects.
    /// </summary>
    /// <typeparam name="T">The object to provide with configuration abilities.</typeparam>
    public interface ILifetimeConfig<T>
    {
        void OnDestroy(Action<T> action);
        //void DestroyOnEvent(Delegate handler);
        //void DestroyOnEvent(Action<ILifetimeManagement> lf);
    }
}