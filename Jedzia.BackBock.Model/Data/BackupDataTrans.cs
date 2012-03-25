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
            get 
            {
                return Validate();
            }
        }

        private string Validate()
        {
            //string result = null;
            if (this.BackupItem.Count == 0 && this.DatasetName == "Daily")
            {
                return "No BackupItems present and you call that 'Daily', you stupid?";
            }


            return null;
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
                    if (this.DatasetName.Contains("1"))
                        return "BASE: DatasetName cannot contain an 1.";
                    //if (this.DatasetName.Contains("y"))
                    //    return "BASE: What the ... Y?";
                }

                return null;
            }
        }

        #endregion
    }
}
