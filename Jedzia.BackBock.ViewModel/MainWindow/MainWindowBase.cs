﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowBase.cs" company="">
//   
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>
//   Defines the IMainWindow type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    using System.Windows;

    public class MainWindowBase : Window, IMainWindow
    {
        /*protected virtual IMainWorkArea GetDesigner()
        {
            return null;
        }*/
        #region Properties

        public IMainWorkArea Designer
        {
            get
            {
                return this.GetDesigner();
            }
        }

        #endregion

        protected virtual IMainWorkArea GetDesigner()
        {
            return null;
        }

        protected void Test()
        {
            // this.Initialized
        }
    }
}