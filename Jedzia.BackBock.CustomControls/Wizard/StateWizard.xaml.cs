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
            this.btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            this.btnPrevious.Click += new RoutedEventHandler(btnPrevious_Click);
            this.btnNext.Click += new RoutedEventHandler(btnNext_Click);
            this.detail.SelectionChanged += new SelectionChangedEventHandler(detail_SelectionChanged);
        }

        void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.detail.SelectedIndex--;
        }

        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.detail.SelectedIndex++;
            this.btnPrevious.IsEnabled = true;
        }

        void detail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedIndex = detail.SelectedIndex;
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.btnNext.IsEnabled = true;
            int x = 0;
            foreach (var item in Pages)
            {
                this.detail.Items.Add(new TabItem() { Content = item, Header = "Item Nr." + x });
                x++;
            }
        }



        public object[] Pages
        {
            get { return (object[])GetValue(PageProperty); }
            set
            {
                SetValue(PageProperty, value);
                //if (value)
                {
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



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(StateWizard), new UIPropertyMetadata(0));

    }
}