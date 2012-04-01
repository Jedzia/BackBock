namespace Jedzia.BackBock.ViewModel.Commands
{
    using System;
    using System.Windows.Input;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Diagnostics.CodeAnalysis;
    using Jedzia.BackBock.ViewModel.Util;

    /// <summary>
    /// Implementation of the Relay Command <see cref="ICommand"/> pattern.
    /// </summary>
    public class RelayCommand : /*DependencyObject,*/ ICommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="kg">The associated <see cref="KeyGesture"/>.</param>
        /// <param name="registeredType">Type of the registered command.</param>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate that determines if the command can be executed.</param>
        /// <example>
        /// <code><![CDATA[
        /// var cmd = new RelayCommand(new KeyGesture(Key.F7), typeof(DesignerItemWithData), 
        ///        this.CommandNameExecuted, this.CommandNameEnabled);
        /// ]]>
        /// </code>
        /// </example>
        public RelayCommand(KeyGesture kg, Type registeredType, Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.InputGestures.Add(kg);
            InputBinding ib = new InputBinding(this, kg);
            CommandManager.RegisterClassInputBinding(registeredType, ib);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate that determines if the command can be executed.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            Guard.NotNull(() => execute, execute);
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {

        }

        #endregion

        private InputGestureCollection inputGestures;

        /// <summary>
        /// Gets the input gestures associated with this command.
        /// </summary>
        public InputGestureCollection InputGestures
        {
            get
            {
                if (this.inputGestures == null)
                {
                    this.inputGestures = new InputGestureCollection();
                }
                return this.inputGestures;
            }
        }

        /// <summary>
        /// Gets the input gesture text.
        /// </summary>
        public object InputGestureText
        {
            get
            {
                return GetInputGestureText(InputGestures);
            }
        }

        /// <summary>
        /// Gets the input gesture text from a specified collection of input gestures.
        /// </summary>
        /// <param name="inputGestures">The input gestures.</param>
        /// <returns>The string representing the gesture.</returns>
        public static string GetInputGestureText(InputGestureCollection inputGestures)
        {
            if (inputGestures == null)
            {
                return null;
            }
            for (int i = 0; i < inputGestures.Count; i++)
            {
                KeyGesture gesture = ((IList)inputGestures)[i] as KeyGesture;
                if (gesture != null)
                {
                    return gesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
                }
            }
            return null;
        }


        #region ICommand Implementation

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            var lcanExecute = true;
            if (this.canExecute != null)
            {
                lcanExecute = this.canExecute(parameter);
            }
            return lcanExecute;
        }

        /// <summary>
        /// Occurs when [can execute changed].
        /// </summary>
        private event EventHandler canExecuteChanged;

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public void RaiseCanExecuteChanged()
        {
            var handler = canExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                canExecuteChanged += value;
                /*var menu = value.Target as MenuItem;
                if (menu != null)
                {
                    var found = menu.Parent;
                    var par = VisualTreeHelper.GetParent(menu);
                    //var par1 = VisualTreeHelper.GetParent(par);
                    //var par2 = VisualTreeHelper.GetParent(par1);
                    //var par3 = VisualTreeHelper.GetParent(par2);
                    //var par4 = VisualTreeHelper.GetParent(par3);
                    foreach (var item in InputGestures)
                    {
                        InputBinding ib = new InputBinding(this, (InputGesture)item);
                        menu.InputBindings.Add(ib);
                    }
                }*/
            }

            remove 
            { 
                CommandManager.RequerySuggested -= value;
                canExecuteChanged -= value;
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute(parameter);
            }
        }

        #endregion

        #region Fields

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        #endregion
    }
}
