using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using System.ComponentModel;
using Jedzia.BackBock.ViewModel.MVVM.Validation;

namespace Jedzia.BackBock.Application.Behaviors
{
    public class EditOnFocusBehavior : Behavior<FrameworkElement>
    {


        /// <summary>
        /// Gets or sets a value indicating whether to call EndEdit when the EditableObject lost its focus.
        /// </summary>
        /// <value>
        ///  When <c>true</c> call the EditableObject IEditableObject.EndEdit() method when the control 
        ///  looses its focus; otherwise, <c>false</c>.
        /// </value>
        public bool FiresOnLostFocus
        {
            get { return (bool)GetValue(FiresOnLostFocusProperty); }
            set { SetValue(FiresOnLostFocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FiresOnLostFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiresOnLostFocusProperty =
            DependencyProperty.Register("FiresOnLostFocus", typeof(bool), typeof(EditOnFocusBehavior), new UIPropertyMetadata(false));


        /// <summary>
        /// Gets or sets the editable object.
        /// </summary>
        /// <value>
        /// The editable object.
        /// </value>
        public object EditableObject
        {
            get { return (object)GetValue(EditableObjectProperty); }
            set { SetValue(EditableObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditableObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditableObjectProperty =
            DependencyProperty.Register("EditableObject", typeof(object), typeof(EditOnFocusBehavior), new PropertyMetadata(null));


        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.GotFocus -= new RoutedEventHandler(AssociatedObject_GotFocus);
            if (FiresOnLostFocus)
            {
                this.AssociatedObject.LostFocus -= new RoutedEventHandler(AssociatedObject_LostFocus);
                this.AssociatedObject.KeyDown -= new KeyEventHandler(AssociatedObject_KeyDown);
                if (EditableObject is ValidatingViewModelBase)
                {
                    var vm = (ValidatingViewModelBase)EditableObject;
                    vm.PropertyChanged -= vm_PropertyChanged;
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.GotFocus += new RoutedEventHandler(AssociatedObject_GotFocus);
            if (FiresOnLostFocus)
            {
                this.AssociatedObject.LostFocus += new RoutedEventHandler(AssociatedObject_LostFocus);
                this.AssociatedObject.KeyDown += new KeyEventHandler(AssociatedObject_KeyDown);

                if (EditableObject != null)
                {
                    if (EditableObject is ValidatingViewModelBase)
                    {
                        // Todo: Extra switch, maybe FireViewModelPropertyChanged
                        //var vm = (ValidatingViewModelBase)EditableObject;
                        //vm.PropertyChanged += new PropertyChangedEventHandler(vm_PropertyChanged);
                    }
                    else if (this.AssociatedObject is IInputElement)
                    {
                        var inpe = (IInputElement)this.AssociatedObject;
                        //inpe.GotKeyboardFocus
                        //inpe.be
                    }
                }
            }
        }

        void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = sender as ValidatingViewModelBase;
            if (vm == null)
            {
                return;
            }
            vm.Validate();
        }

        void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
            {
                return;
            }

            if (e.OriginalSource is Button) return;

            if (EditableObject != null && EditableObject is IEditableObject)
            {
                (EditableObject as IEditableObject).CancelEdit();
            }
        }

        void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button) return;

            if (EditableObject != null && EditableObject is IEditableObject)
            {
                (EditableObject as IEditableObject).EndEdit();
            }
        }


        void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button) return;

            if (EditableObject != null && EditableObject is IEditableObject)
            {
                (EditableObject as IEditableObject).BeginEdit();
            }
        }
    }
}
