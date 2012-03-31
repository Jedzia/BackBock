// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignBackupDataRepository.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Design.Data
{
    using System;
    using Jedzia.BackBock.DataAccess;
    using Jedzia.BackBock.Model.Data;

    /// <summary>
    /// Design-Time <see cref="BackupDataRepository"/> fake.
    /// </summary>
    internal class DesignBackupDataRepository : IDisposable
    {
        #region Fields

        private DesignDataProvider d = new DesignDataProvider();

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
        /// <returns>A set of Design-Time <see cref="BackupData"/> sample data.</returns>
        public BackupData GetBackupData()
        {
            var data = this.d.GenerateSampleData();
            data.BackupItem.Insert(0, new BackupItemType { ItemName = "This is Design Data" });
            return data;
        }
    }
}