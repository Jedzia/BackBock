﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace WpfApplication2
{
    public class MainWindowAdapter : WindowAdapter
    {
        //private readonly IMainWindowViewModelFactory vmFactory;
        private bool initialized;

        public MainWindowAdapter(Window wpfWindow/*, IMainWindowViewModelFactory viewModelFactory*/)
            : base(wpfWindow)
        {
            /*if (viewModelFactory == null)
            {
                throw new ArgumentNullException("viewModelFactory");
            }

            this.vmFactory = viewModelFactory;*/
        }

        #region IWindow Members

        public override void Close()
        {
            this.EnsureInitialized();
            base.Close();
        }

        public override IWindow CreateChild(object viewModel)
        {
            this.EnsureInitialized();
            return base.CreateChild(viewModel);
        }

        public override void Show()
        {
            this.EnsureInitialized();
            base.Show();
        }

        public override bool? ShowDialog()
        {
            this.EnsureInitialized();
            return base.ShowDialog();
        }

        #endregion

        /*private void DeclareKeyBindings(MainWindowViewModel vm)
        {
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.RefreshCommand, new KeyGesture(Key.F5)));
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.InsertProductCommand, new KeyGesture(Key.Insert)));
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.EditProductCommand, new KeyGesture(Key.Enter)));
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.DeleteProductCommand, new KeyGesture(Key.Delete)));
        }*/

        private void EnsureInitialized()
        {
            if (this.initialized)
            {
                return;
            }

            //var vm = this.vmFactory.Create(this);
            //this.WpfWindow.DataContext = vm;
            //this.DeclareKeyBindings(vm);

            this.initialized = true;
        }
    }
}
