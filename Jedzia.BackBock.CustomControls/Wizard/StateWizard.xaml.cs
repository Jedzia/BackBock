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
            //this.Initialized += new EventHandler(StateWizard_Initialized);
            this.InitializeComponent();

            // Insert code required on object creation below this point.
            //this.btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            //this.btnPrevious.Click += new RoutedEventHandler(btnPrevious_Click);
            //this.btnNext.Click += new RoutedEventHandler(btnNext_Click);
            //this.detail.SelectionChanged += new SelectionChangedEventHandler(detail_SelectionChanged);
        }

        void StateWizard_Initialized(object sender, EventArgs e)
        {
            MessageBox.Show("Ini");
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

        /*void detail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedIndex = detail.SelectedIndex;
        }*/

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.btnNext.IsEnabled = true;
        }



        public object[] Pages
        {
            get { return (object[])GetValue(PageProperty); }
            set
            {
                SetValue(PageProperty, value);
                MessageBox.Show("Pages");

                //if (value)
                {
                }
            }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Pages", typeof(object[]), 
            typeof(StateWizard), new UIPropertyMetadata(null));


        public object XContent { get; set; }
        // public static readonly DependencyProperty ContentProperty;

        public void Blabla()
        {
            ContentControl bla;
            //bla.Content
            //this.OnPropertyChanged
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == PageProperty)
            {
                this.detail.Items.Clear();
                int x = 0;
                foreach (var item in Pages)
                {
                    this.detail.Items.Add(new TabItem() { Content = item, Header = "Item Nr." + x });
                    x++;
                }
            }
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