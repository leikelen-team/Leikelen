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
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.View.Widget.PreferencesTab
{
    /// <summary>
    /// Lógica de interacción para PreferencesPersonColors.xaml
    /// </summary>
    public partial class PreferencesPersonColors : TabItem, IPreference
    {
        public PreferencesPersonColors()
        {
            InitializeComponent();

            p1Label.Content = Properties.GUI.Person + " 1";
            p2Label.Content = Properties.GUI.Person + " 2";
            p3Label.Content = Properties.GUI.Person + " 3";
            p4Label.Content = Properties.GUI.Person + " 4";
            p5Label.Content = Properties.GUI.Person + " 5";
            p6Label.Content = Properties.GUI.Person + " 6";

            List<Rectangle> rectangleColors = new List<Rectangle>();
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Black });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.AliceBlue });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.AntiqueWhite });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Aqua });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Aquamarine });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Azure });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Beige });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Bisque });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.BlanchedAlmond });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Blue });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.BlueViolet });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Brown });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.BurlyWood });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.CadetBlue });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.Chartreuse });
            rectangleColors.Add(new Rectangle() { Fill = Brushes.White });
            
            p1BonesCmbx.ItemsSource = rectangleColors;
            p2BonesCmbx.ItemsSource = rectangleColors;
            p3BonesCmbx.ItemsSource = rectangleColors;
            p4BonesCmbx.ItemsSource = rectangleColors;
            p5BonesCmbx.ItemsSource = rectangleColors;
            p6BonesCmbx.ItemsSource = rectangleColors;

            p1DotsCmbx.ItemsSource = rectangleColors;
            p2DotsCmbx.ItemsSource = rectangleColors;
            p3DotsCmbx.ItemsSource = rectangleColors;
            p4DotsCmbx.ItemsSource = rectangleColors;
            p5DotsCmbx.ItemsSource = rectangleColors;
            p6DotsCmbx.ItemsSource = rectangleColors;
        }

        public void Apply()
        {
        }
    }
}
