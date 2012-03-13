namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    /// <summary>
    /// Provides the implementation of a destroy method for lifetime management 
    /// of <see cref="SimpleIoc"/> container objects.
    /// </summary>
    public interface ILifetimeManagement
    {
        void DoDestroy();
        //void DoRelease(object o, EventArgs e);
    }
}