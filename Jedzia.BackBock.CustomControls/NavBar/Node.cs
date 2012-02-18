namespace Jedzia.BackBock.CustomControls.NavBar
{
    using System.Collections.Generic;

    public class Node
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public IList<object> Children { get; set; }
    }

    //[TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
    //[TemplatePart(Name = "PART_HeaderScroll", Type = typeof(ScrollViewer))]
    //[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TabItem))]


    //[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeViewItem))]
}
