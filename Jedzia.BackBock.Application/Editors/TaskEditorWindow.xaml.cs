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
using Jedzia.BackBock.ViewModel.Wizard;

namespace Jedzia.BackBock.Application.Editors
{
    /// <summary>
    /// Interaction logic for TaskEditorWindow.xaml
    /// </summary>
    public partial class TaskEditorWindow : Window, IDialogWindow
    {
        public TaskEditorWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            wpg2.Instance = this.DataContext;
        }
    }
}
