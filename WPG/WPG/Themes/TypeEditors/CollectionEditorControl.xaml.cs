using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.WpfDesign.Designer.PropertyGrid.Editors.BrushEditor;
using WPG.Data;
using WPG.TypeEditors;

namespace WPG.Themes.TypeEditors
{
    public partial class CollectionEditorControl : UserControl
    {


        public Property MyProperty
        {
            get { return (Property)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(Property), typeof(CollectionEditorControl), new UIPropertyMetadata(null));

        
        public Type NumerableType
        {
            get { return (Type)GetValue(NumerableTypeProperty); }
            set { SetValue(NumerableTypeProperty, value); }
        }

        public static readonly DependencyProperty NumerableTypeProperty =
            DependencyProperty.Register("NumerableType", typeof(Type), typeof(CollectionEditorControl), new UIPropertyMetadata(null), ValidateNumerableTypeProperty);

        public static bool ValidateNumerableTypeProperty(object value)
        {
            return true;
        }

        public IEnumerable NumerableValue
        {
            get { return (IEnumerable)GetValue(NumerableValueProperty); }
            set { SetValue(NumerableValueProperty, value); }
        }

        public static readonly DependencyProperty NumerableValueProperty =
            DependencyProperty.Register("NumerableValue", typeof(IEnumerable), typeof(CollectionEditorControl), new UIPropertyMetadata(null));

        public string NumerableTypeIdentifier
        {
            get { return (string)GetValue(NumerableTypeIdentifierProperty); }
            set { SetValue(NumerableTypeIdentifierProperty, value); }
        }

        public static readonly DependencyProperty NumerableTypeIdentifierProperty =
            DependencyProperty.Register("NumerableTypeIdentifier", typeof(string), typeof(CollectionEditorControl), new UIPropertyMetadata(null));

        public CollectionEditorControl()
        {
            InitializeComponent();            
            //txtTypeName.Text = NumerableType.GetType().ToString();
        }

        public ICommand AddTypeCommand
        {
            get { return (ICommand)GetValue(AddTypeCommandProperty); }
            set { SetValue(AddTypeCommandProperty, value); }
        }

        public static readonly DependencyProperty AddTypeCommandProperty =
            DependencyProperty.Register("AddTypeCommand", typeof(ICommand), typeof(CollectionEditorControl), new UIPropertyMetadata(null));

        public ICommand RemoveTypeCommand
        {
            get { return (ICommand)GetValue(RemoveTypeCommandProperty); }
            set { SetValue(RemoveTypeCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveTypeCommandProperty =
            DependencyProperty.Register("RemoveTypeCommand", typeof(ICommand), typeof(CollectionEditorControl), new UIPropertyMetadata(null));

        public bool AddToCollection
        {
            get { return (bool)GetValue(AddToCollectionProperty); }
            set { SetValue(AddToCollectionProperty, value); }
        }

        public static readonly DependencyProperty AddToCollectionProperty =
            DependencyProperty.Register("AddToCollection", typeof(bool), typeof(CollectionEditorControl), new UIPropertyMetadata(true));

        public Visibility TextBoxVisible
        {
            get { return (Visibility)GetValue(TextBoxVisibleProperty); }
            set { SetValue(TextBoxVisibleProperty, value); }
        }

        public static readonly DependencyProperty TextBoxVisibleProperty =
            DependencyProperty.Register("TextBoxVisible", typeof(Visibility), typeof(CollectionEditorControl), new UIPropertyMetadata(Visibility.Visible));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CollectionEditorWindow collEdt = new CollectionEditorWindow(this);
            collEdt.ShowDialog();
        }

        
    }
}
