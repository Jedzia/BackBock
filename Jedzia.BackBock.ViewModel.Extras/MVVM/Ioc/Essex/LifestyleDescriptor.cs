namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public class LifestyleDescriptor<S> : AbstractOverwriteableDescriptor<S>
        where S : class
    {
        private readonly LifestyleType lifestyle;

        public LifestyleDescriptor(LifestyleType lifestyle)
        {
            this.lifestyle = lifestyle;
        }

        protected override void ApplyToConfiguration(IKernel kernel, IConfiguration configuration)
        {
            if (configuration.Attributes["lifestyle"] == null || IsOverWrite)
            {
                configuration.Attributes["lifestyle"] = lifestyle.ToString();
            }
        }
    }
}