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
using System.ComponentModel;

namespace Jedzia.BackBock.Application.Editors.TaskWizard
{
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime;

    /// <summary>
    /// Interaction logic for TaskWizard.xaml
    /// </summary>
    public partial class TaskWizard : Window, IStateWizard, IDestructible
    {
        #region IDestructible Members

        private ILifetimeEnds candidate;

        public ILifetimeEnds Candidate
        {
            get
            {
                return this.candidate;
            }
            set
            {
                this.candidate = value;
            }
        }

        #endregion

        public TaskWizard()
        {
            InitializeComponent();
            var vm = ((TaskWizardViewModel)this.DataContext);
            vm.Wizard = this;
            // end the lifetime of the viewmodel. 
            // Todo: better put this in an ioc factory
            //this.Closed += (o, e) => { if (vm.Candidate != null) vm.Candidate.Release(); };
            Closed += (o, e) =>
            {
                if (Candidate != null)
                    Candidate.Release();
                vm.Wizard = null;
                //Candidate = null;
                //this.DataContext = null;
            };
            //vm.Reset();
        }

        #region IStateWizard Members

        public int PageCount
        {
            get { return 2; }
        }

        public int SelectedPage
        {
            get { return this.wizard.SelectedIndex; }
            set
            {
                if (this.wizard.SelectedIndex == value)
                {
                    return;
                }
                this.wizard.SelectedIndex = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("SelectedPage"));
            }
        }

        #endregion
    }
}
