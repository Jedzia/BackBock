using System;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Interactivity;
using Jedzia.BackBock.ViewModel.MVVM.Validation;

namespace Jedzia.BackBock.Application.Behaviors
{
    public class ValidationHelperBehavior : Behavior<FrameworkElement>
    {
        public string DataBoundPropertyName { get; set; }

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ValidationHelperBehavior), new PropertyMetadata(""));

        
        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ValidationHelperBehavior), new PropertyMetadata(null,
                new PropertyChangedCallback(ViewModelChangedCallback)
                ));

        private string viewModelProperty = null;

        private static void ViewModelChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            //We need the ViewModel
            var vm = args.NewValue;
            IValidatingViewModelBase viewModel = null;
            if (vm is IValidatingViewModelBase) { viewModel = vm as IValidatingViewModelBase; }
            else
            {
                Debug.WriteLine("ValidationHelperBehavior supports only IValidatingViewModelBase derived types");
                return;
            }

            var behavior = (sender as ValidationHelperBehavior);
            var associatedObj = behavior.AssociatedObject;
            if(associatedObj == null ) return;

            //We need the Data Bound Property
            FieldInfo fi = associatedObj.GetType().GetField(behavior.DataBoundPropertyName + "Property");

            DependencyProperty dependencyProperty = null;

            try
            {
                dependencyProperty = (DependencyProperty)fi.GetValue(associatedObj);
            }
            catch (Exception)
            {
                
                Debug.WriteLine("DataBoundPropertyName must be a valid Dependecy Property's name");
                return;
            }

            //Get the Binding
            BindingExpression expression = associatedObj.GetBindingExpression(dependencyProperty);
            if (expression == null)
            {
                Debug.WriteLine("No Binding was defined on the specified property");
                return;
            }


            //Check if we can subscribe to BindingValidationError event
            if (expression.ParentBinding.NotifyOnValidationError == false ||
                expression.ParentBinding.ValidatesOnExceptions == false)
            {
                Debug.WriteLine(
                    "ValidationExceptions and NotifyOnValidationError properties must be set to true on the Binding");
                return;
            }

            //Now we know the property name
            behavior.viewModelProperty = expression.ParentBinding.Path.Path;

            //Subscribe to BindingValidationError event!
            /*associatedObj.BindingValidationError += (source, e) =>
            {
                //We are only interested in FormatExceptions!!!
                if (e.Error.Exception is FormatException == false) return;

                //Set the error in ViewModel
                if (e.Action == ValidationErrorEventAction.Added)
                {

                    viewModel.AddError(behavior.viewModelProperty, behavior.ErrorMessage);
                }
                else
                {
                    viewModel.RemoveError(behavior.viewModelProperty, behavior.ErrorMessage);
                }
            };*/

        }

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

    }
}
