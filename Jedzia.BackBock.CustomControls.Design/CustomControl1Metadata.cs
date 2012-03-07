// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

//extern alias Silverlight;
using System.ComponentModel;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using Jedzia.BackBock.CustomControls;
using Jedzia.BackBock.CustomControls.Design.Common;
using System.Windows;
//using SSWCD = Silverlight::System.Windows.Controls.DataVisualization;
//using SSWCDC = Silverlight::System.Windows.Controls.DataVisualization.Charting;

namespace Jedzia.BackBock.CustomControls.Design
{
    /// <summary>
    /// To register design time metadata for SSWCD.Legend.
    /// </summary>
    internal class CustomControl1Metadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCD.Legend.
        /// </summary>
        public CustomControl1Metadata()
            : base()
        {
            AddCallback(
                typeof(CustomControl1),
                b =>
                {
                    /*b.AddCustomAttributes(
                        Extensions.GetMemberName<CustomControl1>(x => x.Items),
                        new BrowsableAttribute(false));*/

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<CustomControl1>(x => x.Title),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    CategoryAttribute calendarCategory = new CategoryAttribute("Arschloch");
                    b.AddCustomAttributes(CustomControl1.MyPropertyProperty, calendarCategory);

                    MessageBox.Show("LegendMetadata AddCallback");
                    /*b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.Items),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.TitleStyle),
                        new CategoryAttribute(Properties.Resources.DataVisualizationStyling));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.Title),
                        PropertyValueEditor.CreateEditorAttribute(typeof(TextBoxEditor)));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.Items),
                        new NewItemTypesAttribute(typeof(SSWCDC.LegendItem)));

                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.Items)));*/

#if MWD40
                    /*b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.Title),
                        new AlternateContentPropertyAttribute());

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.DataVisualizationControlParts, false));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCD.Legend>(x => x.TitleStyle),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWCD.Legend>(x => x.Title),
                            false));*/
#endif
                });
        }
    }
}