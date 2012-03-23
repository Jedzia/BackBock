using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Design
{
    public class DesignSettingsProvider : ISettingsProvider
    {

        #region ISettingsProvider Members

        public string GetStartupDataFile()
        {
            return "This is a dummy string from: GetStartupDataFile() "
                + " Jedzia.BackBock.ViewModel.Design.DesignSettingsProvider";
        }

        #endregion
    }
}
