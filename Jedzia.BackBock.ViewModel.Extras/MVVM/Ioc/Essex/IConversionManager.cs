namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public interface IConversionManager
    {
        T PerformConversion<T>(string initialRaw);
    }
}