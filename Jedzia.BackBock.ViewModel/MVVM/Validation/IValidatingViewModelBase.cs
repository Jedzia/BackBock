// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidatingViewModelBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
    /// <summary>
    /// Provides validation of <c>ViewModel</c> data.
    /// </summary>
    public interface IValidatingViewModelBase
    {
        /// <summary>
        /// Add an error to the error list.
        /// </summary>
        /// <param name="propertyName">Name of the property with an error.</param>
        /// <param name="validationError">The detected validation error.</param>
        void AddError(string propertyName, string validationError);

        /// <summary>
        /// Remove the specified error from the list.
        /// </summary>
        /// <param name="propertyName">Name of the property with an error.</param>
        /// <param name="validationError">The detected validation error.</param>
        void RemoveError(string propertyName, string validationError);

        /// <summary>
        /// Removes all errors of a specified property from the list.
        /// </summary>
        /// <param name="propertyName">Name of the property with an error.</param>
        void RemoveErrors(string propertyName);
    }
}