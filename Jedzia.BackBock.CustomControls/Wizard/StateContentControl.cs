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
    public class StateContentControl : TabItem
    {
        static StateContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StateContentControl), new FrameworkPropertyMetadata(typeof(StateContentControl)));
        }

        public StateContentControl()
        {
            // Insert code required on object creation below this point.
        }
    }
}