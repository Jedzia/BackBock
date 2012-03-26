using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.IO;
using System.Xml;

namespace Jedzia.BackBock.Model
{
    [Obsolete("Data Access happens in Jedzia.BackBock.DataAccess, etc.")]
    public static class ModelLoader
    {
        public static BackupData LoadBackupData(string path)
        {
            //var classData = BackupData.LoadFromFile(path);
            return null;
        }
    }
    
    [Obsolete("Data Access happens in Jedzia.BackBock.DataAccess, etc.")]
    public static class ModelSaver
    {
        public static void SaveBackupData(BackupData data, string path)
        {
            try
            {
                /*var xml = data.Serialize();
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                doc.Normalize();
                doc.Save(path);*/

            }
            catch (Exception ex)
            {
                // Todo: remove this after testing. ! evil. no one knows when no save occurs.
            }   
            //data.SaveToFile(path);
        }
    }
}
