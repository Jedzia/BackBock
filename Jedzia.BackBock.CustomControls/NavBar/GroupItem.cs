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
    using System.Windows.Threading;

    public class GroupItem : TabItem
    {
        #region Constructors

        static GroupItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(typeof(GroupItem)));
            return;
            /*ResourceDictionary destination = Application.Current.Resources;
            bool reinitDestination = false;
            Application.Current.Dispatcher.BeginInvoke(
                (ChangeTheme)((d, reinit) =>
                                  {
                                      d.BeginInit();
                                      d.MergedDictionaries.Add(
                                          new ResourceDictionary
                                              {
                                                  Source =
                                                      new Uri(
                                                      "pack://application:,,,/Jedzia.BackBock.CustomControls.NavBar;component/NavBar/NavBarStyles.xaml")
                                              });
                                      //d.MergedDictionaries.RemoveAt(0);
                                      d.EndInit();
                                  }
                             ),
                DispatcherPriority.ApplicationIdle,
                destination,
                reinitDestination);*/
        }

        //static NavBarStyles n = new NavBarStyles();
        public GroupItem()
        {
            //InitializeComponent();

            if (NavBarItem.USEmres)
            {
                var res = NavBarItem.mres["GroupItemStyle"] as Style;
                Style = res;
            }
            //this.IsEnabled = false;
            this.ApplyTemplate();
            //this.Loaded += new RoutedEventHandler(GroupItem_Loaded);
        }

        #endregion

        #region Delegates

        private delegate void ChangeTheme(ResourceDictionary destination, bool reinitDestination);

        #endregion

        private void GroupItem_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}