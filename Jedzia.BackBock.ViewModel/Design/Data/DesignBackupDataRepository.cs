// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleResourceProvider.cs" company="">
//   
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>
//   Defines the SampleResourceProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Design.Data
{
    using System.IO;
    using Jedzia.BackBock.Model.Data;
    using Jedzia.BackBock.ViewModel.Data;
    using Jedzia.BackBock.Model;
    using System;

    internal class DesignDataProvider
    {
        private BackupData classData;
        public BackupData GenerateSampleData()
        {
            if (classData == null)
            {
                // Das ist Model Stuff
                string str = string.Empty;
                using (
                    Stream stream =
                        typeof(DesignDataProvider).Assembly.GetManifestResourceStream(
                            "Jedzia.BackBock.ViewModel.Design.Data.BackupData01.xml"))
                {
                    TextReader txr = new StreamReader(stream);
                    str = txr.ReadToEnd();
                }
                classData = BackupData.Deserialize(str);
            }

            return classData;
        }
    }

    public class DesignBackupDataRepository : BackupDataRepository, IDisposable
    {
        private DesignDataProvider d = new DesignDataProvider();

        public override BackupData GetBackupData()
        {
            var data = d.GenerateSampleData();
            data.BackupItem.Insert(0, new BackupItemType() { ItemName = "This is Design Data" });
            return data;
        }

        #region IDisposable Members

        public void Dispose()
        {
            d = null;
        }

        #endregion
    }
}