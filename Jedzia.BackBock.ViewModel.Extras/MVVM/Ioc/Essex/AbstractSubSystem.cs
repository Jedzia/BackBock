namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    [Serializable]
#if (SILVERLIGHT)
	public abstract class AbstractSubSystem : ISubSystem
#else
    public abstract class AbstractSubSystem : MarshalByRefObject, ISubSystem
#endif
    {
        private IKernelInternal kernel;

#if (!SILVERLIGHT)
#if DOTNET40
		[SecurityCritical]
#endif
        public override object InitializeLifetimeService()
        {
            return null;
        }
#endif

        public virtual void Init(IKernelInternal kernel)
        {
            this.kernel = kernel;
        }

        public virtual void Terminate()
        {
        }

        protected IKernelInternal Kernel
        {
            get { return kernel; }
        }
    }
}