using System;
using System.Collections;
using System.Windows;
using WPG.Themes.TypeEditors;
using System.ComponentModel;

namespace WPG.TypeEditors
{
    /// <summary>
    /// Interaction logic for CollectionEditor.xaml
    /// </summary>
    public partial class CollectionEditorWindow : Window
    {
        public CollectionEditorControl baseControl { get; set; }

        public bool HasDefaultConstructor(Type type)
        {
            if (type.IsValueType)
                return true;

            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
                return false;

            return true;
        }
        
        public CollectionEditorWindow(CollectionEditorControl ctrl)
        {
            InitializeComponent();
            baseControl = ctrl;
            this.DataContext = baseControl.DataContext;

            foreach (var tmp in baseControl.NumerableValue)
            {
                myLst.Items.Add(tmp);
            }

            //Visibilty of cmdAdd

            if (baseControl.MyProperty == null)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(baseControl.DataContext);
                baseControl.MyProperty = new WPG.Data.Property(
                    baseControl.DataContext, properties[baseControl.NumerableTypeIdentifier]);

                //cmdAdd.Visibility = Visibility.Collapsed;
                //cmdRemove.Visibility = Visibility.Collapsed;
                //return;
            }
            // Todo: check generic list type

            //var aa = baseControl.MyProperty.PropertyType.GetGenericArguments();
            if (baseControl.MyProperty.PropertyType.GetGenericArguments().Length > 0)
                if (!HasDefaultConstructor(baseControl.MyProperty.PropertyType.GetGenericArguments()[0]) || baseControl.MyProperty.IsReadOnly)
                {
                    cmdAdd.Visibility = Visibility.Collapsed;
                }

            if (baseControl.MyProperty.IsReadOnly)
                cmdRemove.Visibility = Visibility.Collapsed;


            //myLst.ItemsSource = baseControl.NumerableValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void myLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            myGrid.Instance = myLst.SelectedItem;
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            object newElem = null;
            if (baseControl.MyProperty.PropertyType.IsGenericType)
            {
                Type[] typeArguments = baseControl.MyProperty.PropertyType.GetGenericArguments();
                newElem = System.Activator.CreateInstance(typeArguments[0]);
                if (baseControl.AddToCollection)
                    baseControl.MyProperty.AddCollectionElement(newElem);
            }
            else if (baseControl.MyProperty.PropertyType.BaseType.IsGenericType)
            {
                Type[] typeArguments = baseControl.MyProperty.PropertyType.BaseType.GetGenericArguments();
                newElem = System.Activator.CreateInstance(typeArguments[0]);
                if (baseControl.AddToCollection)
                    baseControl.MyProperty.AddCollectionElement(newElem);
            }
            else
            {
                newElem = System.Activator.CreateInstance(baseControl.MyProperty.PropertyType.GetGenericArguments()[0]);
            }

            if (baseControl.AddTypeCommand != null)
            {
                baseControl.AddTypeCommand.Execute(newElem);
            }
            myLst.Items.Add(newElem);
        }

        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            if (myLst.SelectedItem != null)
            {
                var rem = myLst.SelectedItem;
                if (baseControl.MyProperty.PropertyType.IsGenericType)
                {
                    if (baseControl.AddToCollection)
                        baseControl.MyProperty.RemoveCollectionElement(rem);
                }
                else if (baseControl.MyProperty.PropertyType.BaseType.IsGenericType)
                {
                    if (baseControl.AddToCollection)
                        baseControl.MyProperty.RemoveCollectionElement(rem);
                }

                myLst.Items.Remove(myLst.SelectedItem);

                if (baseControl.RemoveTypeCommand != null)
                {
                    baseControl.RemoveTypeCommand.Execute(rem);
                }
            }
            myGrid.Instance = null;
        }
    }
}
