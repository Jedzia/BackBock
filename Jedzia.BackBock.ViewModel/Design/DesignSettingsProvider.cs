// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignSettingsProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Design
{
    /// <summary>
    /// Design-Time Settings Provider stub.
    /// </summary>
    public class DesignSettingsProvider : ISettingsProvider
    {
        /// <summary>
        /// Gets the path of the startup data file.
        /// </summary>
        /// <returns>
        /// The path of the startup data file.
        /// </returns>
        public string GetStartupDataFile()
        {
            return "This is a dummy string from: GetStartupDataFile() "
                   + " Jedzia.BackBock.ViewModel.Design.DesignSettingsProvider";
        }
    }
}