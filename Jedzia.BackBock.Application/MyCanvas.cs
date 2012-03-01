using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jedzia.BackBock.Application
{
    using Jedzia.BackBock.ViewModel.MainWindow;

    public class MyCanvas : DockPanel, IMainWorkArea
    {


        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(MyCanvas), new UIPropertyMetadata(null));


        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyCanvas"/> class.
        /// </summary>
        public MyCanvas()
            : base()
        {
        }
    }

}
