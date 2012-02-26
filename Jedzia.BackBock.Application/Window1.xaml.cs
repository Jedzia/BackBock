namespace Jedzia.BackBock.Application
{
    using System.Windows;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.ComponentModel;
    using System.Collections;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc;
    //    using Jedzia.BackBock.Application.Resources.Styles.Resources.Styles;

    public partial class Window1 : Window, IMainWindow
    {
        public Window1()
        {
            //mainWindowViewModel = new MainWindowViewModel(this);
            InitializeComponent();

            //this.DataContext = this;
            //this.InputBindings

        }

        public IMainWorkArea Designer
        {
            get
            {
                return this.MyDesigner;
            }
        }
        
       
        //MainWindowViewModel mainWindowViewModel;

        /*public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                //if (mainWindowViewModel == null)
                //{
                    //mainWindowViewModel = new MainWindowViewModel(App.ApplicationViewModel, this);
                //}
                //return mainWindowViewModel;
                    return ViewModelLocator.MainStatic;
            }
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*Classlistbox_xaml res = new Classlistbox_xaml();
            res.InitializeComponent();
            var exp = res["ClassListDataTemplate-CSharp"];*/
            //wpg.Instance = this.MyDesigner.DataContext;
            //var modl = (BackupDataViewModel)MyDesigner.DataContext;
            //ShowDetail(modl.BackupItems[0]);
        }

        private void MainWindowBase_Initialized(object sender, System.EventArgs e)
        {
            // Register necessary designer sub windows. Todo: Maybe this can be static.
            //ClassSpecificationWindow wnd = new ClassSpecificationWindow();
            //wnd.ShowDialog();
        }

        static Window1()
        {
            ApplicationViewModel.RegisterControl(Jedzia.BackBock.ViewModel.MainWindow.MainWindowViewModel.WindowTypes.TaskEditor,
                typeof(Editors.TaskEditorWindow));
        }

    }

}
