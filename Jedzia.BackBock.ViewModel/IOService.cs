using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Jedzia.BackBock.ViewModel
{
    public interface IOService
    {
        string OpenFileDialog(string defaultPath);

        //Other similar untestable IO operations
        Stream OpenFile(string path);
    }
}
