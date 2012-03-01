using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.TaskTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = TaskRegistry.GetInstance();
            var task = service["Backup"];
        }
    }
}
