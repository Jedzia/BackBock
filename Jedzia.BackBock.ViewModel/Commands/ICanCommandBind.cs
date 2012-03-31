// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanCommandBind.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.Commands
{
    using System.Windows.Input;

    /// <summary>
    /// Provides <see cref="CommandBinding"/> abilities. 
    /// </summary>
    public interface ICanCommandBind
    {
        #region Properties

        /// <summary>
        /// Gets a collection of <see cref="CommandBinding"/> objects associated with this element. 
        /// A <see cref="T:System.Windows.Input.CommandBinding"/> enables command handling for 
        /// this element, and declares the linkage between a command, its events, and the handlers 
        /// attached by this element.
        /// </summary>
        /// <returns>
        /// The collection of all <see cref="CommandBinding"/> objects.
        /// </returns>
        CommandBindingCollection CommandBindings { get; }

        #endregion
    }
}