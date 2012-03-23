namespace Jedzia.BackBock.ViewModel.Data
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Security.Principal;
    using Jedzia.BackBock.Model.Data;
    using System.Collections.Specialized;

    public class BackupDataService : IBackupDataService
    {
        private readonly IEnumerable<BackupDataRepository> repositories;

        public BackupDataService(IEnumerable<BackupDataRepository> repositories)
        {
            if (repositories == null || repositories.Count() < 1)
            {
                throw new ArgumentNullException("repository");
            }

            this.repositories = repositories;
            //this.repositoryType = repository.RepositoryType;
        }

        public BackupData GetBackupData(BackupRepositoryType repotype, IPrincipal user, StringDictionary parameters)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var repo = repositories.First(x => x.RepositoryType == repotype);
            return repo.GetBackupData();

            /*if (this.repository is BackupDataFsRepository)
            {
                var repo = (BackupDataFsRepository)this.repository;
                return repo.LoadBackupData(parameters["filename"], parameters);
            }
            else if (this.repository is BackupDataRepository)
            {
                return this.repository.GetBackupData();
            }*/
            throw new NotSupportedException("Unknown Repository Type");
            //return this.repository.GetFeaturedProducts();
            //return from p in
            //           this.repository.GetFeaturedProducts()
            //       select p.ApplyDiscountFor(user);
        }


        /*private readonly BackupRepositoryType repositoryType = BackupRepositoryType.Unknown;
        public BackupRepositoryType ServiceType
        {
            get { return repositoryType; }
        }*/

        #region IBackupDataService Members


        public BackupData Load(string filename, IPrincipal user, StringDictionary parameters)
        {
            // use the 
            // var repo = repositories.First(x => x.RepositoryType == BackupRepositoryType.FileSystemProvider);
            
            //var repo = repositories.OfType<BackupDataFsRepository>().First();
            //return repo.LoadBackupData(...);
            throw new NotImplementedException();
        }

        #endregion
    }
}