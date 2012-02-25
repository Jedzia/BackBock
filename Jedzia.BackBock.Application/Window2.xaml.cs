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
using Jedzia.BackBock.ViewModel.MainWindow;

namespace Jedzia.BackBock.Application
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window, IMainWindow
    {
        public Window2()
        {
            this.DataContext = new MainWindowViewModel(App.ApplicationViewModel, this);
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //this.DataContext = MainWindowViewModel.GetSampleData();
            //this.MyDesigner.DataContext = MainWindowViewModel.GetSampleData();
        }

        #region IMainWindow Members

        public IMainWorkArea Designer
        {
            get { return this.MyDesigner as IMainWorkArea; }
        }

        #endregion

        #region IMainWindow Members


        public MainWindowViewModel MainWindowViewModel
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
