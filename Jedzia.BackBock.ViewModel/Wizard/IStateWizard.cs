using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.Wizard
{
    public interface IStateWizard
    {
        int PageCount { get; }
        int SelectedPage { get; set; }

        void Close();
        bool? ShowDialog();
    }
}
