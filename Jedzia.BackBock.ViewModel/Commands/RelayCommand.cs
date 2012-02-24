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

    public class RelayCommand : /*DependencyObject,*/ ICommand
    {
        #region Constructors

        // Example
        // var cmd = new RelayCommand(new KeyGesture(Key.F7), typeof(DesignerItemWithData), 
        //        this.CommandNameExecuted, this.CommandNameEnabled);
        public RelayCommand(KeyGesture kg, Type registeredType, Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.InputGestures.Add(kg);
            InputBinding ib = new InputBinding(this, kg);
            CommandManager.RegisterClassInputBinding(registeredType, ib);
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            Guard.NotNull(() => execute, execute);
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {

        }

        #endregion

        private InputGestureCollection inputGestures;

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

        public object InputGestureText
        {
            get
            {
                return GetInputGestureText(InputGestures);
            }
        }

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

        public bool CanExecute(object parameter)
        {
            var lcanExecute = true;
            if (this.canExecute != null)
            {
                lcanExecute = this.canExecute(parameter);
            }
            return lcanExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
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
            remove { CommandManager.RequerySuggested -= value; }
        }

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
