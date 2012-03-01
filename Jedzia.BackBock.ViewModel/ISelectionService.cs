using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel
{
    /// <summary>
    /// Provides a DialogService.
    /// </summary>
    public interface IDialogServiceProvider
    {
        /// <summary>
        /// Gets the dialog service for this instance.
        /// </summary>
        IDialogService DialogService { get; }
    }
}
