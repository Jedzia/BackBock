using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Jedzia.BackBock.Data.Xml.XmlData;

namespace Jedzia.BackBock.Data.Xml
{
    internal static class ModelLoader
    {
        public static BackupData LoadBackupData(string path)
        {
            var classData = BackupData.LoadFromFile(path);
            return classData;
        }
    }

    internal static class ModelSaver
    {
        public static void SaveBackupData(BackupData data, string path)
        {
            try
            {
                var xml = data.Serialize();
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                doc.Normalize();
                doc.Save(path);

            }
            catch (Exception ex)
            {
                // Todo: remove this after testing. ! evil. no one knows when no save occurs.
            }   
            data.SaveToFile(path);
        }
    }
}
