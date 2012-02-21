using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.Tasks
{
    public interface ITask
    {
        string Name { get; }
        bool Execute();
    }
}
