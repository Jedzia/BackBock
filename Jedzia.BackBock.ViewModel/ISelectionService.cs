﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel
{
    public interface IDialogServiceProvider
    {
        IDialogService DialogService { get; }
    }
}
