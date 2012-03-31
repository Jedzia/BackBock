// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralCommandsModel.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Jedzia.BackBock.ViewModel.Commands;
    using Jedzia.BackBock.ViewModel.MainWindow;

    /// <summary>
    /// General application <see cref="ICommand"/>s.
    /// </summary>
    public sealed class GeneralCommandsModel
    {
        // : INotifyPropertyChanged
        // DesignerCanvasCommandModel commands;

        // public static readonly DependencyProperty DesignerCommandsProperty = DependencyProperty.Register(
        // "DesignerCommands", typeof(DesignerCanvasCommandModel), typeof(DesignerCanvas));

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

        // private ApplicationViewModel applicationViewModel;
        // private IMainWorkArea workArea;
        #region Fields

        private RelayCommand cancelCommand;
        private IMainWindow mainWindow;
        private MainWindowViewModel mainwindowViewmodel;

        // private RelayCommand selectAllCommand;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralCommandsModel"/> class.
        /// </summary>
        /// <param name="mainwindowViewmodel">The main window <see cref="ViewModel"/>.</param>
        /// <param name="mainWindow">The main window.</param>
        public GeneralCommandsModel(
            /*ApplicationViewModel applicationViewModel,*/
            MainWindowViewModel mainwindowViewmodel,
            IMainWindow mainWindow)
        {
            // this.applicationViewModel = applicationViewModel;
            this.mainWindow = mainWindow;
            this.mainwindowViewmodel = mainwindowViewmodel;
            mainWindow.Initialized += this.MainWindowInitialized;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return this.cancelCommand ??
                       (this.cancelCommand = new RelayCommand(
                                                 o => this.mainwindowViewmodel.Cancel(),
                                                 sender => true));
            }
        }

        /// <summary>
        /// Gets the select all command.
        /// </summary>
        [CommandKeyGesture(Key.A, ModifierKeys.Control, "mainWindow")]
        public ICommand SelectAllCommand
        {
            get
            {
                return this.selectAllCommand ?? (this.selectAllCommand = new RelayCommand(this.SelectAllExecuted));
            }
        }

        #endregion

        /// <summary>
        /// Cleans this instance up.
        /// </summary>
        public void CleanUp()
        {
            this.mainwindowViewmodel = null;
            this.mainWindow = null;
        }

        private void CopyEnabled(object o, CanExecuteRoutedEventArgs args)
        {
            // this.designerCanvas.CopyPasteProcessor.CopyEnabled(o, args);
        }

        private void CopyExecuted(object o, ExecutedRoutedEventArgs args)
        {
            // this.designerCanvas.CopyPasteProcessor.CopyExecuted(o, args);
        }

        private void CutEnabled(object o, CanExecuteRoutedEventArgs args)
        {
            // this.designerCanvas.CopyPasteProcessor.CutEnabled(o, args);
        }

        private void CutExecuted(object o, ExecutedRoutedEventArgs args)
        {
            // this.designerCanvas.CopyPasteProcessor.CutExecuted(o, args);
        }

        /// <summary>
        /// Mains the window initialized.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentNullException">main Window WorkArea is <c>null</c>.</exception>
        private void MainWindowInitialized(object sender, EventArgs e)
        {
            if (this.mainWindow.WorkArea == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }

            var workArea = this.mainWindow.WorkArea;

            // the global ApplicationCommands adding.
            // this.designerCanvas.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            workArea.AddCommandBinding(ApplicationCommands.New, this.NewExecuted);
            workArea.AddCommandBinding(ApplicationCommands.Open, this.OpenExecuted);
            workArea.AddCommandBinding(ApplicationCommands.Save, this.SaveExecuted, this.SaveEnabled);
            workArea.AddCommandBinding(ApplicationCommands.Print, this.PrintExecuted);

            // this.designerCanvas.CommandBindings.Add(new CommandBinding(this.PasteCommand, this.Paste_Executed, Paste_Enabled));
            workArea.AddCommandBinding(ApplicationCommands.Cut, this.CutExecuted, this.CutEnabled);
            workArea.AddCommandBinding(ApplicationCommands.Copy, this.CopyExecuted, this.CopyEnabled);
            workArea.AddCommandBinding(ApplicationCommands.Paste, this.PasteExecuted, this.Paste_Enabled);
            workArea.AddCommandBinding(ApplicationCommands.Delete, this.DeleteExecuted, this.DeleteEnabled);

            // this.designerCanvas.CommandBindings.Add(new CommandBinding(SelectAllCommand, SelectAll_Executed));
            // SelectAllCommand.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            // System.Windows.Input.KeyBinding f = new KeyBinding(
            // mainWindow.InputBindings.Add(new KeyBinding(this.SelectAllCommand, new KeyGesture(Key.A, ModifierKeys.Control)));
            this.mainWindow.InputBindings.Add(new KeyBinding(this.CancelCommand, new KeyGesture(Key.Escape)));

            // Apply KeyGestures from Attributes.
            CommandKeyGestureAttribute.ApplyKeyGestures(GetType(), this.mainWindow, this);

            // and finally detach from the Initialized event.
            this.mainWindow.Initialized -= this.MainWindowInitialized;
        }

        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // designerCanvas.Children.Clear();
            // designerCanvas.SelectionService.ClearSelection();
        }

        private void OpenExecuted(object o, ExecutedRoutedEventArgs args)
        {
            this.mainwindowViewmodel.Open();
        }

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // designerCanvas.CopyPasteProcessor.PasteExecuted(this, null);
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        private void SaveEnabled(object sender, CanExecuteRoutedEventArgs args)
        {
            // Todo: make a summary HasErrors property for mainwindowViewmodel.
            bool canExecute = !this.mainwindowViewmodel.Data.HasErrors;

            // bool canExecute = true;
            args.CanExecute = canExecute;
        }

        private void SaveExecuted(object o, ExecutedRoutedEventArgs args)
        {
            this.mainwindowViewmodel.Save();
        }

        private void SelectAllExecuted(object e)
        {
            // designerCanvas.SelectionService.SelectAll();
        }

        #region Print Command

        /// <summary>
        /// Prints the task data.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ApplicationException">The DesignerCanvas can't be printed (no cast to Visual).</exception>
        private void PrintExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // designerCanvas.SelectionService.ClearSelection();
            var printDialog = new PrintDialog();

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

        private void DeleteEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            // e.CanExecute = this.designerCanvas.SelectionService.CurrentSelection.Count() > 0;
        }

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // this.designerCanvas.CopyPasteProcessor.DeleteCurrentSelection();
        }

        #endregion
    }
}