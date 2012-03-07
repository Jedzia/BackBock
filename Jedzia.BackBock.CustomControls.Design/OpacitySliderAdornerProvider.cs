using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Windows.Design.Model;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Windows.Design.Interaction;

namespace Jedzia.BackBock.CustomControls.Design
{
    // The following class implements an adorner provider for the 
    // adorned control. The adorner is a slider control, which 
    // changes the Background opacity of the adorned control.
    class OpacitySliderAdornerProvider : PrimarySelectionAdornerProvider
    {
        private static readonly DependencyProperty changeProperty = CustomControl1.OpacityProperty;
        private ModelItem adornedControlModel;
        private ModelEditingScope batchedChange;
        private Slider opacitySlider;
        private AdornerPanel opacitySliderAdornerPanel;

        public OpacitySliderAdornerProvider()
        {
            opacitySlider = new Slider();
            //MessageBox.Show("OpacitySliderAdornerProvider Ctor");
        }

        // The following method is called when the adorner is activated.
        // It creates the adorner control, sets up the adorner panel,
        // and attaches a ModelItem to the adorned control.
        protected override void Activate(ModelItem item, DependencyObject view)
        {
            //MessageBox.Show("OpacitySliderAdornerProvider Activate");

            // Save the ModelItem and hook into when it changes.
            // This enables updating the slider position when 
            // a new Background value is set.
            adornedControlModel = item;
            adornedControlModel.PropertyChanged +=
                new System.ComponentModel.PropertyChangedEventHandler(
                    AdornedControlModel_PropertyChanged);

            // Setup the slider's min and max values.
            opacitySlider.Minimum = 0;
            opacitySlider.Maximum = 1;

            // Setup the adorner panel.
            // All adorners are placed in an AdornerPanel
            // for sizing and layout support.
            AdornerPanel myPanel = this.Panel;

            AdornerPanel.SetHorizontalStretch(opacitySlider, AdornerStretch.Stretch);
            AdornerPanel.SetVerticalStretch(opacitySlider, AdornerStretch.None);

            AdornerPlacementCollection placement = new AdornerPlacementCollection();

            // The adorner's width is relative to the content.
            // The slider extends the full width of the control it adorns.
            placement.SizeRelativeToContentWidth(1.0, 0);

            // The adorner's height is the same as the slider's.
            placement.SizeRelativeToAdornerDesiredHeight(1.0, 0);

            // Position the adorner above the control it adorns.
            placement.PositionRelativeToAdornerHeight(-1.0, 0);

            // Position the adorner up 5 pixels. This demonstrates 
            // that these placement calls are additive. These two calls
            // are equivalent to the following single call:
            // PositionRelativeToAdornerHeight(-1.0, -5).
            placement.PositionRelativeToAdornerHeight(0, -5);

            AdornerPanel.SetPlacements(opacitySlider, placement);

            // Initialize the slider when it is loaded.
            opacitySlider.Loaded += new RoutedEventHandler(slider_Loaded);

            // Handle the value changes of the slider control.
            opacitySlider.ValueChanged +=
                new RoutedPropertyChangedEventHandler<double>(
                    slider_ValueChanged);

            opacitySlider.PreviewMouseLeftButtonUp +=
                new System.Windows.Input.MouseButtonEventHandler(
                    slider_MouseLeftButtonUp);

            opacitySlider.PreviewMouseLeftButtonDown +=
                new System.Windows.Input.MouseButtonEventHandler(
                    slider_MouseLeftButtonDown);

            base.Activate(item, view);
        }

        // The Panel utility property demand-creates the 
        // adorner panel and adds it to the provider's 
        // Adorners collection.
        public AdornerPanel Panel
        {
            get
            {
                if (this.opacitySliderAdornerPanel == null)
                {
                    opacitySliderAdornerPanel = new AdornerPanel();

                    opacitySliderAdornerPanel.Children.Add(opacitySlider);

                    // Add the panel to the Adorners collection.
                    Adorners.Add(opacitySliderAdornerPanel);
                }

                return this.opacitySliderAdornerPanel;
            }
        }


        // The following method deactivates the adorner.
        protected override void Deactivate()
        {
            adornedControlModel.PropertyChanged -=
                new System.ComponentModel.PropertyChangedEventHandler(
                    AdornedControlModel_PropertyChanged);
            base.Deactivate();
        }

        // The following method handles the PropertyChanged event.
        // It updates the slider control's value if the adorned control's 
        // Background property changed,
        void AdornedControlModel_PropertyChanged(
            object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Background")
            {
                opacitySlider.Value = GetCurrentOpacity();
            }
        }

        // The following method handles the Loaded event.
        // It assigns the slider control's initial value.
        void slider_Loaded(object sender, RoutedEventArgs e)
        {
            opacitySlider.Value = GetCurrentOpacity();
        }

        // The following method handles the MouseLeftButtonDown event.
        // It calls the BeginEdit method on the ModelItem which represents 
        // the adorned control.
        void slider_MouseLeftButtonDown(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            batchedChange = adornedControlModel.BeginEdit();
        }

        // The following method handles the MouseLeftButtonUp event.
        // It commits any changes made to the ModelItem which represents the
        // the adorned control.
        void slider_MouseLeftButtonUp(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            if (batchedChange != null)
            {
                batchedChange.Complete();
                batchedChange.Dispose();
                batchedChange = null;
            }
        }

        // The following method handles the slider control's 
        // ValueChanged event. It sets the value of the 
        // Background opacity by using the ModelProperty type.
        void slider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            double newOpacityValue = e.NewValue;

            // During setup, don't make a value local and set the opacity.
            if (newOpacityValue == GetCurrentOpacity())
            {
                return;
            }
            //return;
            // Access the adorned control's Background property
            // by using the ModelProperty type.
            ModelProperty backgroundProperty =
                adornedControlModel.Properties[changeProperty];
            if (!backgroundProperty.IsSet)
            {
                // If the value isn't local, make it local 
                // before setting a sub-property value.
                backgroundProperty.SetValue(backgroundProperty.ComputedValue);
            }

            // Set the Opacity property on the Background Brush.
            backgroundProperty.SetValue(newOpacityValue);
        }

        // This utility method gets the adorned control's
        // Background brush by using the ModelItem.
        private double GetCurrentOpacity()
        {
            double opacity =
                (double)adornedControlModel.Properties[changeProperty].ComputedValue;
            //var bb = adornedControlModel.Properties[CustomControl1.OpacityProperty].ComputedValue;
            /*var bb = opacity;

            if (bb == null)
            {
                MessageBox.Show("bb is null");

            }
            else
            {
                MessageBox.Show(bb.ToString());

            }*/
            //return 55;
            return opacity;
        }
    }


}
