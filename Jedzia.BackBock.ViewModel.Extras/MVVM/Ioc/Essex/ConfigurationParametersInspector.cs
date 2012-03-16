namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    ///   Check for a node 'parameters' within the component 
    ///   configuration. For each child it, a ParameterModel is created
    ///   and added to ComponentModel's Parameters collection
    /// </summary>
    [Serializable]
    public class ConfigurationParametersInspector : IContributeComponentModelConstruction
    {
        /// <summary>
        ///   Inspect the configuration associated with the component
        ///   and populates the parameter model collection accordingly
        /// </summary>
        /// <param name = "kernel"></param>
        /// <param name = "model"></param>
        public virtual void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (model.Configuration == null)
            {
                return;
            }

            var parameters = model.Configuration.Children["parameters"];
            if (parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters.Children)
            {
                var name = parameter.Name;
                var value = parameter.Value;

                if (value == null && parameter.Children.Count != 0)
                {
                    var parameterValue = parameter.Children[0];
                    //model.Parameters.Add(name, parameterValue);
                }
                else
                {
                    //model.Parameters.Add(name, value);
                }
            }

            // Experimental code
            //InspectCollections(model);
        }

        /*private void AddAnyServiceOverrides(ComponentModel model, IConfiguration config, ParameterModel parameter)
        {
            foreach (var item in config.Children)
            {
                if (item.Children.Count > 0)
                {
                    AddAnyServiceOverrides(model, item, parameter);
                }

                var componentName = ReferenceExpressionUtil.ExtractComponentName(item.Value);
                if (componentName == null)
                {
                    continue;
                }
                model.Dependencies.Add(new ComponentDependencyModel(componentName));
            }
        }*/

        /*private void InspectCollections(ComponentModel model)
        {
            foreach (var parameter in model.Parameters)
            {
                if (parameter.ConfigValue != null)
                {
                    if (IsArray(parameter) || IsList(parameter))
                    {
                        AddAnyServiceOverrides(model, parameter.ConfigValue, parameter);
                    }
                }

                if (ReferenceExpressionUtil.IsReference(parameter.Value))
                {
                    model.Dependencies.Add(new DependencyModel(parameter.Name, null, false));
                }
            }
        }*/

        /*private bool IsArray(ParameterModel parameter)
        {
            return parameter.ConfigValue.Name.EqualsText("array");
        }

        private bool IsList(ParameterModel parameter)
        {
            return parameter.ConfigValue.Name.EqualsText("list");
        }*/
    }
}