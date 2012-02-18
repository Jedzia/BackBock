namespace Jedzia.BackBock.ViewModel.Commands
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// The ClickBehavior class exposes usefull attached properties that can be used to attach a commmand (ICommand)
    /// to a mouse event (such as MouseDoubleClick) to ANY UIElement.
    /// </summary>
    public class ClickCommands
    {
        #region MouseDoubleClickCommand
        /// <summary>
        /// MouseDoubleClickCommand Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty MouseDoubleClickCommandProperty =
            DependencyProperty.RegisterAttached(
                "MouseDoubleClickCommand",
                typeof(ICommand),
                typeof(ClickCommands),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMouseDoubleClickCommandChanged)));

        /// <summary>
        /// Gets the MouseDoubleClickCommand property.
        /// </summary>
        public static ICommand GetMouseDoubleClickCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(MouseDoubleClickCommandProperty);
        }

        /// <summary>
        /// Sets the MouseDoubleClickCommand property.
        /// </summary>
        public static void SetMouseDoubleClickCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(MouseDoubleClickCommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the MouseDoubleClickCommand property.
        /// </summary>
        private static void OnMouseDoubleClickCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as UIElement;
            if (element == null)
            {
                return;
            }
            if ((e.OldValue == null) && (e.NewValue != null))
            {
                element.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(UiElementMouseLeftButtonDown), true);
            }
            else if ((e.OldValue != null) && (e.NewValue == null))
            {
                element.RemoveHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(UiElementMouseLeftButtonDown));
            }
        }

        private static void UiElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // gets the sender is UIElement and check ClickCount because we want double click only
            var target = sender as UIElement;
            if (target != null && e.ClickCount > 1)
            {
                var iCommand = (ICommand)target.GetValue(MouseDoubleClickCommandProperty);
                if (iCommand != null)
                {
                    var routedCommand = iCommand as RoutedCommand;
                    // check if the command has a parameter using the MouseEventParameterProperty
                    object parameter = target.GetValue(MouseEventParameterProperty) ?? target;
                    // execute the command
                    if (routedCommand != null)
                    {
                        routedCommand.Execute(parameter, target);
                    }
                    else
                    {
                        iCommand.Execute(parameter);
                    }
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region MouseEventParameter
        /// <summary>
        /// MouseEventParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty MouseEventParameterProperty =
            DependencyProperty.RegisterAttached(
                "MouseEventParameter",
                typeof(object),
                typeof(ClickCommands),
                new FrameworkPropertyMetadata((object)null, null));

        /// <summary>
        /// Gets the MouseEventParameter property.
        /// </summary>
        public static object GetMouseEventParameter(DependencyObject d)
        {
            return d.GetValue(MouseEventParameterProperty);
        }

        /// <summary>
        /// Sets the MouseEventParameter property.
        /// </summary>
        public static void SetMouseEventParameter(DependencyObject d, object value)
        {
            d.SetValue(MouseEventParameterProperty, value);
        }
        #endregion

        #region ContextMenuOpeningCommand
        /// <summary>
        /// ContextMenuOpeningCommand Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ContextMenuOpeningCommandProperty =
            DependencyProperty.RegisterAttached(
                "ContextMenuOpeningCommand",
                typeof(ICommand),
                typeof(ClickCommands),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContextMenuOpeningCommandChanged)));

        /// <summary>
        /// Gets the ContextMenuOpeningCommand property.
        /// </summary>
        public static ICommand GetContextMenuOpeningCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(ContextMenuOpeningCommandProperty);
        }

        /// <summary>
        /// Sets the ContextMenuOpeningCommand property.
        /// </summary>
        public static void SetContextMenuOpeningCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(ContextMenuOpeningCommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the ContextMenuOpeningCommand property.
        /// </summary>
        private static void OnContextMenuOpeningCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as FrameworkElement;
            if (element == null)
            {
                return;
            }
            if ((e.OldValue == null) && (e.NewValue != null))
            {
                element.AddHandler(FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(FrameworkElementContextMenuOpening), true);
            }
            else if ((e.OldValue != null) && (e.NewValue == null))
            {
                element.RemoveHandler(FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(FrameworkElementContextMenuOpening));
            }
        }

        private static void FrameworkElementContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // gets the sender is UIElement and check ClickCount because we want double click only
            var target = sender as FrameworkElement;
            if (target != null /*&& e.ClickCount > 1*/)
            {
                var iCommand = (ICommand)target.GetValue(ContextMenuOpeningCommandProperty);
                if (iCommand != null)
                {
                    var routedCommand = iCommand as RoutedCommand;
                    // check if the command has a parameter using the ContextMenuOpeningParameterProperty
                    object[] parameters = new object[] {
                        target.GetValue(ContextMenuOpeningParameterProperty) ?? target,
                        e 
                    };
                    // execute the command

                    if (routedCommand != null)
                    {
                        routedCommand.Execute(parameters, target);
                    }
                    else
                    {
                        iCommand.Execute(parameters);
                    }
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region ContextMenuOpeningParameter
        /// <summary>
        /// ContextMenuOpeningParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ContextMenuOpeningParameterProperty =
            DependencyProperty.RegisterAttached(
                "ContextMenuOpeningParameter",
                typeof(object),
                typeof(ClickCommands),
                new FrameworkPropertyMetadata((object)null, null));

        /// <summary>
        /// Gets the ContextMenuOpeningParameter property.
        /// </summary>
        public static object GetContextMenuOpeningParameter(DependencyObject d)
        {
            return d.GetValue(ContextMenuOpeningParameterProperty);
        }

        /// <summary>
        /// Sets the ContextMenuOpeningParameter property.
        /// </summary>
        public static void SetContextMenuOpeningParameter(DependencyObject d, object value)
        {
            d.SetValue(ContextMenuOpeningParameterProperty, value);
        }
        #endregion

        #region SelectionChangedCommand
        /// <summary>
        /// SelectionChangedCommand Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.RegisterAttached(
                "SelectionChangedCommand",
                typeof(ICommand),
                typeof(ClickCommands),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectionChangedCommandChanged)));

        /// <summary>
        /// Gets the SelectionChangedCommand property.
        /// </summary>
        public static ICommand GetSelectionChangedCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(SelectionChangedCommandProperty);
        }

        /// <summary>
        /// Sets the SelectionChangedCommand property.
        /// </summary>
        public static void SetSelectionChangedCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(SelectionChangedCommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the SelectionChangedCommand property.
        /// </summary>
        private static void OnSelectionChangedCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TabControl;
            if (element == null)
            {
                return;
            }
            if ((e.OldValue == null) && (e.NewValue != null))
            {
                element.AddHandler(TabControl.SelectionChangedEvent, new SelectionChangedEventHandler(UiElementSelectionChanged), true);
            }
            else if ((e.OldValue != null) && (e.NewValue == null))
            {
                element.RemoveHandler(TabControl.SelectionChangedEvent, new SelectionChangedEventHandler(UiElementSelectionChanged));
            }
        }

        private static void UiElementSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // gets the sender is UIElement and check ClickCount because we want double click only
            var target = sender as UIElement;
            if (target != null /*&& e.ClickCount > 1*/)
            {
                var iCommand = (ICommand)target.GetValue(SelectionChangedCommandProperty);
                if (iCommand != null)
                {
                    var routedCommand = iCommand as RoutedCommand;
                    //var relayCommand = iCommand as RelayCommand;
                    //var abc = e.AddedItems[0];
                    // check if the command has a parameter using the MouseEventParameterProperty
                    //object parameter = target.GetValue(SelectionChangedParameterProperty) ?? target;
                    object parameter = target.GetValue(SelectionChangedParameterProperty) ?? e;
                    // execute the command
                    if (routedCommand != null)
                    {
                        routedCommand.Execute(parameter, target);
                    }
                    else
                    {
                        iCommand.Execute(parameter);
                    }
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region SelectionChangedParameter
        /// <summary>
        /// SelectionChangedParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectionChangedParameterProperty =
            DependencyProperty.RegisterAttached(
                "SelectionChangedParameter",
                typeof(object),
                typeof(ClickCommands),
                new FrameworkPropertyMetadata((object)null, null));

        /// <summary>
        /// Gets the SelectionChangedParameter property.
        /// </summary>
        public static object GetSelectionChangedParameter(DependencyObject d)
        {
            return d.GetValue(SelectionChangedParameterProperty);
        }

        /// <summary>
        /// Sets the SelectionChangedParameter property.
        /// </summary>
        public static void SetSelectionChangedParameter(DependencyObject d, object value)
        {
            d.SetValue(SelectionChangedParameterProperty, value);
        }
        #endregion

    }
}

