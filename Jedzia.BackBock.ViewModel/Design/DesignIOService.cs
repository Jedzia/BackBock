using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Design
{
    public class DesignIOService : IOService
    {
        private const string error = "DesignIOService does not provide any IO-Operations!";

        #region IOService Members

        public string OpenFileDialog(string defaultPath)
        {
            throw new NotImplementedException(error);
        }

        public string SaveFileDialog(string defaultPath)
        {
            throw new NotImplementedException(error);
        }

        public System.IO.Stream OpenFile(string path)
        {
            throw new NotImplementedException(error);
        }

        #endregion
    }
}
