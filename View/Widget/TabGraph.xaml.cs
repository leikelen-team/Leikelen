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
using LiveCharts;
using LiveCharts.Wpf;

namespace cl.uv.leikelen.View.Widget
{
    /// <summary>
    /// Lógica de interacción para TabGraph.xaml
    /// </summary>
    public partial class TabGraph : TabItem
    {
        public TabGraph()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Title = "Seated",
                    Values = new ChartValues<int> {25, 20, 30, 15},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedRowSeries
                {
                    Title = "Hand Open",
                    Values = new ChartValues<int> {25,7 , 40, 15},
                    StackMode = StackMode.Values,
                    DataLabels = true
                },
                new StackedRowSeries
                {
                    Title = "Standed",
                    Values = new ChartValues<int> {25, 10, 15, 10},
                    StackMode = StackMode.Values,
                    DataLabels = true,                      
                },
            };


            /*
            //adding series updates and animates the chart
            SeriesCollection.Add(new StackedRowSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Values
            });
            
            //adding values also updates and animates
            SeriesCollection[2].Values.Add(4d);            
            */
            Labels = new[] { "Person 1", "Person 2", "Person 3", "Person 4" };
            YFormatter = value => value + " Horas";
            DataContext = this;

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
