namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    public class AttributeDescriptor<S> : AbstractOverwriteableDescriptor<S> where S : class
    {
        // Fields
        private readonly string name;
        private readonly string value;

        // Methods
        public AttributeDescriptor(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        protected override void ApplyToConfiguration(IKernel kernel, IConfiguration configuration)
        {
            // Todo: not implemented
            /*if ((configuration.Attributes[this.name] == null) || base.IsOverWrite)
            {
                configuration.Attributes[this.name] = this.value;
            }*/
        }
    }
}