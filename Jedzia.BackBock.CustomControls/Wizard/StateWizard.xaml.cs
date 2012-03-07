using System;
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
using System.Windows.Shapes;

namespace Jedzia.BackBock.CustomControls.Wizard
{
    /// <summary>
    /// Interaction logic for StateWizard.xaml
    /// </summary>
    public partial class StateWizard : ContentControl
    {
        public StateWizard()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }



        public object[] Pages
        {
            get { return (object[])GetValue(PageProperty); }
            set
            {
                SetValue(PageProperty, value);
                //if (value)
                {
                    this.btnNext.IsEnabled = true;
                    foreach (var item in value)
                    {
                        this.detail.Items.Add(item);
                    }
                }
            }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Pages", typeof(object[]), typeof(StateWizard), new UIPropertyMetadata(null));


        public object XContent { get; set; }
        // public static readonly DependencyProperty ContentProperty;

        public void Blabla()
        {
            ContentControl bla;
            //bla.Content
        }
    }
}