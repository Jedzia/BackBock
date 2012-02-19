using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jedzia.BackBock.Application
{
    using Jedzia.BackBock.ViewModel.MainWindow;

    public class MyCanvas : Grid, IMainWorkArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyCanvas"/> class.
        /// </summary>
        public MyCanvas()
            : base()
        {
        }
    }

}
