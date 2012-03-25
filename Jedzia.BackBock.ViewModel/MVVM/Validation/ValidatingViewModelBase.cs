using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
    public abstract class ValidatingViewModelBase : EditableViewModelBase, IValidatingViewModelBase, IDataErrorInfo
    {
        protected Dictionary<string, List<string>> errorList =
                new Dictionary<string, List<string>>();
        private List<string> _businessErrors = new List<string>();

        #region Single Property validation using attriubtes

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
            if (_businessErrors.Contains(error)) return;

            // Store the error
            _businessErrors.Add(error);
        }

        public virtual bool ValidateProperty(string propertyName, object propertyValue)
        {
            //Setup context and output variable
            List<ValidationResult> validationResults = new List<ValidationResult>();
            //ValidationContext validationContext = new ValidationContext(this, null, null);
            //validationContext.MemberName = propertyName;

            //Perform validation
            bool isValid = true;
            //Validator.TryValidateProperty(propertyValue, validationContext, validationResults);
            RemoveErrors(propertyName);

            var type = this.GetType();
            var property = type.GetProperty(propertyName);
            {
                foreach (ValidationAttribute attribute in
                    property.GetCustomAttributes(typeof(ValidationAttribute), true))
                {
                    //if (!attribute.IsValid(property.GetValue(this, null)))
                    try
                    {
                        if (!attribute.IsValid(propertyValue))
                        {
                            //ValidationResult.ValidResult
                            //BrokenRules.Add(attribute.ErrorMessage);
                            var msg = attribute.FormatErrorMessage(propertyName);
                            AddError(propertyName, msg);
                            isValid = false;
                        }

                    }
                    catch (Exception ex)
                    {

                        isValid = false;
                        AddError(propertyName, ex.Message);
                        //throw;
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

        #endregion

        #region IValidatingViewModelBase Members

        public virtual void AddError(string propertyName, string valdationError)
        {
            CheckErrorCollectionForProperty(propertyName);

            if (errorList[propertyName].Contains(valdationError)) return;
            errorList[propertyName].Add(valdationError);

            RaiseErrorsChanged(propertyName);
            RaisePropertyChanged("HasErrors");
        }

        public virtual void RemoveError(string propertyName, string validationError)
        {
            CheckErrorCollectionForProperty(propertyName);

            if (errorList[propertyName].Contains(validationError))
            {
                errorList[propertyName].Remove(validationError);
            }

            RaiseErrorsChanged(propertyName);
            RaisePropertyChanged("HasErrors");
        }

        public virtual void RemoveErrors(string propertyName)
        {
            CheckErrorCollectionForProperty(propertyName);

            errorList[propertyName].Clear();

            RaiseErrorsChanged(propertyName);
            RaisePropertyChanged("HasErrors");
        }

        #endregion

        #region INotifyDataErrorInfo Members

        /*public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            CheckErrorCollectionForProperty(propertyName);

            return errorList[propertyName];
        }*/

        public bool HasErrors
        {
            get {
                var hasBusError = _businessErrors.Count > 0;
                var hasFieldError = errorList.Values.Sum(c => c.Count) > 0;
                return hasBusError | hasFieldError; 
            }
        }

        #endregion
        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                // Declare variables
                string error = null;

                // If not validated, validate
                //if (!IsValidated) Validate();

                // Check if any errors occurred
                if ((_businessErrors != null) && (_businessErrors.Count > 0))
                {
                    // Yes, retrieve the first one
                    error = _businessErrors[0];
                }

                // Return result
                return error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                CheckErrorCollectionForProperty(propertyName);

                //return errorList[propertyName];
                if (errorList[propertyName].Count > 0)
                {
                    return errorList[propertyName][0];
                }

                return null;
            }
        }

        #endregion

        #region Helper Functions

        protected void CheckErrorCollectionForProperty(string propertyName)
        {
            if (!errorList.ContainsKey(propertyName))
            {
                errorList[propertyName] = new List<string>();
            }
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            /*if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }*/
        }

        #endregion

        #region Base Class Overrides and Properties

        public override void CancelEdit()
        {
            base.CancelEdit();
            OnValidateViewModel();
        }

        private void OnValidateViewModel()
        {
            _businessErrors.Clear();
            ValidateViewModel();
            //if (HasErrors)
            {
                RaisePropertyChanged("Error");
            }
        }

        public override void EndEdit()
        {
            OnValidateViewModel();

            if (!HasErrors) //Make sure we are in a valid state!
            {
                base.EndEdit();
            }
        }

        string objectValidationErrorMessage;

        public string ObjectValidationErrorMessage
        {
            get { return objectValidationErrorMessage; }
            set
            {
                objectValidationErrorMessage = value;
                RaisePropertyChanged("ObjectValidationErrorMessage");
            }
        }

        protected abstract void ValidateViewModel();

        #endregion


        public void Validate()
        {
            OnValidateViewModel();
        }
    }
}
