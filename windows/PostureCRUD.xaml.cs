using Microsoft.Samples.Kinect.VisualizadorMultimodal.db;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;
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

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.windows
{
    /// <summary>
    /// Interaction logic for PostureCRUD.xaml
    /// </summary>
    public partial class PostureCRUD : Window
    {
        public PostureCRUD()
        {
            InitializeComponent();

            //CollectionViewSource itemCollectionViewSource;
            //itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            //itemCollectionViewSource.Source = PostureType.availablesPostureTypes;
            postureCrudDataGrid.ItemsSource = PostureType.availablesPostureTypes;
        }

        private void addPostureButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPostureType addPostureView = new AddOrEditPostureType();
            addPostureView.Show();
        }
        public void refreshList()
        {
            postureCrudDataGrid.ItemsSource = PostureType.availablesPostureTypes;
        }

        private void removePostureButton_Click(object sender, RoutedEventArgs e)
        {
            int idx = postureCrudDataGrid.SelectedIndex;
            PostureType selectedItem = (PostureType) postureCrudDataGrid.SelectedItem;
            if (selectedItem != PostureType.none)
            {
                SqliteAppContext.db.PostureType.Remove(SqliteAppContext.db.PostureType.FirstOrDefault(p => p.id == selectedItem.id));
                SqliteAppContext.db.SaveChanges();
                this.refreshList();
            }
            
        }
    }
}
