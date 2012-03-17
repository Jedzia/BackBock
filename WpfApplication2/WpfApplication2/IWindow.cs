using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    public interface IWindow
    {
        void Close();

        IWindow CreateChild(object viewModel);

        void Show();

        bool? ShowDialog();
    }
}
