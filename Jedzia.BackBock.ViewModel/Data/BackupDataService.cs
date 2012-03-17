namespace Jedzia.BackBock.ViewModel.Data
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using Jedzia.BackBock.Model.Data;

    public class BackupDataService : IBackupDataService
    {
        private readonly BackupDataRepository repository;

        public BackupDataService(BackupDataRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public BackupData GetBackupData(IPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return this.repository.GetBackupData();
            //return this.repository.GetFeaturedProducts();
            //return from p in
            //           this.repository.GetFeaturedProducts()
            //       select p.ApplyDiscountFor(user);
        }

    }
}