using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel;
using Microsoft.Win32;

namespace Jedzia.BackBock.Application
{
    class FileIOService : IOService
    {
        #region IOService Members

        public string OpenFileDialog(string defaultPath)
        {
            var opf = new OpenFileDialog();
            if (!string.IsNullOrEmpty(defaultPath))
            {
                opf.FileName = defaultPath;
            }
            opf.ShowDialog();
            return opf.FileName;
        }

        public System.IO.Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public string SaveFileDialog(string defaultPath)
        {
            var opf = new SaveFileDialog();
            if (!string.IsNullOrEmpty(defaultPath))
            {
                opf.FileName = defaultPath;
            }
            opf.ShowDialog();
            return opf.FileName;
        }

        public System.IO.Stream SaveFile(string path)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
