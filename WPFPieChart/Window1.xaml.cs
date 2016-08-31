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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using ScottLogic.Shapes;

namespace WPFPieChart
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ObservableCollection<AssetClass> classes;

        public Window1()
        {   
            InitializeComponent();

            // create our test dataset and bind it
            classes = new ObservableCollection<AssetClass>(AssetClass.ConstructTestData());                          
            this.DataContext = classes;                 
        }

        /// <summary>
        /// Handle clicks on the listview column heading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumn column = ((GridViewColumnHeader)e.OriginalSource).Column;
            piePlotter.PlottedProperty = column.Header.ToString();
        }

        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            AssetClass asset = new AssetClass() { Class = "new class"};
            classes.Add(asset);
        }

    }
}
