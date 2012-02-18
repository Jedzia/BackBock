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
    using Jedzia.BackBock.ViewModel.Diagram.Designer;

    public sealed class ApplicationCommandModel //: INotifyPropertyChanged
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

        private IDesignerCanvas designerCanvas;
        private readonly IMainWindow mainWindow;
        public ApplicationCommandModel(IMainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Initialized += this.MainWindowInitialized;

        }

        /// <exception cref="ArgumentNullException"><paramref name="mainWindow.Designer" /> is <c>null</c>.</exception>
        void MainWindowInitialized(object sender, EventArgs e)
        {
            if (this.mainWindow.Designer == null)
            {
                throw new ArgumentNullException("mainWindow", "No Designer!");
            }
            this.designerCanvas = this.mainWindow.Designer;

            // the global ApplicationCommands adding.
            //this.designerCanvas.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            this.designerCanvas.AddCommandBinding(ApplicationCommands.New, this.NewExecuted);
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Open, this.OpenExecuted);
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Save, this.SaveExecuted);
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Print, this.PrintExecuted);

            // this.designerCanvas.CommandBindings.Add(new CommandBinding(this.PasteCommand, this.Paste_Executed, Paste_Enabled));
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Cut, this.CutExecuted, this.CutEnabled);
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Copy, this.CopyExecuted, this.CopyEnabled);
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Paste, this.PasteExecuted, Paste_Enabled);
            this.designerCanvas.AddCommandBinding(ApplicationCommands.Delete, this.DeleteExecuted, this.DeleteEnabled);

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
            //designerCanvas.DesignerCanvasFileProcessor.OpenExecuted(o, args);
        }
        private void SaveExecuted(object o, ExecutedRoutedEventArgs args)
        {
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
                var visual = this.designerCanvas as Visual;
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