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

    public class DesignDataProvider
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

    public class DesignBackupDataRepository : BackupDataRepository
    {

        public override BackupData GetBackupData()
        {
            var d = new DesignDataProvider();
            var data = d.GenerateSampleData();
            data.BackupItem.Insert(0, new BackupItemType() { ItemName = "This is Design Data" });
            return data;
        }
    }
}