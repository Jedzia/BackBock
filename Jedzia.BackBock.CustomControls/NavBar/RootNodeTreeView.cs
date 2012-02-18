// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.CustomControls.NavBar
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;

    public class RootNodeTreeView : TreeView
    {
        #region Fields

        public static readonly DependencyProperty IsRootNodeProperty =
            DependencyProperty.RegisterAttached(
                "IsRootNode", typeof(bool), typeof(RootNodeTreeView), new UIPropertyMetadata(false));



        /*public static double GetMainWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(MainWidthProperty);
        }

        public static void SetMainWidth(DependencyObject obj, double value)
        {
            obj.SetValue(MainWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MainWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainWidthProperty =
            DependencyProperty.RegisterAttached("MainWidth", typeof(double), typeof(RootNodeTreeView), new UIPropertyMetadata(0d));
        */

        #endregion



        public bool Debug
        {
            get { return (bool)GetValue(DebugProperty); }
            set { SetValue(DebugProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Debug.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DebugProperty =
            DependencyProperty.Register("Debug", typeof(bool),
            typeof(RootNodeTreeView), new UIPropertyMetadata(false));

        #region Constructors

        static RootNodeTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RootNodeTreeView), new FrameworkPropertyMetadata(typeof(RootNodeTreeView)));
        }

        public RootNodeTreeView()
        {
            //InitTree();
            //var res = NavBarItem.mres["RootNodeTreeViewItemContainerStyle"] as Style;
            //this.ItemContainerStyle = res;
            /*if (NavBarItem.USEmres)
            {
                var res = NavBarItem.mres["NavBarItemTreeViewTemplate"] as Style;
                Style = res;
            }*/
            //this.ItemTemplate = NavBarItem.mres["RootNodeTreeViewDataTemplate"] as HierarchicalDataTemplate;
            //this.ApplyTemplate();

            //this.DataContext = InitTree();
            Loaded += this.RootNodeTreeView_Loaded;
            //IEnumerable<Node>
        }




        public static Node[] InitTree()
        {
            var tr = new TreeViewItem { Header = "TreeViewItem1" };
            var trs1 = new TreeViewItem { Header = "TreeViewItem-Sub1" };
            var trs2 = new TreeViewItem { Header = "TreeViewItem-Sub2" };
            tr.Items.Add(trs1);
            tr.Items.Add(trs2);

            return new[]
                              {
                                  new Node
                                      {
                                          Text = "Root1",
                                          Children = new object[]
                                                         {
                                                             new Node { Text = "Child1" },
                                                             new Node { Text = "Child2" },
                                                             //tr
                                                         }
                                      },
                                  new Node
                                      {
                                          Text = "Root2",
                                          Children = new[]
                                                         {
                                                             new Node
                                                                 {
                                                                     Text = "Child1",
                                                                     Children = new[]
                                                                                    {
                                                                                        new Node { Text = "Child1" },
                                                                                        new Node
                                                                                            {
                                                                                                Text = "Child2",
                                                                                                Children = new[]
                                                                                                               {
                                                                                                                   new Node
                                                                                                                       {
                                                                                                                           Text
                                                                                                                               =
                                                                                                                               "Child1",
                                                                                                                           Children
                                                                                                                               =
                                                                                                                               new[
                                                                                                                               ]
                                                                                                                                   {
                                                                                                                                       new Node
                                                                                                                                           {
                                                                                                                                               Text
                                                                                                                                                   =
                                                                                                                                                   "Child1"
                                                                                                                                           }
                                                                                                                                       ,
                                                                                                                                       new Node
                                                                                                                                           {
                                                                                                                                               Text
                                                                                                                                                   =
                                                                                                                                                   "Child2"
                                                                                                                                           }
                                                                                                                                       ,
                                                                                                                                   }
                                                                                                                       }
                                                                                                                   ,
                                                                                                                   new Node
                                                                                                                       {
                                                                                                                           Text
                                                                                                                               =
                                                                                                                               "Child2"
                                                                                                                       }
                                                                                                                   ,
                                                                                                               }
                                                                                            },
                                                                                    }
                                                                 },
                                                             new Node { Text = "Child2" },
                                                         }
                                      },
                                  new Node
                                      {
                                          Text = "Root3",
                                          Children = new[]
                                                         {
                                                             new Node { Text = "Child1" },
                                                             new Node { Text = "Child2" },
                                                         }
                                      },
                              };


        }

        #endregion

        public static bool GetIsRootNode(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRootNodeProperty);
        }

        public static void SetIsRootNode(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRootNodeProperty, value);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            SetIsRootNode(element, true);
            //SetMainWidth(element, this.Width);
            base.PrepareContainerForItemOverride(element, item);
        }

        private void RootNodeTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var x = Template;
        }
    }
}