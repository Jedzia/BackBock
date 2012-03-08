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

namespace Jedzia.BackBock.Application.Editors.TaskWizard
{
    /// <summary>
    /// Interaction logic for TaskWizard.xaml
    /// </summary>
    public partial class TaskWizard : Window, IStateWizard
    {
        public TaskWizard()
        {
            InitializeComponent();
            ((TaskWizardViewModel)this.DataContext).Wizard = this;
        }

        #region IStateWizard Members

        public int PageCount
        {
            get { return 2; }
        }

        public int SelectedPage
        {
            get { return this.wizard.SelectedIndex; }
        }

        #endregion
    }
}
