namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public class DefaultComponentModelBuilder : IComponentModelBuilder
    {
        //private readonly List<IContributeComponentModelConstruction> contributors = new List<IContributeComponentModelConstruction>();
        private readonly IKernel kernel;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DefaultComponentModelBuilder" /> class.
        /// </summary>
        /// <param name = "kernel">The kernel.</param>
        public DefaultComponentModelBuilder(IKernel kernel)
        {
            this.kernel = kernel;
            //InitializeContributors();
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

        public ComponentModel BuildModel(IComponentModelDescriptor[] customContributors)
        {
            var model = new ComponentModel();
            Array.ForEach(customContributors, c => c.BuildComponentModel(kernel, model));
            //contributors.ForEach(c => c.ProcessModel(kernel, model));
            Array.ForEach(customContributors, c => c.ConfigureComponentModel(kernel, model));
            return model;
        }

        #endregion
    }
}