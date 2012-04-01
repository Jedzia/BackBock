// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.CustomControls.NavBar
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using System.Collections.Generic;
    using System.ComponentModel;

    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_ContentTreeView", Type = typeof(TreeView))]
    public class NavBarItem : TabItem
    {
        #region Fields

        internal static bool USEmres = false;
        internal static ResourceDictionary mres = null;

        /*internal static ResourceDictionary mres = new ResourceDictionary
                                                      {
                                                          Source =
                                                              new Uri(
                                                              "pack://application:,,,/Jedzia.BackBock.CustomControls;component/NavBar/NavBarStyles.xaml")
                                                      };*/

        #endregion

        #region Constructors

        static NavBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavBarItem), new FrameworkPropertyMetadata(typeof(NavBarItem)));
            return;
            /*ResourceDictionary destination = Application.Current.Resources;
            bool reinitDestination = false;
            Application.Current.Dispatcher.BeginInvoke(
                (ChangeTheme)((d, reinit) =>
                                  {
                                      d.BeginInit();
                                      d.MergedDictionaries.Add(mres);
                                      //d.MergedDictionaries.RemoveAt(0);
                                      d.EndInit();
                                  }
                             ),
                DispatcherPriority.ApplicationIdle,
                destination,
                reinitDestination);*/
        }

        //static NavBarStyles n = new NavBarStyles();
        public NavBarItem()
        {
            //InitializeComponent();

            /*if (USEmres)
            {
                var res = mres["NavBarItemStyle"] as Style;
                Style = res;
            }*/
            //this.ApplyTemplate();
            //this.Nodes = RootNodeTreeView.InitTree();
            Loaded += this.NavBarItem_Loaded;
        }

        #endregion

        #region Delegates

        private delegate void ChangeTheme(ResourceDictionary destination, bool reinitDestination);

        #endregion

        private void NavBarItem_Loaded(object sender, RoutedEventArgs e)
        {
            //var style = mres["NavBarItemStyle"] as Style;
            //var dtt = mres["RootNodeTreeViewDataTemplate"] as HierarchicalDataTemplate;

            //this.Style = style;
            //this.ApplyTemplate();
            if (Template == null)
            {
                return;
            }
            var cntpre = Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
            //cntpre.ApplyTemplate();
            if (cntpre == null)
            {
                return;
            }
            var cnttree = cntpre.ContentTemplate.FindName("PART_ContentTreeView", cntpre) as RootNodeTreeView;
            if (cnttree != null)
            {
                //cnttree.ItemTemplate = dtt;
                //cnttree.ApplyTemplate();
                cnttree.MouseDown += this.cnttree_MouseDown;
                cnttree.SelectedItemChanged += this.cnttree_SelectedItemChanged;
                cnttree.GotFocus += this.cnttree_GotFocus;
                //var cntprb = this.Template.FindName("Bd", this);
                //var cntbx = this.Template.FindName("Bx", this);
                //var cntpre = this.Template.FindName("PART_Dreck", this);
            }
            Loaded -= this.NavBarItem_Loaded;
        }

        private void NewMethod(object sender)
        {
            IsSelected = true;

            /*var cnttree = cntpre.ContentTemplate.FindName("PART_ContentTreeView", cntpre) as RootNodeTreeView;
            if (cnttree != null)
            {
            }*/
            //var par1 = cnttree.Parent as StackPanel;
            //par1.Parent
        }

        private void cnttree_GotFocus(object sender, RoutedEventArgs e)
        {
            this.NewMethod(sender);
        }


        private void cnttree_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //var cnttree = sender as TreeView;
            //cnttree.CaptureMouse();
            this.NewMethod(sender);
            //e.Handled = true;
            //cnttree.RaiseEvent(e);
        }

        private void cnttree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.NewMethod(sender);
            var cnttree = sender as TreeView;
            var sel = cnttree.SelectedItem;
            var args = new RoutedPropertyChangedEventArgs<Node>((Node)e.OldValue, (Node)e.NewValue, e.RoutedEvent) { Source = e.Source, Handled = e.Handled };
            if (TreeItemClickedCommand != null /*&& TreeItemClickedCommand.CanExecute(e)*/)
            {
                TreeItemClickedCommand.Execute(args);
            }

            //e.Handled = true;
            //TreeViewItem e;
            //e.IsHitTestVisible

            /*var style = mres["RootNodeTreeViewItemContainerStyle"] as Style;
            var it = e.NewValue as TreeViewItem;
            it.ItemContainerStyle = style;
            it.ApplyTemplate();*/
        }

        /*public static readonly RoutedEvent TreeItemClickedEvent = EventManager.RegisterRoutedEvent(
            "TreeItemClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavBarItem));
        
        public event RoutedEventHandler TreeItemClicked
        {
            add { AddHandler(TreeItemClickedEvent, value); }
            remove { RemoveHandler(TreeItemClickedEvent, value); }
        }*/

        [Bindable(true)]
        [Category("Action")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand TreeItemClickedCommand
        {
            get { return (ICommand)GetValue(TreeItemClickedCommandProperty); }
            set { SetValue(TreeItemClickedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TreeItemClickedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TreeItemClickedCommandProperty =
            DependencyProperty.Register("TreeItemClickedCommand", typeof(ICommand), typeof(NavBarItem), new UIPropertyMetadata(null));



        public IEnumerable<Node> Nodes
        {
            get { return (IEnumerable<Node>)GetValue(NodesProperty); }
            set { SetValue(NodesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Nodes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodesProperty =
            DependencyProperty.Register("Nodes", typeof(IEnumerable<Node>), typeof(NavBarItem), new UIPropertyMetadata(RootNodeTreeView.InitTree()));



        public string TestString
        {
            get { return (string)GetValue(TestStringProperty); }
            set { SetValue(TestStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TestString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TestStringProperty =
            DependencyProperty.Register("TestString", typeof(string), typeof(NavBarItem), new UIPropertyMetadata("Ficken"));

    }
}