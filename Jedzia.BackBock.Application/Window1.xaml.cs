namespace Jedzia.BackBock.Application
{
    using System.Windows;
    using Jedzia.BackBock.ViewModel;
    using Jedzia.BackBock.ViewModel.MainWindow;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.ComponentModel;
    using System.Collections;
    using Jedzia.BackBock.ViewModel.Data;
    using System;
    using System.Diagnostics;
    //    using Jedzia.BackBock.Application.Resources.Styles.Resources.Styles;

    public partial class Window1 : Window, IMainWindow
    {

        public Window1()
        {
            //mainWindowViewModel = new MainWindowViewModel(this);
            InitializeComponent();
            //Debugger.Break();
            //var epsilon = this.DataContext;
            //this.DataContext = this;
            //this.InputBindings
            //logtext.TextChanged += this.logtext_TextChanged;
            //logtext.SourceUpdated += this.logtext_SourceUpdated;
            //Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        void logtext_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            logtext.CaretIndex = logtext.Text.Length;
        }

        void logtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            logtext.CaretIndex = logtext.Text.Length;
        }

        public IMainWorkArea WorkArea
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
            /*var main1 = ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            var main2 = ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            if (object.ReferenceEquals(main1, main2))
            {

            }
            var modl = (BackupDataViewModel)MyDesigner.DataContext;
            ShowDetail(modl.BackupItems[0]);*/
        }

        private void MainWindowBase_Initialized(object sender, System.EventArgs e)
        {
            // Register necessary designer sub windows. Todo: Maybe this can be static.
            //ClassSpecificationWindow wnd = new ClassSpecificationWindow();
            //wnd.ShowDialog();
        }

        static Window1()
        {
            //ControlRegistrator.RegisterControl(BackupItemViewModel.WindowTypes.TaskEditor,
              //  typeof(Editors.TaskEditorWindow));
            //ControlRegistrator.RegisterControl(MainWindowViewModel.WindowTypes.TaskWizard,
            //    typeof(Jedzia.BackBock.CustomControls.Wizard.StateWizard));
            //ControlRegistrator.RegisterControl(MainWindowViewModel.WindowTypes.TaskWizard,
              //  typeof(Editors.TaskWizard.TaskWizard));
        }

        public /*override*/ void ShowDetail(object val)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(val);
            var col = new WPG.Themes.TypeEditors.CollectionEditorControl();
            col.MyProperty = new WPG.Data.Property(val, properties["Paths"]);
            //col.MyProperty. IsReadOnly = false;
            //col.NumerableType = typeof(PathViewModel);
            col.NumerableType = val.GetType();
            col.NumerableValue = ((BackupItemViewModel)val).Paths;
            var pg = new WPG.TypeEditors.CollectionEditorWindow(col);
            pg.ShowDialog();
        }

        #region IDialogServiceProvider Members

        private IDialogService dialogService;
        public IDialogService DialogService
        {
            get
            {
                if (this.dialogService == null)
                {
                    this.dialogService = new DialogService();
                }
                return this.dialogService;
            }
        }

        #endregion

        #region IMainWindow Members


        public void UpdateLogText(string text)
        {
            logtext.AppendText(text);
            logtext.ScrollToEnd();
        }

        public void ClearLogText()
        {
            logtext.Clear();
        }

        #endregion
    }


    /*    class depp : InstanceLifetime
    {
        //protected override object CreateInstance(object initial)
        //{
         //   return initial;
        //}

        protected override object GetInstance(Dictionary<string, object> instances, string key)
        {
            if (instances.ContainsKey(key))
            {
                instances.Remove(key);
            }
            return null;
        }

        public override InstanceLifetime Release(IDestructor destruction)
        {
            return this;
        }
    }*/


}
