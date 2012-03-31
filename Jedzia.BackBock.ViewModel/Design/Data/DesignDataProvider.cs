// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignDataProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Design.Data
{
    using Jedzia.BackBock.Model.Data;

    /// <summary>
    /// Design-Time <see cref="BackupData"/> Provider stub.
    /// </summary>
    internal class DesignDataProvider
    {
        #region Fields

        private BackupData classData;

        #endregion

        /// <summary>
        /// Generates the sample data.
        /// </summary>
        /// <returns>A set of Design-Time <see cref="BackupData"/> sample data.</returns>
        public BackupData GenerateSampleData()
        {
            if (this.classData == null)
            {
                /*// Das ist Model Stuff
                string str = string.Empty;
                using (
                    Stream stream =
                        typeof(DesignDataProvider).Assembly.GetManifestResourceStream(
                            "Jedzia.BackBock.ViewModel.Design.Data.BackupData01.xml"))
                {
                    TextReader txr = new StreamReader(stream);
                    str = txr.ReadToEnd();
                }
                classData = BackupData.Deserialize(str);*/
                this.classData = new BackupData { DatasetGroup = "Main", DatasetName = "Daily" };

                this.classData.BackupItem.Add(
                    new BackupItemType
                        {
                            ItemGroup = "Standard", 
                            IsEnabled = true, 
                            ItemName = "All from Temp, exclude *.msi;B* "
                        });
            }

            return this.classData;
        }
    }
}