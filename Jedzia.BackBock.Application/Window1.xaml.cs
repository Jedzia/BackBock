namespace Jedzia.BackBock.Application
{
    using System.Windows;
    using Jedzia.BackBock.ViewModel.Diagram.Designer;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using System.Collections.Generic;
    using System.Windows.Controls;
    //    using Jedzia.BackBock.Application.Resources.Styles.Resources.Styles;

    public partial class Window1 : MainWindowBase
    {
        public Window1()
        {
            //mainWindowViewModel = new MainWindowViewModel(this);
            InitializeComponent();

            //this.DataContext = this;
            //this.InputBindings

        }

        protected override IDesignerCanvas GetDesigner()
        {
            return this.MyDesigner;
        }

        MainWindowViewModel mainWindowViewModel;

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                if (mainWindowViewModel == null)
                {
                    mainWindowViewModel = new MainWindowViewModel(App.ApplicationViewModel, this);
                }
                return mainWindowViewModel;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*Classlistbox_xaml res = new Classlistbox_xaml();
            res.InitializeComponent();
            var exp = res["ClassListDataTemplate-CSharp"];*/
        }

        private void MainWindowBase_Initialized(object sender, System.EventArgs e)
        {
            // Register necessary designer sub windows. Todo: Maybe this can be static.
            //ClassSpecificationWindow wnd = new ClassSpecificationWindow();
            //wnd.ShowDialog();
        }

        static Window1()
        {
            //App.ApplicationViewModel.RegisterControl(DesignerCanvas.WindowTypes.ClassSpecificationWindow,
            //    typeof(ClassSpecificationWindow));
        }
    }

}
