using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Jedzia.BackBock.Model.Data
{
    public partial class BackupData : IDataErrorInfo 
    {
        #region IDataErrorInfo Members

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DatasetName")
                {
                    //if (this.Age < 0)
                    //    return "Age cannot be less than 0.";

                    //if (120 < this.Age)
                    //   return "Age cannot be greater than 120.";
                    if (this.DatasetName.Contains("x"))
                        return "DatasetName cannot contain an x.";
                    if (this.DatasetName.Contains("y"))
                        return "What the ... Y?";
                }

                return null;
            }
        }

        #endregion
    }
}
