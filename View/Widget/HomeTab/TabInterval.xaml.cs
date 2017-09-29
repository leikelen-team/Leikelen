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
    /// Lógica de interacción para TabInterval.xaml
    /// </summary>
    public partial class TabInterval : TabItem, ITab
    {
        public TabInterval()
        {
            InitializeComponent();
        }

        public void Fill()
        {
            StackButtons.Visibility = Visibility.Visible;
            if (!ReferenceEquals(null, SceneInUse.Instance.Scene))
            {
                var widget = new Widget.IntervalGraphControl();
                widget.MinHeight = GeneralSettings.Instance.IntervalsGraphMinHeight.Value;
                MainStack.Children.Add(widget);
            }
        }

        public void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(null, SceneInUse.Instance.Scene))
            {
                var widget = new Widget.IntervalGraphControl();
                widget.MinHeight = GeneralSettings.Instance.IntervalsGraphMinHeight.Value;
                MainStack.Children.Add(widget);
            }
        }

        public void RemoveLastBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!ReferenceEquals(null, SceneInUse.Instance.Scene) && MainStack.Children.Count > 1)
                MainStack.Children.RemoveAt(MainStack.Children.Count - 1);
        }
    }
}
