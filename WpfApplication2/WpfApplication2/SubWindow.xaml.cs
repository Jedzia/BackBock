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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    public partial class SubWindow : Window, IWindow
    {
        public SubWindow()
        {
            InitializeComponent();
        }

        #region ISubWindow Members


        public IWindow CreateChild(object viewModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
