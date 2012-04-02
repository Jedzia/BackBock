// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskContext.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model
{
    using System;
    using System.Collections.Generic;
    using Jedzia.BackBock.Tasks;

    /// <summary>
    /// The ambient context that holds the system wide <see cref="ITaskService"/>.
    /// </summary>
    public abstract class TaskContext
    {
        #region Fields

        /// <summary>
        /// Holds the one and only instance of the <see cref="TaskContext"/>.
        /// </summary>
        private static TaskContext defaultContext;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default <see cref="TaskContext"/>.
        /// </summary>
        /// <value>
        /// The default <see cref="TaskContext"/>.
        /// </value>
        /// <remarks>
        /// <para>The default <see cref="TaskContext"/> can only set once, and that before accessing it.</para>
        /// <para>To achieve a different behavior for the functionality provided by another implementation
        /// of an <see cref="ITaskService"/>, derive from this class and override the abstract 
        /// <see cref="TaskContext.TaskService"/> getter.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Cannot set the default <see cref="ITaskService"/> twice. Maybe you accessed it before you've written to it.</exception>
        public static TaskContext Default
        {
            get
            {
                return defaultContext ?? (defaultContext = new DefaultTaskContext());
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (defaultContext != null)
                {
                    throw new InvalidOperationException(
                        "Cannot set the default ITaskService twice. Maybe you accessed it before you've written to it.");
                }

                defaultContext = value;
            }
        }

        /// <summary>
        /// Gets the task service of this instance.
        /// </summary>
        public abstract ITaskService TaskService { get; }

        #endregion

        /// <summary>
        /// Resets to the default context.
        /// </summary>
        internal static void ResetToDefault()
        {
            defaultContext = new DefaultTaskContext();
        }

        /// <summary>
        /// Resets to a deterministic starting position for unit tests.
        /// </summary>
        internal static void ResetToTestCondition()
        {
            defaultContext = null;
        }
    }
}