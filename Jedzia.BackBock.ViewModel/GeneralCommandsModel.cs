// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesignerCanvasCommandModel.Commands.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>
//   Defines the DesignerCanvasCommandModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Controls;
    using System;
    using System.Windows;
    using Jedzia.BackBock.ViewModel.Commands;
    using Jedzia.BackBock.ViewModel.MainWindow;

    public sealed class GeneralCommandsModel //: INotifyPropertyChanged
    {
        //DesignerCanvasCommandModel commands;

        //public static readonly DependencyProperty DesignerCommandsProperty = DependencyProperty.Register(
        //"DesignerCommands", typeof(DesignerCanvasCommandModel), typeof(DesignerCanvas));

        /*public DesignerCanvasCommandModel DesignerCommands
        {
            get
            {
                if (commands == null)
                {
                    commands = new DesignerCanvasCommandModel(designerCanvas);
                }
                return commands;
            }
        }*/
       /* /// <summary>
        /// Gets or sets 
        /// </summary>
        public IMainWindow MainWindow
        {
            get
            {
                return this.mainWindow;
            }

            //internal set
            //{
            //    this.mainWindow = value;
            //}
        }*/
        //private ApplicationViewModel applicationViewModel;
        //private IMainWorkArea workArea;
        MainWindowViewModel mainwindowViewmodel;
        private IMainWindow mainWindow;

        public void CleanUp()
        {
            this.mainwindowViewmodel = null;
            this.mainWindow = null;
        }

        public GeneralCommandsModel(/*ApplicationViewModel applicationViewModel,*/
            MainWindowViewModel mainwindowViewmodel,
            IMainWindow mainWindow)
        {
            //this.applicationViewModel = applicationViewModel;
            this.mainWindow = mainWindow;
            this.mainwindowViewmodel = mainwindowViewmodel;
            mainWindow.Initialized += this.MainWindowInitialized;

        }

        /// <exception cref="ArgumentNullException"><paramref name="mainWindow.Designer" /> is <c>null</c>.</exception>
        void MainWindowInitialized(object sender, EventArgs e)
        {
            if (this.mainWindow.WorkArea == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }
            var workArea = this.mainWindow.WorkArea;

            // the global ApplicationCommands adding.
            //this.designerCanvas.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            workArea.AddCommandBinding(ApplicationCommands.New, this.NewExecuted);
            workArea.AddCommandBinding(ApplicationCommands.Open, this.OpenExecuted);
            workArea.AddCommandBinding(ApplicationCommands.Save, this.SaveExecuted);
            workArea.AddCommandBinding(ApplicationCommands.Print, this.PrintExecuted);

            // this.designerCanvas.CommandBindings.Add(new CommandBinding(this.PasteCommand, this.Paste_Executed, Paste_Enabled));
            workArea.AddCommandBinding(ApplicationCommands.Cut, this.CutExecuted, this.CutEnabled);
            workArea.AddCommandBinding(ApplicationCommands.Copy, this.CopyExecuted, this.CopyEnabled);
            workArea.AddCommandBinding(ApplicationCommands.Paste, this.PasteExecuted, Paste_Enabled);
            workArea.AddCommandBinding(ApplicationCommands.Delete, this.DeleteExecuted, this.DeleteEnabled);

            //this.designerCanvas.CommandBindings.Add(new CommandBinding(SelectAllCommand, SelectAll_Executed));
            //SelectAllCommand.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            //System.Windows.Input.KeyBinding f = new KeyBinding(
            //mainWindow.InputBindings.Add(new KeyBinding(this.SelectAllCommand, new KeyGesture(Key.A, ModifierKeys.Control)));

            // Apply KeyGestures from Attributes.
            CommandKeyGestureAttribute.ApplyKeyGestures(GetType(), mainWindow, this);
            // and finally detach from the Initialized event.
            mainWindow.Initialized -= this.MainWindowInitialized;
        }


        //private RelayCommand selectAllCommand;
        /*private static ICommand selectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (selectAllCommand == null)
                {
                    selectAllCommand = new RoutedCommand("SelectAll", typeof(ApplicationCommandModel), 
                        new InputGestureCollection() { new KeyGesture(Key.A, ModifierKeys.Control) });
                }

                return selectAllCommand;
            }
        }*/
        private RelayCommand selectAllCommand;

        [CommandKeyGesture(Key.A, ModifierKeys.Control, "mainWindow")]
        public ICommand SelectAllCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.selectAllCommand == null)
                {
                    this.selectAllCommand = new RelayCommand(this.SelectAllExecuted);
                }

                return this.selectAllCommand;
            }
        }

        private void SelectAllExecuted(object e)
        {
            //designerCanvas.SelectionService.SelectAll();
        }

        /*private bool SelectAll_Enabled(object sender)
        {
            bool canExecute = designerCanvas.SelectionService.CurrentSelection.Count() > 0;
            return canExecute;
        }*/


        /*private RelayCommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.newCommand == null)
                {
                    this.newCommand = new RelayCommand(e => this.New_Executed());
                }

                return this.newCommand;
            }
        }*/

        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //designerCanvas.Children.Clear();
            //designerCanvas.SelectionService.ClearSelection();
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }
        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //designerCanvas.CopyPasteProcessor.PasteExecuted(this, null);
        }

        private void CutExecuted(object o, ExecutedRoutedEventArgs args)
        {
            //this.designerCanvas.CopyPasteProcessor.CutExecuted(o, args);
        }


        private void CutEnabled(object o, CanExecuteRoutedEventArgs args)
        {
            //this.designerCanvas.CopyPasteProcessor.CutEnabled(o, args);
        }

        private void CopyEnabled(object o, CanExecuteRoutedEventArgs args)
        {
            //this.designerCanvas.CopyPasteProcessor.CopyEnabled(o, args);
        }

        private void CopyExecuted(object o, ExecutedRoutedEventArgs args)
        {
            //this.designerCanvas.CopyPasteProcessor.CopyExecuted(o, args);
        }



        private void OpenExecuted(object o, ExecutedRoutedEventArgs args)
        {
            var path = ApplicationViewModel.MainIOService.OpenFileDialog(string.Empty);
            
            //this.mainWindow.WorkArea.
            //ViewModelLocator.MainStatic.OpenFile(path);
            mainwindowViewmodel.OpenFile(path);
            //designerCanvas.DesignerCanvasFileProcessor.OpenExecuted(o, args);
        }
        private void SaveExecuted(object o, ExecutedRoutedEventArgs args)
        {
            var path = ApplicationViewModel.MainIOService.SaveFileDialog(string.Empty);
            //ViewModelLocator.MainStatic.SaveFile(path);
            mainwindowViewmodel.SaveFile(path);
            //designerCanvas.DesignerCanvasFileProcessor.SaveExecuted(o, args);
        }

        #region Print Command

        /// <exception cref="ApplicationException">The DesignerCanvas can't be printed (no cast to Visual).</exception>
        private void PrintExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //designerCanvas.SelectionService.ClearSelection();

            PrintDialog printDialog = new PrintDialog();

            if (true == printDialog.ShowDialog())
            {
                var visual = this.mainWindow.WorkArea as Visual;
                if (visual == null)
                {
                    throw new ApplicationException("The DesignerCanvas can't be printed (no cast to Visual).");
                }
                printDialog.PrintVisual(visual, "WPF Diagram");
            }
        }

        #endregion

        #region Delete Command

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //this.designerCanvas.CopyPasteProcessor.DeleteCurrentSelection();
        }

        private void DeleteEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = this.designerCanvas.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

    }
}