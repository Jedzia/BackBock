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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, IWindow
    {
        public Window1(/*MainViewModel viewmodel*/)
        {
            //this.DataContext = viewmodel;
            InitializeComponent();
        }

        #region IWindow Members


        public IWindow CreateChild(object viewModel)
        {
            var cw = new SubWindow();
            cw.Owner = this;
            cw.DataContext = viewModel;
            //WindowAdapter.ConfigureBehavior(cw);

            return cw;
        }

        #endregion
    }
}
