﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatingViewModelBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Base class for a validating <c>ViewModel</c> that implements <see cref="IDataErrorInfo"/> data validation.
    /// </summary>
    public abstract class ValidatingViewModelBase : EditableViewModelBase, IValidatingViewModelBase, IDataErrorInfo
    {
        #region Fields

        /// <summary>
        /// Internal list of errors.
        /// </summary>
        protected Dictionary<string, List<string>> errorList = new Dictionary<string, List<string>>();

        /// <summary>
        /// Internal list of business errors.
        /// </summary>
        private readonly List<string> businessErrors = new List<string>();

        private string objectValidationErrorMessage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error
        {
            get
            {
                if (IsInDesignMode)
                {
                    return "Sample Error String";
                }

                // Declare variables
                string error = null;

                // If not validated, validate
                // if (!IsValidated) Validate();

                // Check if any errors occurred
                if ((this.businessErrors != null) && (this.businessErrors.Count > 0))
                {
                    // Yes, retrieve the first one
                    error = this.businessErrors[0];
                }

                // Return result
                return error;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get
            {
                var hasBusError = this.businessErrors.Count > 0;
                var hasFieldError = this.errorList.Values.Sum(c => c.Count) > 0;
                return hasBusError | hasFieldError;
            }
        }

        /// <summary>
        /// Gets or sets the object validation error message.
        /// </summary>
        /// <value>
        /// The object validation error message.
        /// </value>
        public string ObjectValidationErrorMessage
        {
            get
            {
                // Todo: implement this ... seems to be Error...
                return this.objectValidationErrorMessage;
            }

            set
            {
                this.objectValidationErrorMessage = value;
                RaisePropertyChanged("ObjectValidationErrorMessage");
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">Name of the requested property.</param>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        public string this[string propertyName]
        {
            get
            {
                this.CheckErrorCollectionForProperty(propertyName);

                // return errorList[propertyName];
                if (this.errorList[propertyName].Count > 0)
                {
                    return this.errorList[propertyName][0];
                }

                return null;
            }
        }

        #endregion

        /// <summary>
        /// Adds an error to the collection.
        /// </summary>
        /// <param name="propertyName">Name of the property to be added.</param>
        /// <param name="validationError">The validation error message.</param>
        public virtual void AddError(string propertyName, string validationError)
        {
            this.CheckErrorCollectionForProperty(propertyName);

            if (this.errorList[propertyName].Contains(validationError))
            {
                return;
            }

            this.errorList[propertyName].Add(validationError);

            this.RaiseErrorsChanged(propertyName);
            RaisePropertyChanged("HasErrors");
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public override void CancelEdit()
        {
            base.CancelEdit();
            this.OnValidateViewModel();
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public override void EndEdit()
        {
            this.OnValidateViewModel();

            if (!this.HasErrors)
            {
                // Make sure we are in a valid state!
                base.EndEdit();
            }
        }

        /// <summary>
        /// Remove the specified error from the list.
        /// </summary>
        /// <param name="propertyName">Name of the property with an error.</param>
        /// <param name="validationError">The detected validation error.</param>
        public virtual void RemoveError(string propertyName, string validationError)
        {
            this.CheckErrorCollectionForProperty(propertyName);

            if (this.errorList[propertyName].Contains(validationError))
            {
                this.errorList[propertyName].Remove(validationError);
            }

            this.RaiseErrorsChanged(propertyName);
            RaisePropertyChanged("HasErrors");
        }

        /// <summary>
        /// Removes all errors of a specified property from the list.
        /// </summary>
        /// <param name="propertyName">Name of the property with an error.</param>
        public virtual void RemoveErrors(string propertyName)
        {
            this.CheckErrorCollectionForProperty(propertyName);

            this.errorList[propertyName].Clear();

            this.RaiseErrorsChanged(propertyName);
            RaisePropertyChanged("HasErrors");
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            this.OnValidateViewModel();
        }

        /// <summary>
        /// Validates the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> on a valid property.</returns>
        public virtual bool ValidateProperty(string propertyName /*, object propertyValue*/)
        {
            // Setup context and output variable
            // var validationResults = new List<ValidationResult>();

            // ValidationContext validationContext = new ValidationContext(this, null, null);
            // validationContext.MemberName = propertyName;

            // Perform validation
            bool isValid = true;

            // Validator.TryValidateProperty(propertyValue, validationContext, validationResults);
            this.RemoveErrors(propertyName);

            var type = GetType();
            var property = type.GetProperty(propertyName);
            {
                object propertyValue = property.GetValue(this, null);
                foreach(ValidationAttribute attribute in
                    property.GetCustomAttributes(typeof(ValidationAttribute), true))
                {
                    // if (!attribute.IsValid(property.GetValue(this, null)))
                    try
                    {
                        // if (!attribute.IsValid(propertyValue))
                        if (!attribute.IsValid(propertyValue))
                        {
                            // ValidationResult.ValidResult
                            // BrokenRules.Add(attribute.ErrorMessage);
                            var msg = attribute.FormatErrorMessage(propertyName);
                            this.AddError(propertyName, msg);
                            isValid = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        isValid = false;
                        this.AddError(propertyName, ex.Message);

                        // throw;
                    }
                }
            }

            /*if (!isValid) //Register validation errors
            {
                //Perform a full clear
                RemoveErrors(propertyName);

                //Add active validation errors
                foreach (var validationResult in validationResults)
                {
                    //AddError(propertyName, validationResult.ErrorMessage);
                }
            }
            else // Remove all error messages
            {
                RemoveErrors(propertyName);
            }*/
            return isValid;
        }

        /// <summary>
        /// Checks the error collection for a property and creates one, if not present.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void CheckErrorCollectionForProperty(string propertyName)
        {
            if (!this.errorList.ContainsKey(propertyName))
            {
                this.errorList[propertyName] = new List<string>();
            }
        }

        /// <summary>
        /// Called when the <c>ViewModel</c> is validated.
        /// </summary>
        protected void OnValidateViewModel()
        {
            this.businessErrors.Clear();
            this.ValidateViewModel();
            {
                // if (HasErrors)
                RaisePropertyChanged("Error");
            }
        }

        /// <summary>
        /// Raises the errors changed event. Todo: implement.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaiseErrorsChanged(string propertyName)
        {
            /*if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }*/
        }

        /// <summary>
        /// Sets a specific business rule error.
        /// </summary>
        /// <param name="error">Error message</param>
        protected void SetBusinessRuleError(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                return;
            }

            // Make sure to list the error only once
            if (this.businessErrors.Contains(error))
            {
                return;
            }

            // Store the error
            this.businessErrors.Add(error);
        }

        /// <summary>
        /// Validates the view model.
        /// </summary>
        protected abstract void ValidateViewModel();
    }
}