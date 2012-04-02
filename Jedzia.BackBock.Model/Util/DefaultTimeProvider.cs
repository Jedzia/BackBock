// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultTimeProvider.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Util
{
    using System;

    /// <summary>
    /// Time provider local default implementation.
    /// </summary>
    public class DefaultTimeProvider : TimeProvider
    {
        #region Properties

        /// <summary>
        /// Gets a <see cref="T:System.DateTime"/> object that is set to the current date
        /// and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> whose value is the current UTC date and time.
        /// </returns>
        public override DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        #endregion
    }
}