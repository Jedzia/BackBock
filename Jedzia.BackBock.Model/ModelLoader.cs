using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.Model.Data;
using System.IO;
using System.Xml;

namespace Jedzia.BackBock.Model
{
    public static class ModelLoader
    {
        public static BackupData LoadBackupData(string path)
        {
            var classData = BackupData.LoadFromFile(path);
            return classData;
        }
    }
    public static class ModelSaver
    {
        public static void SaveBackupData(BackupData data, string path)
        {
            var xml = data.Serialize();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Normalize();
            doc.Save(path);
            //data.SaveToFile(path);
        }
    }
}
