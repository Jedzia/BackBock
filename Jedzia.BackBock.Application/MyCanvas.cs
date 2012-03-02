// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyCanvas.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2019, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Application
{
    using System.Windows;
    using System.Windows.Controls;
    using Jedzia.BackBock.ViewModel.MainWindow;

    public class MyCanvas : DockPanel, IMainWorkArea
    {
        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        #region Fields

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(MyCanvas), new UIPropertyMetadata(null));

        #endregion

        #region Properties

        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        #endregion
    }
}