using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Wizard
{
    public interface IDialog
    {
        void Close();
        bool? ShowDialog();
    }

    public interface IDialogWindow : IDialog
    {
        object DataContext { get; set; }
    }

    public interface IStateWizard : IDialog
    {
        int PageCount { get; }
        int SelectedPage { get; set; }
    }
}
