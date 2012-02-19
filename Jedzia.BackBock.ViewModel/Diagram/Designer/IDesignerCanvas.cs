// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.ViewModel.Diagram.Designer
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public interface IDesignerCanvas : ICanInputBind, ICanCommandBind, IInputElement
    {
        #region Properties

        UIElementCollection Children { get; }
        object DataContext { get; set; }
        #endregion

    }
}