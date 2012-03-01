namespace Jedzia.BackBock.Tasks
{
    using System.Collections.Generic;

    public interface ITaskService //: IServiceProvider
    {
        bool Register(ITask task);
        ITask this[string taskName] { get; /*set;*/ }
        void Reset();
        void ResetAll();
        IEnumerable<string> GetRegisteredTasks();
    }
}