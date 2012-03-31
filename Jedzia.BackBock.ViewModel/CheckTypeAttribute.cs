// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckTypeAttribute.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System;

    /// <summary>
    /// Declares which type a referenced item has to be of.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CheckTypeAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">Declares which type the referenced item has to be of.</param>
        public CheckTypeAttribute(Type type)
        {
            this.Type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the reference type.
        /// </summary>
        /// <value>
        /// The reference type of the item.
        /// </value>
        public Type Type { get; set; }

        #endregion
    }
}