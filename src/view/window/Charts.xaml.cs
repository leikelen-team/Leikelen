using cl.uv.multimodalvisualizer.src.model;
using cl.uv.multimodalvisualizer.src.view;
using cl.uv.multimodalvisualizer.src.controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFPieChart;

namespace cl.uv.multimodalvisualizer.src.view.window
{
    /// <summary>
    /// Interaction logic for Charts.xaml
    /// </summary>
    public partial class Charts : Window
    {
        //private ObservableCollection<ChartView> classes;
        public Charts()
        {
            InitializeComponent();
            PersonsComboBox.SelectionChanged += PersonsComboBox_SelectionChanged;
            //ComboBoxItem item;
            foreach(Person person in Scene.Instance.Persons)
            {
                PersonsComboBox.Items.Add(person);
            }
            PersonsComboBox.SelectedIndex = 0;
            this.DataContext = new ObservableCollection<ChartView>(ChartView.GenerateFromPerson((Person)PersonsComboBox.SelectedItem));
             
        }

        private void PersonsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = new ObservableCollection<ChartView>(ChartView.GenerateFromPerson((Person)PersonsComboBox.SelectedItem));
             
        }

        ///// <summary>
        ///// Handle clicks on the listview column heading
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        //{
        //    GridViewColumn column = ((GridViewColumnHeader)e.OriginalSource).Column;
        //    piePlotter.PlottedProperty = column.Header.ToString();
        //}

        //private void AddNewItem(object sender, RoutedEventArgs e)
        //{
        //    ChartView asset = new ChartView() { Class = "new class" };
        //    classes.Add(asset);
        //}
    }
}
