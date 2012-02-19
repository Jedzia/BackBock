using System;
using System.Collections.Generic;
using System.Windows;

namespace Jedzia.BackBock.Application
{
    internal static class ThemeManager
    {
        internal static string CurrentSkin;
        private static Uri currentUri;

        internal static List<string> GetSkins()
        {
            List<string> skins = new List<string>();

            //TODO: make this dynamic by checking against WittySkins assembly
            skins.Add("");
            skins.Add("ExpressionDark");
            skins.Add("JedBlack");
            skins.Add("ShinyRed");
            //skins.Add("Felix");

            return skins;
        }

        internal static void ChangeTheme(string theme)
        {
            var app = System.Windows.Application.Current;
            if (string.IsNullOrEmpty(theme))
            {
                if (currentUri != null)
                {
                    var oldtheme = app.Resources.MergedDictionaries[app.Resources.MergedDictionaries.Count - 1];
                    //FindName(currentUri.OriginalString) as ResourceDictionary;
                    app.Resources.MergedDictionaries.Remove(oldtheme);
                }
                CurrentSkin = null;
                currentUri = null;
                return;
            }

            Uri resourceLocator = new Uri("BackBock;Component/Themes/" + theme + "/Theme.xaml", UriKind.Relative);
            //Application.Current.Resources =  Application.LoadComponent(resourceLocator) as ResourceDictionary;
            //var dictionary = System.Windows.Application.LoadComponent(resourceLocator) as ResourceDictionary;
            var dictionary = new ResourceDictionary();
            dictionary.Source = resourceLocator;
            if (currentUri != null)
            {
                var oldtheme = app.Resources.MergedDictionaries[app.Resources.MergedDictionaries.Count - 1];
                //FindName(currentUri.OriginalString) as ResourceDictionary;
                app.Resources.MergedDictionaries.Remove(oldtheme);
            }

            app.Resources.MergedDictionaries.Add(dictionary);
            //app.Resources = dictionary;
            CurrentSkin = theme;
            currentUri = resourceLocator;
        }

        internal static void ChangeThemeA(string theme)
        {
            var app = System.Windows.Application.Current;

            Uri resourceLocator = new Uri("NetUml;Component/Themes/" + theme + "/Theme.xaml", UriKind.Relative);
            System.Windows.Application.Current.Resources.MergedDictionaries[0] = System.Windows.Application.LoadComponent(resourceLocator) as ResourceDictionary;
            CurrentSkin = theme;
            currentUri = resourceLocator;
        }
    }
}
