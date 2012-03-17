using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    /// <summary>
    /// Summary
    /// </summary>
    public interface IDataProvider
    {
        string GetTitle();
    }

    class DataProvider : IDataProvider
    {
        #region IDataProvider Members

        public string GetTitle()
        {
            return "There is work to do!";
        }

        #endregion
    }

    class DesignDataProvider : IDataProvider
    {
        #region IDataProvider Members

        public string GetTitle()
        {
            return "We are in DesignMode";
        }

        #endregion
    }

}
