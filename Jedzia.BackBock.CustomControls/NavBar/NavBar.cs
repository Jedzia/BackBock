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
    using System.Windows.Threading;

    public class CustomResources
    {
        public static readonly ComponentResourceKey NavBarTreeHeaderBackBrushKey = new ComponentResourceKey(
                  typeof(CustomResources), "NavBarTreeHeaderBackBrush");


        /*public static ComponentResourceKey RootNodeTreeViewToggleButtonKey
        {
            get
            {
                return new ComponentResourceKey(
                  typeof(CustomResources), "RootNodeTreeViewToggleButton");
            }
        }*/

        public static ComponentResourceKey RootNodeTreeViewToggleButtonKey = new ComponentResourceKey(
                  typeof(CustomResources), "RootNodeTreeViewToggleButton");

        public static ComponentResourceKey DebugColorKey = new ComponentResourceKey(
              typeof(CustomResources), "DebugColor");

        public static ComponentResourceKey DebugKey = new ComponentResourceKey(
           typeof(CustomResources), "Debug");
    }

    public class NavBar : TabControl
    {
        #region Constructors

        private delegate void ChangeTheme(ResourceDictionary destination, bool reinitDestination);
        static NavBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavBar), new FrameworkPropertyMetadata(typeof(NavBar)));
            var x = SystemColors.WindowBrush;
            return;
            /*ResourceDictionary destination = Application.Current.Resources;
            bool reinitDestination = false;
            Application.Current.Dispatcher.BeginInvoke(
                (ChangeTheme)((d, reinit) =>
                {
                    d.BeginInit();
                    d.MergedDictionaries.Add(NavBarItem.mres);
                    //d.MergedDictionaries.RemoveAt(0);
                    d.EndInit();
                }
                             ),
                DispatcherPriority.ApplicationIdle,
                destination,
                reinitDestination);*/
        }

        public NavBar()
        {
            //InitializeComponent();
            if (NavBarItem.USEmres)
            {
                var res = NavBarItem.mres["NavBarStyle"] as Style;
                Style = res;
            }
            TabStripPlacement = Dock.Left;
            //this.Resources.MergedDictionaries.Add(NavBarItem.mres);
            this.Loaded += NavBar_Loaded;
        }

        #endregion

        private void NavBar_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= this.NavBar_Loaded;
            //this.Resources
        }

        /*protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            //SetIsRootNode(element, true);
            base.PrepareContainerForItemOverride(element, item);
        }*/
    }
}