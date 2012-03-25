using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Jedzia.BackBock.ViewModel
{
    	public class PropertyData
	{
		#region Variables
		private Type _type = null;
		#endregion

		#region Constructor & destructor
		/// <summary>
		/// Initializes a new instance of this object.
		/// </summary>
		/// <param name="name">Name of the property.</param>
		/// <param name="type">Type of the property.</param>
		/// <param name="defaultValue">Default value of the property.</param>
		internal PropertyData(string name, Type type, object defaultValue)
		{
			// Store values
			Name = name;
			Type = type;
			DefaultValue = defaultValue;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the type of the property.
		/// </summary>
		public Type Type
		{
			get { return (_type != null) ? _type : typeof(object); }
			private set { _type = value; }
		}

		/// <summary>
		/// Gets or sets the default value of the property.
		/// </summary>
		private object DefaultValue { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Returns the default value of the property.
		/// </summary>
		/// <returns>Default value of the property.</returns>
		public object GetDefaultValue()
		{
			return DefaultValue;
		}

		/// <summary>
		/// Returns the typed default value of the property.
		/// </summary>
		/// <returns>Default value of the property.</returns>
		public T GetDefaultValue<T>()
		{
			return ((DefaultValue != null) && (DefaultValue is T)) ? (T)DefaultValue : default(T);
		}
		#endregion
	}

    public class ValidatingViewModelBaseOld : ViewModelBase, IDataErrorInfo
    {
        private Dictionary<string, PropertyData> _propertyInfo = new Dictionary<string, PropertyData>();
        private Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        private Dictionary<string, string> _fieldErrors = new Dictionary<string, string>();
        private List<string> _businessErrors = new List<string>();

        /// <summary>
        /// Gets or sets whether this object is validated or not.
        /// </summary>
        [Browsable(false)]
        private bool IsValidated { get; set; }

        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        protected virtual void ValidateFields()
        { }

        /// <summary>
        /// Validates the business rules of this object. Override this method to enable
        /// validation of business rules.
        /// </summary>
        protected virtual void ValidateBusinessRules()
        { }

        /// <summary>
        /// Sets a specific field error.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="error">Error message.</param>
        protected void SetFieldError(string property, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                return;
            }
            // Store the error
            _fieldErrors[property] = error;
            //var eee = new System.ComponentModel.DataAnnotations.RegularExpressionAttribute("sadfsdf");
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
            if (_businessErrors.Contains(error)) return;

            // Store the error
            _businessErrors.Add(error);
        }

        /// <summary>
        /// Validates the current object for field and business rule errors.
        /// </summary>
        /// <remarks>
        /// To check wether this object contains any errors, use the <see cref="HasErrors"/> property.
        /// </remarks>
        public void Validate()
        {
  
            // Check if the object is already validated
            if (IsValidated) return;

            // Clear all errors
            _fieldErrors = new Dictionary<string, string>();
            _businessErrors = new List<string>();

            // Validate the field errors
            ValidateFields();

            // Validate business rules
            ValidateBusinessRules();

            // Object is validated
            IsValidated = true;
            //RaisePropertyChanged("Error");
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets the current error
        /// </summary>
        string IDataErrorInfo.Error
        //public string Error
        {
            get
            {
                // Declare variables
                string error = null;

                // If not validated, validate
                if (!IsValidated) Validate();

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

        protected override void OnPropertyChanged(string propertyName)
        {
            //Validate("OnPropertyChanged " + propertyName);
            base.OnPropertyChanged(propertyName);
            
            // Not validated
            IsValidated = false;
        }


        /// <summary>
        /// Gets an error for a specific column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Error</returns>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                // Declare variables
                string error = string.Empty;

                // If not validated, validate
                if (!IsValidated) Validate();

                // Check if the error is available (and thus occurred)
                if ((_fieldErrors != null) && _fieldErrors.ContainsKey(columnName))
                {
                    // Yes, retrieve the error
                    error = _fieldErrors[columnName];
                }

                // Return result
                return error;
            }
        }
        #endregion
    }
}
