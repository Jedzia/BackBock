// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignDataProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Data.Xml
{
    using System.IO;
    using Jedzia.BackBock.Data.Xml.XmlData;

    /// <summary>
    /// Design-Time Backup data provider.
    /// </summary>
    internal class DesignDataProvider
    {
        #region Fields

        /// <summary>
        /// Backing field for the design backup data.
        /// </summary>
        private BackupData classData;

        #endregion

        /// <summary>
        /// Generates the sample data.
        /// </summary>
        /// <returns>Design-Time sample data.</returns>
        public BackupData GenerateSampleData()
        {
            if (this.classData == null)
            {
                // Das ist Model Stuff
                string str;
                using (
                    Stream stream =
                        typeof(DesignDataProvider).Assembly.GetManifestResourceStream(
                            "Jedzia.BackBock.Data.Xml.SampleData.BackupData01.xml"))
                {
                    TextReader txr = new StreamReader(stream);
                    str = txr.ReadToEnd();
                }

                this.classData = BackupData.Deserialize(str);
            }

            return this.classData;
        }
    }
}