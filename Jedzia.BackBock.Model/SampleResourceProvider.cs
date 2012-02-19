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

namespace Jedzia.BackBock.Model
{
    using System.IO;
    using Jedzia.BackBock.Model.Data;

    public static class SampleResourceProvider
    {
        private static BackupData classData;
        public static BackupData GenerateSampleData()
        {
            if (classData == null)
            {
                // Das ist Model Stuff
                string str = string.Empty;
                using (
                    Stream stream =
                        typeof(SampleResourceProvider).Assembly.GetManifestResourceStream(
                            "Jedzia.BackBock.Model.Data.BackupData01.xml"))
                {
                    TextReader txr = new StreamReader(stream);
                    str = txr.ReadToEnd();
                }
                classData = BackupData.Deserialize(str);
            }

            return classData;
        }
    }
}