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
using Microsoft.Practices.ServiceLocation;

namespace Jedzia.BackBock.Application
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window, IMainWindow
    {
        public Window2()
        {
            //App.ApplicationViewModel.MainWindow = this;
            this.DataContext = ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //this.DataContext = MainWindowViewModel.GetSampleData();
            //this.MyDesigner.DataContext = MainWindowViewModel.GetSampleData();
        }

        #region IMainWindow Members

        public IMainWorkArea WorkArea
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
