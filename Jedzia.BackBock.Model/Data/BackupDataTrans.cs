// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDataTrans.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Data
{
    using System.ComponentModel;

    /// <summary>
    /// Backup data Domain Object.
    /// </summary>
    public partial class BackupData : IDataErrorInfo
    {
        // Here goes bussiness code
        #region Properties

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error
        {
            get
            {
                return this.Validate();
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The name of the parameter to be checked.</param>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        public string this[string columnName]
        {
            get
            {
                if (columnName == "DatasetName")
                {
                    // if (this.Age < 0)
                    // return "Age cannot be less than 0.";

                    // if (120 < this.Age)
                    // return "Age cannot be greater than 120.";
                    if (this.DatasetName.Contains("1"))
                    {
                        return "BASE: DatasetName cannot contain an 1.";
                    }

                    // if (this.DatasetName.Contains("y"))
                    // return "BASE: What the ... Y?";
                }

                return null;
            }
        }

        #endregion

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>An error string or <c>null</c>, if there is no error.</returns>
        private string Validate()
        {
            // string result = null;
            if (this.BackupItem.Count == 0 && this.DatasetName == "Daily")
            {
                return "No BackupItems present and you call that 'Daily', you stupid?";
            }

            return null;
        }
    }
}