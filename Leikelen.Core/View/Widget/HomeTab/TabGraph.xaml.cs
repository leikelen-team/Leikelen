using cl.uv.leikelen.API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using cl.uv.leikelen.Data.Access.Internal;

namespace cl.uv.leikelen.View.Widget.HomeTab
{
    /// <summary>
    /// Interaction logic for TabGraph.xaml
    /// </summary>
    public partial class TabGraph : TabItem, ITab
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabGraph"/> class.
        /// </summary>
        public TabGraph()
        {
            InitializeComponent();
        }

        void ITab.Fill()
        {
            StackButtons.Visibility = Visibility.Visible;
            if (!ReferenceEquals(null, SceneInUse.Instance.Scene))
            {
                var widget = new Widget.EventGraphControl();
                widget.MinHeight = GeneralSettings.Instance.EventsGraphMinHeight.Value;
                MainStack.Children.Add(widget);
            }
                
        }

        void ITab.Reset()
        {
            MainStack.Children.RemoveRange(1, MainStack.Children.Count - 1);
            StackButtons.Visibility = Visibility.Hidden;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(null, SceneInUse.Instance.Scene))
            {
                var widget = new Widget.EventGraphControl();
                widget.MinHeight = GeneralSettings.Instance.EventsGraphMinHeight.Value;
                MainStack.Children.Add(widget);
            }
        }

        private void RemoveLastBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(null, SceneInUse.Instance.Scene) && MainStack.Children.Count > 1)
                MainStack.Children.RemoveAt(MainStack.Children.Count - 1);
        }
    }
}
