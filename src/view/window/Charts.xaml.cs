using cl.uv.leikelen.src.model;
using cl.uv.leikelen.src.controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFPieChart;

namespace cl.uv.leikelen.src.view.window
{
    /// <summary>
    /// Interaction logic for Charts.xaml
    /// </summary>
    public partial class Charts : Window
    {
        public Charts()
        {
            InitializeComponent();
            PersonsComboBox.SelectionChanged += PersonsComboBox_SelectionChanged;
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
    }
}
