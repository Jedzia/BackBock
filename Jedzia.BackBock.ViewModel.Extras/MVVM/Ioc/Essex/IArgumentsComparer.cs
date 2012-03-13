namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public interface IArgumentsComparer
    {
        // Methods
        bool RunEqualityComparison(object x, object y, out bool areEqual);
        bool RunHasCodeCalculation(object o, out int hashCode);
    }
}