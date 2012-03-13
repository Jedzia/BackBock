namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections;

    public class ExtendedPropertiesDescriptor : IComponentModelDescriptor
    {
        private readonly IDictionary dictionary;
        private readonly Property[] properties;

        public ExtendedPropertiesDescriptor(params Property[] properties)
        {
            this.properties = properties;
        }

        public ExtendedPropertiesDescriptor(IDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public void BuildComponentModel(IKernel kernel, ComponentModel model)
        {
        }

        public void ConfigureComponentModel(IKernel kernel, ComponentModel model)
        {
            if (dictionary != null)
            {
                foreach (DictionaryEntry property in dictionary)
                {
                    model.ExtendedProperties[property.Key] = property.Value;
                }
            }
            if (properties != null)
            {
                Array.ForEach(properties, p => model.ExtendedProperties[p.Key] = p.Value);
            }
        }
    }
}