using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Jedzia.BackBock.ViewModel.Commands;
using Jedzia.BackBock.ViewModel;

namespace Jedzia.BackBock.Application
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window, IDialogControl
    {
        public event EventHandler Closed;
        private Action _callback;
        private Action<bool> _callbackWithBool;
        private bool? _result;

        /// <summary>
        /// The <see cref="Title" /> dependency property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// Gets or sets the value of the <see cref="Title" />
        /// property. This is a dependency property.
        /// </summary>
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Title" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            TitlePropertyName,
            typeof(string),
            typeof(CustomMessageBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// The <see cref="Message" /> dependency property's name.
        /// </summary>
        public const string MessagePropertyName = "Message";

        /// <summary>
        /// Gets or sets the value of the <see cref="Message" />
        /// property. This is a dependency property.
        /// </summary>
        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Message" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            MessagePropertyName,
            typeof(string),
            typeof(CustomMessageBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// The <see cref="ConfirmButtonText" /> dependency property's name.
        /// </summary>
        public const string ConfirmButtonTextPropertyName = "ConfirmButtonText";

        /// <summary>
        /// Gets or sets the value of the <see cref="ConfirmButtonText" />
        /// property. This is a dependency property.
        /// </summary>
        public string ConfirmButtonText
        {
            get
            {
                return (string)GetValue(ConfirmButtonTextProperty);
            }
            set
            {
                SetValue(ConfirmButtonTextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="ConfirmButtonText" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConfirmButtonTextProperty = DependencyProperty.Register(
            ConfirmButtonTextPropertyName,
            typeof(string),
            typeof(CustomMessageBox),
            new PropertyMetadata(null));

        /// <summary>
        /// The <see cref="CancelButtonText" /> dependency property's name.
        /// </summary>
        public const string CancelButtonTextPropertyName = "CancelButtonText";

        /// <summary>
        /// Gets or sets the value of the <see cref="CancelButtonText" />
        /// property. This is a dependency property.
        /// </summary>
        public string CancelButtonText
        {
            get
            {
                return (string)GetValue(CancelButtonTextProperty);
            }
            set
            {
                SetValue(CancelButtonTextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CancelButtonText" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelButtonTextProperty = DependencyProperty.Register(
            CancelButtonTextPropertyName,
            typeof(string),
            typeof(CustomMessageBox),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var sender = (CustomMessageBox)s;
                    sender.CancelButtonVisibility = string.IsNullOrEmpty((string)e.NewValue)
                                                        ? Visibility.Collapsed
                                                        : Visibility.Visible;
                }));

        /// <summary>
        /// The <see cref="IsShowingError" /> dependency property's name.
        /// </summary>
        public const string IsShowingErrorPropertyName = "IsShowingError";

        /// <summary>
        /// Gets or sets the value of the <see cref="IsShowingError" />
        /// property. This is a dependency property.
        /// </summary>
        public bool IsShowingError
        {
            get
            {
                return (bool)GetValue(IsShowingErrorProperty);
            }
            set
            {
                SetValue(IsShowingErrorProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsShowingError" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsShowingErrorProperty = DependencyProperty.Register(
            IsShowingErrorPropertyName,
            typeof(bool),
            typeof(CustomMessageBox),
            new PropertyMetadata(
                false,
                (s, e) =>
                {
                    var sender = (CustomMessageBox)s;
                    sender.MessageElementsVisibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
                }));

        /// <summary>
        /// The <see cref="CancelButtonVisibility" /> dependency property's name.
        /// </summary>
        public const string CancelButtonVisibilityPropertyName = "CancelButtonVisibility";

        /// <summary>
        /// Gets or sets the value of the <see cref="CancelButtonVisibility" />
        /// property. This is a dependency property.
        /// </summary>
        public Visibility CancelButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(CancelButtonVisibilityProperty);
            }
            set
            {
                SetValue(CancelButtonVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CancelButtonVisibility" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelButtonVisibilityProperty = DependencyProperty.Register(
            CancelButtonVisibilityPropertyName,
            typeof(Visibility),
            typeof(CustomMessageBox),
            new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// The <see cref="MessageElementsVisibility" /> dependency property's name.
        /// </summary>
        public const string MessageElementsVisibilityPropertyName = "MessageElementsVisibility";

        /// <summary>
        /// Gets or sets the value of the <see cref="MessageElementsVisibility" />
        /// property. This is a dependency property.
        /// </summary>
        public Visibility MessageElementsVisibility
        {
            get
            {
                return (Visibility)GetValue(MessageElementsVisibilityProperty);
            }
            set
            {
                SetValue(MessageElementsVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="MessageElementsVisibility" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageElementsVisibilityProperty = DependencyProperty.Register(
            MessageElementsVisibilityPropertyName,
            typeof(Visibility),
            typeof(CustomMessageBox),
            new PropertyMetadata(Visibility.Collapsed));

        public RelayCommand OkCommand
        {
            get;
            private set;
        }

        public RelayCommand CancelCommand
        {
            get;
            private set;
        }

        public Storyboard HideBoxAnimation
        {
            get
            {
                var anim = this.Resources["HideBoxAnimation"];
                return (Storyboard)anim;
            }
        }

        public Storyboard ShowBoxAnimation
        {
            get
            {
                var anim = this.Resources["ShowBoxAnimation"];
                return (Storyboard)anim;
            }
        }


        public CustomMessageBox()
        {
            // Required to initialize variables
            InitializeComponent();
            HideBoxAnimation.Completed += HideBoxAnimationCompleted;

            OkCommand = new RelayCommand((e) =>
            {
                _result = true;
                Hide();
            });

            CancelCommand = new RelayCommand((e) =>
            {
                _result = false;
                Hide();
            });

            DataContext = this;
        }

        private delegate void VoidDelegate();
        private void HideBoxAnimationCompleted(object sender, EventArgs e)
        {
            HideBoxAnimation.Stop();
            Visibility = Visibility.Collapsed;

            VoidDelegate method = HideBoxAnimationCompletedAsync;
            Dispatcher.BeginInvoke(method, new object[0]);
        }

        private void HideBoxAnimationCompletedAsync()
        {
            if (_callbackWithBool != null)
            {
                _callbackWithBool(_result != null && _result.Value);
            }
            if (_callback != null)
            {
                _callback();
            }
            if (Closed != null)
            {
                Closed(this, EventArgs.Empty);
            }
            this.Close();
        }

        private bool _isVisible;

        public void Show(Action callback)
        {
            if (_isVisible)
            {
                return;
            }

            _callbackWithBool = null;
            _callback = callback;
            Show();
        }

        public void Show(Action<bool> callbackWithBool)
        {
            if (_isVisible)
            {
                return;
            }

            _callback = null;
            _callbackWithBool = callbackWithBool;
            Show();
        }

        private void Show()
        {
            _result = null;
            _isVisible = true;
            Visibility = Visibility.Visible;
            HideBoxAnimation.Stop();
            ShowBoxAnimation.Begin();
            ShowDialog();
        }

        public void Hide()
        {
            if (!_isVisible)
            {
                return;
            }

            _isVisible = false;
            ShowBoxAnimation.Stop();
            HideBoxAnimation.Begin();
        }

        /*private void CancelTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            CancelCommand.Execute(null);
            e.Handled = true;
        }

        private void OKTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            OkCommand.Execute(null);
            e.Handled = true;
        }*/
    }
}
