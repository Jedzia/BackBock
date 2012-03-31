// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Testdata.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Data.Xml
{
    using System;
    using System.Linq;
    using Jedzia.BackBock.Data.Xml.XmlData;
    using Jedzia.BackBock.DataAccess;

    /// <summary>
    /// A <see cref="BackupDataRepository"/> that provides test data. 
    /// </summary>
    [Obsolete("Remove this provider after testing")]
    public class TestBackupDataRepository : BackupDataRepository, IDisposable
    {
        // ...or make a initial-setup / sample-data provider of it.
        #region Fields

        /// <summary>
        /// Internal data source.
        /// </summary>
        private DesignDataProvider d = new DesignDataProvider();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the repository.
        /// </summary>
        /// <value>
        /// The type of the repository.
        /// </value>
        public override BackupRepositoryType RepositoryType
        {
            get
            {
                return BackupRepositoryType.Static;
            }
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.d = null;
        }

        /// <summary>
        /// Gets the backup data.
        /// </summary>
        /// <returns>
        /// The backup data.
        /// </returns>
        public override DataAccess.DTO.BackupData GetBackupData()
        {
            var data = this.d.GenerateSampleData();
            var lst = data.BackupItem.ToList();
            lst.Insert(0, new BackupItemType { ItemName = "This is from Xml Test Data" });
            data.BackupItem = lst.ToArray();
            return data.ToHostType();
        }
    }
}