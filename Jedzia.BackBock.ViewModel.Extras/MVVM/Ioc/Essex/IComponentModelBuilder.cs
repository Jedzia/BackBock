namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    /// <summary>
    ///   Implementors must construct a populated
    ///   instance of ComponentModel by inspecting the component
    ///   and|or the configuration.
    /// </summary>
    public interface IComponentModelBuilder
    {
        //IContributeComponentModelConstruction[] Contributors { get; }

        /// <summary>
        ///   "To give or supply in common with others; give to a 
        ///   common fund or for a common purpose". The contributor
        ///   should inspect the component, or even the configuration
        ///   associated with the component, to add or change information
        ///   in the model that can be used later.
        /// </summary>
        //void AddContributor(IContributeComponentModelConstruction contributor);

        /// <summary>
        ///   Constructs a new ComponentModel by invoking
        ///   the registered contributors.
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "services"></param>
        /// <param name = "classType"></param>
        /// <param name = "extendedProperties"></param>
        /// <returns></returns>
        //ComponentModel BuildModel(ComponentName name, Type[] services, Type classType, IDictionary extendedProperties);

        ComponentModel BuildModel(IComponentModelDescriptor[] customContributors);

        /// <summary>
        ///   Removes the specified contributor
        /// </summary>
        /// <param name = "contributor"></param>
        //void RemoveContributor(IContributeComponentModelConstruction contributor);
    }
}