namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.ComponentModel;

    /// <summary>
    ///   Inspects the type looking for interfaces that constitutes
    ///   lifecycle interfaces, defined in the Castle.Model namespace.
    /// </summary>
    [Serializable]
    public class LifecycleModelInspector : IContributeComponentModelConstruction
    {
        /// <summary>
        ///   Checks if the type implements <see cref = "IInitializable" /> and or
        ///   <see cref = "IDisposable" /> interfaces.
        /// </summary>
        /// <param name = "kernel"></param>
        /// <param name = "model"></param>
        public virtual void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            if (IsLateBoundComponent(model))
            {
                //ProcessLateBoundModel(model);
                return;
            }
            ProcessModel(model);
        }

        private bool IsLateBoundComponent(ComponentModel model)
        {
            return typeof(LateBoundComponent) == model.Implementation;
        }

        /*private void ProcessLateBoundModel(ComponentModel model)
        {
            var commission = new LateBoundCommissionConcerns();
            if (model.Services.Any(s => s.Is<IInitializable>()))
            {
                model.Lifecycle.Add(InitializationConcern.Instance);
            }
            else
            {
                commission.AddConcern<IInitializable>(InitializationConcern.Instance);
            }
            if (model.Services.Any(s => s.Is<ISupportInitialize>()))
            {
                model.Lifecycle.Add(SupportInitializeConcern.Instance);
            }
            else
            {
                commission.AddConcern<ISupportInitialize>(SupportInitializeConcern.Instance);
            }
            if (commission.HasConcerns)
            {
                model.Lifecycle.Add(commission);
            }

            if (model.Services.Any(s => s.Is<IDisposable>()))
            {
                model.Lifecycle.Add(DisposalConcern.Instance);
            }
            else
            {
                var decommission = new LateBoundDecommissionConcerns();
                decommission.AddConcern<IDisposable>(DisposalConcern.Instance);
                model.Lifecycle.Add(decommission);
            }
        }*/

        private void ProcessModel(ComponentModel model)
        {
            if (model.Implementation.Is<IInitializable>())
            {
                //model.Lifecycle.Add(InitializationConcern.Instance);
            }
            if (model.Implementation.Is<ISupportInitialize>())
            {
                //model.Lifecycle.Add(SupportInitializeConcern.Instance);
            }
            if (model.Implementation.Is<IDisposable>())
            {
                //model.Lifecycle.Add(DisposalConcern.Instance);
            }
        }
    }
}