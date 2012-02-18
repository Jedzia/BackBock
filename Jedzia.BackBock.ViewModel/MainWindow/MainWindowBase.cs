// --------------------------------------------------------------------------------------------------------------------
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
    using Jedzia.BackBock.ViewModel.Diagram.Designer;

    public abstract class MainWindowBase : Window, IMainWindow
    {
        /*protected virtual IDesignerCanvas GetDesigner()
        {
            return null;
        }*/
        #region Properties

        public IDesignerCanvas Designer
        {
            get
            {
                return this.GetDesigner();
            }
        }

        #endregion

        protected abstract IDesignerCanvas GetDesigner();

        protected void Test()
        {
            // this.Initialized
        }
    }
}