using cl.uv.multimodalvisualizer.src.dbcontext;
using cl.uv.multimodalvisualizer.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.multimodalvisualizer.src.view.window
{
    /// <summary>
    /// Interaction logic for PostureCRUD.xaml
    /// </summary>
    public partial class PostureCRUD : Window
    {
        public PostureCRUD()
        {
            InitializeComponent();
            refreshList();
        }

        private void addPostureButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPostureType addPostureView = new AddOrEditPostureType();
            addPostureView.Show();
        }
        public void refreshList()
        {
            postureCrudDataGrid.ItemsSource = PostureTypeContext.db.PostureType.ToList();
        }

        private void removePostureButton_Click(object sender, RoutedEventArgs e)
        {
            PostureType selectedItem = (PostureType) postureCrudDataGrid.SelectedItem;
            if (selectedItem != PostureType.none)
            {
                PostureTypeContext.db.PostureType.Remove(PostureTypeContext.db.PostureType.FirstOrDefault(p => p.PostureTypeId == selectedItem.PostureTypeId));
                PostureTypeContext.db.SaveChanges();
                this.refreshList();
            }
        }

        private void editPostureButton_Click(object sender, RoutedEventArgs e)
        {
            PostureType selectedPosture = (PostureType)postureCrudDataGrid.SelectedItem;
            selectedPosture = PostureTypeContext.db.PostureType.FirstOrDefault(p => p.PostureTypeId == selectedPosture.PostureTypeId);
            AddOrEditPostureType addPostureView = new AddOrEditPostureType(selectedPosture);
            addPostureView.Show();
        }
    }
}
