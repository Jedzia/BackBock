// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDataService.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Security.Principal;
    using Jedzia.BackBock.Model.Data;

    /// <summary>
    /// Backup data provider.
    /// </summary>
    public class BackupDataService : IBackupDataService
    {
        #region Fields

        /// <summary>
        /// List of available <see cref="BackupData"/> repositories.
        /// </summary>
        private readonly IEnumerable<BackupDataRepository> repositories;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupDataService"/> class.
        /// </summary>
        /// <param name="repositories">The repositories to use.</param>
        /// <exception cref="ArgumentNullException">Argument is <c>null</c>.</exception>
        public BackupDataService(IEnumerable<BackupDataRepository> repositories)
        {
            if (repositories == null || repositories.Count() < 1)
            {
                throw new ArgumentNullException("repository");
            }

            this.repositories = repositories;

            // this.repositoryType = repository.RepositoryType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the available endpoints that provide <see cref="BackupData"/>.
        /// </summary>
        public IEnumerable<string> LoadedServices
        {
            get
            {
                return this.repositories.Select(item => item.RepositoryType.ToString() + ": " + item.GetType().FullName);

                /*foreach (var item in repositories)
                {
                    yield return item.RepositoryType.ToString() + item.GetType().FullName;
                }*/
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
        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <c>null</c>.</exception>
        public BackupData GetBackupData(BackupRepositoryType repotype, IPrincipal user, StringDictionary parameters)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var repo = this.repositories.First(x => x.RepositoryType == repotype);
            var dto = repo.GetBackupData();
            var mapper = new BackupDataAssembler();
            var bussinessData = mapper.ConvertFromDTO(dto);
            return bussinessData;

            /*if (this.repository is BackupDataFsRepository)
            {
                var repo = (BackupDataFsRepository)this.repository;
                return repo.LoadBackupData(parameters["filename"], parameters);
            }
            else if (this.repository is BackupDataRepository)
            {
                return this.repository.GetBackupData();
            }*/
            
            // throw new NotSupportedException("Unknown Repository Type");

            // return this.repository.GetFeaturedProducts();
            // return from p in
            // this.repository.GetFeaturedProducts()
            // select p.ApplyDiscountFor(user);
        }

        /*private readonly BackupRepositoryType repositoryType = BackupRepositoryType.Unknown;
        public BackupRepositoryType ServiceType
        {
            get { return repositoryType; }
        }*/

        /// <summary>
        /// Loads the specified <see cref="BackupData"/> by filename.
        /// </summary>
        /// <param name="filename">The path to the stored <see cref="BackupData"/> on disk.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be <c>null</c>.</param>
        /// <returns>
        /// A set of Backup data.
        /// </returns>
        public BackupData Load(string filename, IPrincipal user, StringDictionary parameters)
        {
            // use the 
            var repo = this.repositories.First(x => x.RepositoryType == BackupRepositoryType.FileSystemProvider);
            var fileRepo = (BackupDataFsRepository)repo;

            // var repo = repositories.OfType<BackupDataFsRepository>().First();
            return fileRepo.LoadBackupData(filename, parameters).FromDTO();
           
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the specified <see cref="BackupData"/> to disk.
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="filename">The path to the stored <see cref="BackupData"/> on disk.</param>
        /// <param name="user">The requesting user with permissions.</param>
        /// <param name="parameters">Optional specified parameters. Can be <c>null</c>.</param>
        public void Save(BackupData data, string filename, IPrincipal user, StringDictionary parameters)
        {
            // use the 
            var repo = this.repositories.First(x => x.RepositoryType == BackupRepositoryType.FileSystemProvider);
            var fileRepo = (BackupDataFsRepository)repo;

            // var repo = repositories.OfType<BackupDataFsRepository>().First();
            fileRepo.SaveBackupData(data.ToDTO(), filename, parameters);

            // throw new NotImplementedException();
        }

    }
}