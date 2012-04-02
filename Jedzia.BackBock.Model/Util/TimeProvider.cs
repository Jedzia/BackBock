// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeProvider.cs" company="EvePanix">
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
    /// Time provider ambient context.
    /// </summary>
    public abstract class TimeProvider
    {
        #region Fields

        private static TimeProvider current;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="TimeProvider"/> class.
        /// </summary>
        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current <see cref="TimeProvider"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="TimeProvider"/>.
        /// </value>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public static TimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                current = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.DateTime"/> object that is set to the current date 
        /// and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> whose value is the current UTC date and time.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract DateTime UtcNow { get; }

        #endregion

        /// <summary>
        /// Resets to the default <see cref="DefaultTimeProvider"/> for testing purposes.
        /// </summary>
        internal static void ResetToDefault()
        {
            current = new DefaultTimeProvider();
        }
    }
}