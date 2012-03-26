using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Data.Xml.XmlData;
using Jedzia.BackBock.DataAccess;
using System.IO;

namespace Jedzia.BackBock.Data.Xml
{
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
                            "Jedzia.BackBock.Data.Xml.SampleData.BackupData01.xml"))
                {
                    TextReader txr = new StreamReader(stream);
                    str = txr.ReadToEnd();
                }
                classData = BackupData.Deserialize(str);
            }

            return classData;
        }
    }

    [Obsolete("Remove this provider after testing")]
    public class TestBackupDataRepository : BackupDataRepository, IDisposable
    {
        // ...or make a initial-setup / sample-data provider of it.
        private DesignDataProvider d = new DesignDataProvider();

        public override Jedzia.BackBock.DataAccess.DTO.BackupData GetBackupData()
        {
            var data = d.GenerateSampleData();
            var lst = data.BackupItem.ToList();
            lst.Insert(0, new BackupItemType() { ItemName = "This is from Xml Test Data" });
            data.BackupItem = lst.ToArray();
            return data.ToHostType();
        }

        #region IDisposable Members

        public void Dispose()
        {
            d = null;
        }

        #endregion

        public override BackupRepositoryType RepositoryType
        {
            get { return BackupRepositoryType.Static; }
        }
    }
}
