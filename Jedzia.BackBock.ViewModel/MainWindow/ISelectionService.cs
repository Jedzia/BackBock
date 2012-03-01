using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    /// <summary>
    /// Provides a selection service.
    /// </summary>
    public interface ISelectionService
    {
        /// <summary>
        /// Gets the current selected item.
        /// </summary>
        object SelectedItem { get; }
    }
}
