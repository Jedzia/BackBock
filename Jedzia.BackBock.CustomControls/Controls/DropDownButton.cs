namespace Jedzia.BackBock.CustomControls.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    // http://www.google.de/search?sclient=psy-ab&hl=en&site=&source=hp&q=%2Bwpf+button+with+menu&btnG=Search
    // this is from http://andyonwpf.blogspot.com/2006/10/dropdownbuttons-in-wpf.html
    // more on 
    // http://stackoverflow.com/questions/2975244/button-with-menu
    // http://www.codeproject.com/KB/WPF/WpfSplitButton.aspx?msg=3055930
    // http://anothersplitbutton.codeplex.com/
    public class DropDownButton : ToggleButton
    {
        // *** Dependency Properties ***

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(DropDownButton), new UIPropertyMetadata(null));

        // *** Constructors ***

        public DropDownButton()
        {
            // Bind the ToogleButton.IsChecked property to the drop-down's IsOpen property

            Binding binding = new Binding("DropDown.IsOpen");
            binding.Source = this;
            this.SetBinding(IsCheckedProperty, binding);
        }

        // *** Properties ***

        public ContextMenu DropDown
        {
            get
            {
                return (ContextMenu)GetValue(DropDownProperty);
            }
            set
            {
                SetValue(DropDownProperty, value);
            }
        }

        // *** Overridden Methods ***

        protected override void OnClick()
        {
            if (DropDown != null)
            {
                // If there is a drop-down assigned to this button, then position and display it

                DropDown.PlacementTarget = this;
                DropDown.Placement = PlacementMode.Bottom;

                DropDown.IsOpen = true;
            }
        }
    }
}
