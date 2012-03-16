namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

    public class DefaultComponentModelBuilder : IComponentModelBuilder
    {
        private readonly List<IContributeComponentModelConstruction> contributors = new List<IContributeComponentModelConstruction>();
        private readonly IKernel kernel;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DefaultComponentModelBuilder" /> class.
        /// </summary>
        /// <param name = "kernel">The kernel.</param>
        public DefaultComponentModelBuilder(IKernel kernel)
        {
            this.kernel = kernel;
            InitializeContributors();
        }

        #region IComponentModelBuilder Members

        /*/// <summary>
        ///   Constructs a new ComponentModel by invoking
        ///   the registered contributors.
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "services"></param>
        /// <param name = "classType"></param>
        /// <param name = "extendedProperties"></param>
        /// <returns></returns>
        public ComponentModel BuildModel(ComponentName name, Type[] services, Type classType, IDictionary extendedProperties)
        {
            var model = new ComponentModel(name, services, classType, extendedProperties);
            //contributors.ForEach(c => c.ProcessModel(kernel, model));

            return model;
        }*/
        /// <summary>
        ///   "To give or supply in common with others; give to a
        ///   common fund or for a common purpose". The contributor
        ///   should inspect the component, or even the configuration
        ///   associated with the component, to add or change information
        ///   in the model that can be used later.
        /// </summary>
        /// <param name = "contributor"></param>
        public void AddContributor(IContributeComponentModelConstruction contributor)
        {
            contributors.Add(contributor);
        }

        /// <summary>
        ///   Initializes the default contributors.
        /// </summary>
        protected virtual void InitializeContributors()
        {
            var conversionManager = kernel.GetConversionManager();
            //IConversionManager conversionManager = null;
            AddContributor(new GenericInspector());
            AddContributor(new ConfigurationModelInspector());
            AddContributor(new ConfigurationParametersInspector());
            AddContributor(new LifestyleModelInspector(conversionManager));
            //AddContributor(new ConstructorDependenciesModelInspector());
            //AddContributor(new PropertiesDependenciesModelInspector(conversionManager));
            AddContributor(new LifecycleModelInspector());
            //AddContributor(new InterceptorInspector());
            //AddContributor(new MixinInspector());
            //AddContributor(new ComponentActivatorInspector(conversionManager));
            //AddContributor(new ComponentProxyInspector(conversionManager));
        }

        public ComponentModel BuildModel(IComponentModelDescriptor[] customContributors)
        {
            var model = new ComponentModel();
            Array.ForEach(customContributors, c => c.BuildComponentModel(kernel, model));
            contributors.ForEach(c => c.ProcessModel(kernel, model));
            Array.ForEach(customContributors, c => c.ConfigureComponentModel(kernel, model));
            return model;
        }

        #endregion
    }
}