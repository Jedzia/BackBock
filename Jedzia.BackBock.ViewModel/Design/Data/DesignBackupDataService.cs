// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignBackupDataService.cs" company="EvePanix">
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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using Jedzia.BackBock.DataAccess;
    using Jedzia.BackBock.Model.Data;

    /// <summary>
    /// Skips the usually steps of <see cref="BackupDataRepository"/> injection for Design-Time.
    /// </summary>
    public class DesignBackupDataService : IBackupDataService
    {
        #region Properties

        /// <summary>
        /// Gets the available endpoints that provide <see cref="BackupData"/>.
        /// </summary>
        public IEnumerable<string> LoadedServices
        {
            get
            {
                return new[] { "Static: " + typeof(DesignBackupDataRepository).FullName };
            }
        }

        #endregion

        /// <summary>
        /// Gets the backup data for a given repository type.
        /// </summary>
        /// <param name="repotype">The repository type.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be <c>null</c>.</param>
        /// <returns>
        /// A set of Backup data.
        /// </returns>
        public BackupData GetBackupData(BackupRepositoryType repotype, IPrincipal user, StringDictionary parameters)
        {
            var dbr = new DesignBackupDataRepository();
            return dbr.GetBackupData();
        }

        /// <summary>
        /// Not implemented in Design Mode.
        /// </summary>
        /// <param name="connection">The used connection.</param>
        /// <param name="user">The user permissions.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Nothing, not implemented.
        /// </returns>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public BackupData Load(string connection, IPrincipal user, StringDictionary parameters)
        {
            throw new NotImplementedException();
        }
    }
}