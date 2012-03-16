namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;

    public class Burden
    {
        private readonly IHandler handler;
        private Decommission decommission = Decommission.No;

        private List<Burden> dependencies;

        internal Burden(IHandler handler, bool requiresDecommission, bool trackedExternally)
        {
            this.handler = handler;
            TrackedExternally = trackedExternally;
            if (requiresDecommission)
            {
                decommission = Decommission.Yes;
            }
            /*else if (Model.Lifecycle.HasDecommissionConcerns)
            {
                if (Model.Implementation == typeof(LateBoundComponent) && Model.Lifecycle.DecommissionConcerns.All(IsLateBound))
                {
                    decommission = Decommission.LateBound;
                }
                else
                {
                    decommission = Decommission.Yes;
                }
            }*/
        }

        public IHandler Handler
        {
            get { return handler; }
        }

        public object Instance { get; private set; }

        /*public ComponentModel Model
        {
            get { return handler.ComponentModel; }
        }*/

        public bool RequiresDecommission
        {
            get { return decommission != Decommission.No; }
            set
            {
                if (value)
                {
                    decommission = Decommission.Yes;
                }
                else
                {
                    decommission = Decommission.No;
                }
            }
        }

        /// <summary>
        ///   If
        ///   <c>true</c>
        ///   requires release by
        ///   <see cref="IReleasePolicy" />
        ///   . If
        ///   <c>false</c>
        ///   , the object has a well defined, detectable end of life (web-request end, disposal of the container etc), and will be released externally.
        /// </summary>
        public bool RequiresPolicyRelease
        {
            get { return TrackedExternally == false && RequiresDecommission; }
        }

        public bool TrackedExternally { get; set; }

        public void AddChild(Burden child)
        {
            if (dependencies == null)
            {
                //dependencies = new List<Burden>(Model.Dependents.Length);
            }
            dependencies.Add(child);

            if (child.RequiresDecommission)
            {
                decommission = Decommission.Yes;
            }
        }

        public bool Release()
        {
            /*var releasing = Releasing;
            if (releasing != null)
            {
                releasing(this);
            }

            if (handler.Release(this) == false)
            {
                return false;
            }

            var released = Released;
            if (released != null)
            {
                released(this);
            }

            if (dependencies != null)
            {
                dependencies.ForEach(c => c.Release());
            }
            var graphReleased = GraphReleased;
            if (graphReleased != null)
            {
                graphReleased(this);
            }
            */
            return true;
        }

        public void SetRootInstance(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            Instance = instance;
            if (decommission == Decommission.LateBound)
            {
                // TODO: this may need to be extended if we lazily provide any other decimmission concerns
                RequiresDecommission = instance is IDisposable;
            }
        }

        /*private bool IsLateBound(IDecommissionConcern arg)
        {
            return arg is LateBoundConcerns<IDecommissionConcern>;
        }

        public event BurdenReleaseDelegate Released;
        public event BurdenReleaseDelegate Releasing;
        public event BurdenReleaseDelegate GraphReleased;*/

        private enum Decommission : byte
        {
            No,
            Yes,
            LateBound
        }
    }
}