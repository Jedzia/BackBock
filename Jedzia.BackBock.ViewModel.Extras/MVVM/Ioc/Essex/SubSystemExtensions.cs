namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public static class SubSystemExtensions
    {
        public static IConversionManager GetConversionManager(this IKernel kernel)
        {
            return (IConversionManager)kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
        }
    }
}