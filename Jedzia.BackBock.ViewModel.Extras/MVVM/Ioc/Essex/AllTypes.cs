using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    public static class AllTypes
    {
        public static FromAssemblyDescriptor FromThisAssembly()
        {
            return Classes.FromAssembly(Assembly.GetCallingAssembly());
        }
    }
}
